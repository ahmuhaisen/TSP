using Bogus.Bson;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz.Logging;
using System.Runtime;
using System.Text.Json;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Feedback.Contracts;
using TPS.Application.Areas.Shared.Events.Contracts;
using TPS.Infrastructure.AiClient;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Enums;
using TSP.Domain.Events;
using TSP.Domain.Shared;
using TSP.Domain.Shared.Options;

namespace TPS.Application.Areas.Feedback;


public class FeedbackService : IFeedbackService
{
    private readonly ApplicationDbContext _context;
    private readonly EventFeedbackOptions _options;
    private readonly IAiClientService _aiClientService;
    private readonly ILogger<FeedbackService> _logger;

    public FeedbackService(ApplicationDbContext context, IOptions<EventFeedbackOptions> options, IAiClientService aiClientService, ILogger<FeedbackService> logger)
    {
        _context = context;
        _options = options.Value;
        _aiClientService = aiClientService;
        _logger = logger;
    }


    public async Task<Result> SubmitFeedbackAsync(FeedbackAnswerRequest dto)
    {
        var @event = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == dto.EventId);

        if (@event is null)
        {
            var eventRequest = await _context.EventsApproval
                .AsNoTracking()
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.Id == dto.EventId || e.EventId == dto.EventId);

            if (eventRequest != null)
                @event = eventRequest.Event;
        }

        if (@event is null)
            return Result.Failure(Error.NotFound(nameof(Event), dto.EventId.ToString()));

        var now = DateTime.Now;
        var feedbackDeadline = @event.EndTime.AddDays(_options.DurationDays);

        if (now > feedbackDeadline)
            return Result.Failure(Error.CustomError("Feedback submission is closed."));

        var feedback = new FeedbackAnswer
        {
            EventId = @event.Id,
            Rating = dto.Rating,
            Notes = dto.Notes,
            SubmittedAt = now
        };

        _context.FeedbackAnswers.Add(feedback);

        feedback.RaiseDomainEvent(new FeedbackSubmittedDomainEvent(Guid.NewGuid(), feedback.EventId, feedback.EventId, feedback.Rating, feedback.Notes));

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<bool>> IsFeedbackOpenAsync(Guid eventId)
    {
        var @event = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId);

        if (@event is null)
        {
            var eventRequest = await _context.EventsApproval
                .AsNoTracking()
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventRequest != null)
                @event = eventRequest.Event;
        }

        if (@event is null)
            return Result.Failure<bool>(Error.NotFound(nameof(Event), eventId.ToString()));

        var now = DateTime.Now;
        return Result.Success(@event.EndTime <= now && now <= @event.EndTime.AddDays(_options.DurationDays));
    }

    public async Task UpdateSummaryForEventAsync(Guid eventId)
    {
        var feedbacks = await _context.FeedbackAnswers
            .Where(f => f.EventId == eventId)
            .ToListAsync();

        var average = feedbacks.Average(f => f.Rating);
        var total = feedbacks.Count;
        var notes = feedbacks
            .Where(f => !string.IsNullOrWhiteSpace(f.Notes))
            .Select(f => f.Notes!)
            .ToList();

        var feedbackText = string.Join("\n- ", notes);

        var prompt = """
        You are an AI assistant that analyzes student feedback from university events.

        You will receive a list of anonymous text feedback comments related to a single event. Your task is to analyze these and return a structured JSON response that reflects:

        ### TASKS:

        1. **Sentiment Classification**:
           - Classify the overall sentiment of the feedback using one of the following exact values:
             - "Positive"
             - "Mixed"
             - "Negative"
           - Choose the one that best describes the general tone and balance of the feedback.

        2. **Topic Extraction**:
           - Identify up to 5 recurring topics or themes mentioned in the feedback.
           - Return them as a comma-separated string using capitalized keywords (e.g., "Speaker, Venue, Timing").

        3. **Summary Text**:
           - Write a short natural language summary (5-6 sentences) that captures the overall strengths and weaknesses mentioned in the feedback.

        ### FORMAT:

        Return a JSON object that exactly matches the following structure:

        {{
          "Sentiment": "Positive",
          "Topics": "Speaker, Venue, Timing",
          "Summary": "Students appreciated the engaging speaker and informative content. A few mentioned the event started late but overall enjoyed the experience."
        }}

        ### INPUT FEEDBACK:
        - {feedbackText}
        """;

        prompt = prompt.Replace("{feedbackText}", feedbackText);

        var jsonResult = await _aiClientService.GetResponseAsync(prompt);

        jsonResult = SenetizeJsonResponse(jsonResult);

        _logger.LogInformation("AI response: {JsonResult}", jsonResult);

        var aiResult = JsonSerializer.Deserialize<FeedbackAnalysisResult>(jsonResult, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var existing = await _context.FeedbackSummaries
            .FirstOrDefaultAsync(f => f.EventId == eventId);

        if (existing is null)
        {
            var summary = new FeedbackSummary
            {
                EventId = eventId,
                AverageRating = average,
                TotalResponses = total,
                Sentiment = aiResult.Sentiment,
                Topics = aiResult.Topics,
                AiSummary = aiResult.Summary,
                CalculatedAt = DateTime.UtcNow
            };
            _context.FeedbackSummaries.Add(summary);
        }
        else
        {
            existing.AverageRating = average;
            existing.TotalResponses = total;
            existing.Sentiment = aiResult.Sentiment;
            existing.Topics = aiResult.Topics;
            existing.AiSummary = aiResult.Summary;
            existing.CalculatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<Result<EventFeedbackResponseDto>> GetEventFeedbackAsync(Guid eventId)
    {
        var @event = await GetEventAsync(eventId);

        if (@event is null)
            return Result.Failure<EventFeedbackResponseDto>(Error.NotFound(nameof(Event), eventId.ToString()));

        var now = DateTime.Now;
        var feedbackStartDate = @event.StartTime;

        if(now < feedbackStartDate)
            return Result.Failure<EventFeedbackResponseDto>(Error.CustomError("Feedback is not yet available."));

        var feedbacks = await _context.FeedbackAnswers
            .AsNoTracking()
            .Where(f => f.EventId == @event.Id)
            .OrderByDescending(f => f.SubmittedAt)
            //.Take(numberOfFeedbackAnswers)
            .ToListAsync();

        var summary = await _context.FeedbackSummaries
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.EventId == @event.Id);

        if (summary is null)
            return Result.Failure<EventFeedbackResponseDto>(Error.NotFound(nameof(FeedbackSummary), eventId.ToString()));

        var response = new EventFeedbackResponseDto
        {
            Event = new EventBasicDTO
            {
                Id = @event.Id,
                Name = @event.Name,
            },
            Summary = new FeedbackSummaryDto
            {
                SummaryId = summary.Id,
                AverageRating = summary.AverageRating,
                TotalResponses = summary.TotalResponses,
                Sentiment = summary.Sentiment,
                Topics = summary.Topics,
                AiSummary = summary.AiSummary,
                CalculatedAt = summary.CalculatedAt
            },
            Feedbacks = feedbacks.Select(f => new FeedbackAnswerDto
            {
                Rating = f.Rating,
                Notes = f.Notes,
                SubmittedAt = f.SubmittedAt
            }).ToList()
        };

        return Result.Success(response);
    }

    string SenetizeJsonResponse(string text)
    {
        var firstIndexOfPracet = text.IndexOf("{");
        var lastIndexOfPracet = text.LastIndexOf("}");

        return text.Substring(firstIndexOfPracet, lastIndexOfPracet - firstIndexOfPracet + 1);
    }

    async Task<Event> GetEventAsync(Guid id)
    {
        var @event = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);

        if (@event is null)
        {
            var eventRequest = await _context.EventsApproval
                .AsNoTracking()
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.Id == id || e.EventId == id);

            if (eventRequest != null)
                @event = eventRequest.Event;
        }

        return @event;
    }
}

public class EventFeedbackResponseDto
{
    public EventBasicDTO Event { get; set; } = null!;
    public FeedbackSummaryDto Summary { get; set; } = null!;
    public List<FeedbackAnswerDto> Feedbacks { get; set; } = null!;
}

public class FeedbackSummaryDto
{
    public Guid SummaryId { get; set; }
    public decimal AverageRating { get; set; }
    public int TotalResponses { get; set; }
    public Sentiment? Sentiment { get; set; }
    public string? Topics { get; set; }
    public string? AiSummary { get; set; }
    public DateTime CalculatedAt { get; set; }
}

public class FeedbackAnswerDto
{
    public decimal Rating { get; set; }
    public string? Notes { get; set; }
    public DateTime SubmittedAt { get; set; }
}