using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Events;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Societies.EventHandlers;

internal sealed class SocietyCreatedDomainEventHandler(
    ILogger<SocietyCreatedDomainEventHandler> _logger,
    INotificationService _notificationService,
    IUserService _userService,
    IEmailService _emailService
    )
    : INotificationHandler<SocietyCreatedDomainEvent>
{
    public async Task Handle(SocietyCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("SocietyCreatedDomainEvent executing handler...");

        var notificationSubject = $"A new society created";
        var notificationBody = $"{notification.SocietyName}";

        await _notificationService.SendNotificationForAllUsers(notificationSubject, notificationBody);

        var users= (await _userService.GetAllUsers()).Data;
        if (users is null)
        {
            _logger.LogError("Failed to retrieve users for society creation email.");
            return;
        }
        foreach (var user in users) 
        {
            try
            {
                await _emailService.SendNewSocietyCreatedAlert(
                    user.Email!,
                    user.FullName!,
                    user.UserType,
                    notification.SocietyName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {user.Email}");
            }
        }
    }
}
