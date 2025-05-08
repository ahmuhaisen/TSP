using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TSP.Domain.Events;

namespace TPS.Application.Areas.Feedback.EventHandlers;


public sealed class FeedbackSubmittedEventHandler(
        ILogger<FeedbackSubmittedEventHandler> _logger,
        IFeedbackService _feedbackService
    )
    : INotificationHandler<FeedbackSubmittedDomainEvent>
{
    public async Task Handle(FeedbackSubmittedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Feedback submitted: {FeedbackId}, EventId: {EventId}, Rating: {Rating}, Notes: {Notes}",
            notification.feedbackId, notification.eventId, notification.rating, notification.notes);

        await _feedbackService.UpdateSummaryForEventAsync(notification.eventId);
    }
}
