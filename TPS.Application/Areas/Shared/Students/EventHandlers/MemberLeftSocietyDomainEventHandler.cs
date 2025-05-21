using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;
using TSP.Domain.Events;
using TSP.Domain.Primitives;

namespace TPS.Application.Areas.Shared.Students.EventHandlers
{
    internal sealed class MemberLeftSocietyDomainEventHandler(
        ILogger<MemberLeftSocietyDomainEventHandler>_logger,
        INotificationService _notificationService,
        IEmailService _emailService,
        ISocietiesService _societiesService,
        IStudentsService _studentsService,
        IMediator _mediator
        )
        : INotificationHandler<MemberLeftSocietyDomainEvent>
    {
        public async Task Handle(MemberLeftSocietyDomainEvent notification,CancellationToken cancellationToken)
        {
            _logger.LogWarning("MemberLeftSocietyDomainEvent executing handler....");

            var notificationSubject = $"A member has left "+$"{notification.SocietyName}";
            var notificationBody=$"{notification.UserNameLeft}";

            var society = (await _societiesService.getSocietyById(notification.SocietyId)).Data;
            var advisor = (await _mediator.Send(new GetCurrentUserInfo.Query(society!.Advisor.Id, UserType.FacultyMember))).Data;
            var societyMembers = (await _studentsService.getSocietyMembers(notification.SocietyId, true)).Data;

            await _notificationService.SendNotificationToSocietyMembers(notification.SocietyId,notificationSubject, notificationBody);
            await _notificationService.SendNotificationToUser(advisor!.Id, UserType.FacultyMember, notificationSubject, notificationBody);

            await _emailService.SendMemberLeftTheSocietyAlert(
                advisor!.Email,
                advisor.FullName,
                UserType.FacultyMember,
                notification.SocietyName,
                notification.UserNameLeft);

            foreach(var member in societyMembers!)
            {
                try
                {
                    var user = (await _mediator.Send(new GetCurrentUserInfo.Query(member.Id, UserType.Student))).Data;
                    await _emailService.SendMemberLeftTheSocietyAlert(
                        user!.Email,
                        user.FullName,
                        UserType.Student,
                        notification.SocietyName,
                        notification.UserNameLeft
                        );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send email to {member.FirstName+" "+member.LastName}");
                }
            }
        }
    }
}
