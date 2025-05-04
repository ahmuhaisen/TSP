using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Feedback.Contracts;

namespace TSP.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route($"api/[controller]")]
public class FeedbackController : ApiController
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(ISender sender, IFeedbackService feedbackService) : base(sender)
    {
        _feedbackService = feedbackService;
    }

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
}
