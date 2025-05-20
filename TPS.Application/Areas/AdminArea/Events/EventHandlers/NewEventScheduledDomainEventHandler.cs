using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Events;

namespace TPS.Application.Areas.AdminArea.Events.EventHandlers
{
    internal sealed class NewEventScheduledDomainEventHandler(
        ILogger<NewEventScheduledDomainEventHandler> _logger,
        INotificationService _notificationService,
        IUserService _userService,
        IEmailService _emailService
        )
        : INotificationHandler<NewEventScheduledDomainEvent>
    {
        public async Task Handle(NewEventScheduledDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("NewEventScheduledDomainEvent executing handler....");
            var notificationSubject = $"A new event just scheduled";
            var notificationBody = $"{notification.SocietyName}";
            await _notificationService.SendNotificationForAllUsers(notificationSubject, notificationBody);

            var users = (await _userService.GetAllUsers()).Data;
            if (users is null)
            {
                _logger.LogError("Failed to retrieve users for new event scheduled email.");
                return;
            }
            foreach (var user in users)
            {
                try
                {
                    await _emailService.SendNewEventScheduled(
                        user.Email!,
                        user.FullName!,
                        user.UserType,
                        notification.SocietyName,
                        notification.EventName
                        );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send email to {user.Email}");
                }
            }
        }
    }
}
