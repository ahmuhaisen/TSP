using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TSP.Domain.Entities;
using TSP.Domain.Events;

namespace TPS.Application.Areas.AdminArea.Societies.EventHandlers;

internal sealed class SocietyCreatedDomainEventHandler(
    ILogger<SocietyCreatedDomainEventHandler> _logger,
    INotificationService _notificationService
    )
    : INotificationHandler<SocietyCreatedDomainEvent>
{
    public async Task Handle(SocietyCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("SocietyCreatedDomainEvent executing handler...");

        var notificationSubject = "A new society created";
        var notificationBody = $"{notification.SocietyId}";

        await _notificationService.SendNotificationForAllStudents(notificationSubject, notificationBody);


    }
}
