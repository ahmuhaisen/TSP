using TPS.Application.Areas.Feedback.Contracts;
using TSP.Domain.Shared;

namespace TPS.Application.Abstractions;

public interface IFeedbackService
{
    Task<Result> SubmitFeedbackAsync(FeedbackAnswerRequest dto);
    Task<Result<bool>> IsFeedbackOpenAsync(Guid eventId);
}

