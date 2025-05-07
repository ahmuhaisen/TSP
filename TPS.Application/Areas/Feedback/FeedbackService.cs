using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Feedback.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;
using TSP.Domain.Shared.Options;

namespace TPS.Application.Areas.Feedback;


public class FeedbackService : IFeedbackService
{
    private readonly ApplicationDbContext _context;
    private readonly EventFeedbackOptions _options;

    public FeedbackService(ApplicationDbContext context, IOptions<EventFeedbackOptions> options)
    {
        _context = context;
        _options = options.Value;
    }


    public async Task<Result> SubmitFeedbackAsync(FeedbackAnswerRequest dto)
    {
        var @event = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == dto.EventId);

        if (@event is null)
            return Result.Failure(Error.NotFound(nameof(Event), dto.EventId.ToString()));

        var now = DateTime.Now;
        var feedbackDeadline = @event.EndTime.AddDays(_options.DurationDays);

        if (now > feedbackDeadline)
            return Result.Failure(Error.CustomError("Feedback submission is closed."));

        var feedback = new FeedbackAnswer
        {
            EventId = dto.EventId,
            Rating = dto.Rating,
            Notes = dto.Notes,
            SubmittedAt = now
        };

        _context.FeedbackAnswers.Add(feedback);
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
}
