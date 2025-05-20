using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;
using TSP.Domain.Events;

namespace TPS.Application.Areas.StudentArea.Events.EventHandlers
{
    internal sealed class NewEventRequestSubmittedDomainEventHandler(
        ILogger<NewEventRequestSubmittedDomainEventHandler>_logger,
        INotificationService _notificationService,
        IEmailService _emailService,
        IMediator _mediator
        )
        :INotificationHandler<NewEventRequestSubmittedDomainEvent>
    {
        public async Task Handle(NewEventRequestSubmittedDomainEvent notification,CancellationToken cancellationToken)
        {
            _logger.LogWarning("NewEventRequestSubmittedDomainEvent executing handler....");

            var notificationSubject = $"A new event request has been submitted";
            var notificationBody=$"{notification.SocietyName}";

            await _notificationService.SendNotificationToUser(
                notification.UserId,
                notification.UserType,
                notificationSubject,
                notificationBody);

            var user = (await _mediator.Send(new GetCurrentUserInfo.Query(notification.UserId, notification.UserType))).Data;
            try
            {
                await _emailService.SendNewEventRequestSubmittedAlert(
                    user!.Email,
                    user.FullName,
                    user.userType,
                    notification.SocietyName,
                    notification.EventName
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {user!.Email}");
            }
        }
    }
}
