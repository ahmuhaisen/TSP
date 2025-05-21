using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;
using TSP.Domain.Events;

namespace TPS.Application.Areas.StudentArea.Societies.EventHandlers
{
    internal sealed class SocietyJoinRequestStatusUpdateDomainEventHandler(
        ILogger<SocietyJoinRequestStatusUpdateDomainEventHandler> _logger,
        INotificationService _notificationService,
        IEmailService _emailService,
        IMediator _mediator
        )
        : INotificationHandler<SocietyJoinRequestStatusUpdateDomainEvent>
    {
        public async Task Handle(SocietyJoinRequestStatusUpdateDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("SocietyJoinRequestStatusUpdateDomainEvent executing handler....");

            var notificationSubject = $"Welcome to your new society";
            var notificationBody = $"{notification.SocietyName}";

            await _notificationService.SendNotificationToUser(
                notification.StudentId,
                UserType.Student,
                notificationSubject,
                notificationBody);

            var student=(await _mediator.Send(new GetCurrentUserInfo.Query(notification.StudentId,UserType.Student))).Data;

            await _emailService.SendSocietyJoinRequestDecisionMade(
                student!.Email,
                student.FullName,
                UserType.Student,
                notification.SocietyName,
                notification.decision
                );
        }
    }
}
