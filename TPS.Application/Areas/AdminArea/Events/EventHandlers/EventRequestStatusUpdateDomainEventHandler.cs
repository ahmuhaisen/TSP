

using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;
using TSP.Domain.Events;

namespace TPS.Application.Areas.AdminArea.Events.EventsHandlers
{
    internal sealed class EventRequestStatusUpdateDomainEventHandler(
        ILogger<EventRequestStatusUpdateDomainEventHandler> _logger,
        INotificationService _notificationService,
        IEmailService _emailService,
        IMediator _mediator
        )
        : INotificationHandler<EventRequestStatusUpdateDomainEvent>
    {
        public async Task Handle(EventRequestStatusUpdateDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("EventRequestStatusUpdate executing handler....");

            var notificationSubject = $"The decision for your event request has been made!";
            var notificationBody = $"{notification.eventName}";
            await _notificationService.SendNotificationToUser(notification.studentId,UserType.Student, notificationSubject, notificationBody);

            var user = (await _mediator.Send(new GetCurrentUserInfo.Query(notification.studentId, UserType.Student))).Data;
            try
            {
                await _emailService.SendEventRequestDecisionMade(user!.Email,
                                                                 user.FullName,
                                                                 user.userType,
                                                                 notification.societyName,
                                                                 notification.eventName,
                                                                 notification.decision,
                                                                 notification.remark);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {user!.Email}");
            }
        }
    }
}
