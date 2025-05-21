using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Application.Areas.AdminArea.Events.Queries;
using TPS.Application.Areas.Feedback.Contracts;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers;

[ApiController]
[Route($"api/[controller]")]
public class FeedbackController(ISender sender, IFeedbackService _feedbackService) : ApiController(sender)
{
    [HttpPost]
    public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackAnswerRequest request)
    {
        var task = _feedbackService.SubmitFeedbackAsync(request);

        return await FromResult(task);
    }

    [HttpGet("is-open/{eventId}")]
    public async Task<IActionResult> IsFeedbackOpen(Guid eventId)
    {
        var task = _feedbackService.IsFeedbackOpenAsync(eventId);

        return await FromResult(task);
    }

    [HttpGet("events/{eventRequestId}")]
    public async Task<IActionResult> GetEventDetails([FromRoute] Guid eventRequestId)
    {
        var query = EventDetails.Query.Create(eventRequestId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("events/{eventId}/summary")]
    public async Task<IActionResult> GetEventFeedback([FromRoute] Guid eventId)
    {
        var task = _feedbackService.GetEventFeedbackAsync(eventId);

        return await FromResult(task);
    }

}
