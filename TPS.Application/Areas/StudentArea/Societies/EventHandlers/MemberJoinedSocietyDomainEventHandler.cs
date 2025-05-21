using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;
using TSP.Domain.Events;

namespace TPS.Application.Areas.StudentArea.Societies.EventHandlers
{
    internal sealed class MemberJoinedSocietyDomainEventHandler(
        ILogger<MemberJoinedSocietyDomainEventHandler> _logger,
        INotificationService _notificationService,
        IEmailService _emailService,
        ISocietiesService _societiesService,
        IStudentsService _studentsService,
        IMediator _mediator
        )
        : INotificationHandler<MemberJoinedSocietyDomainEvent>
    {
        public async Task Handle(MemberJoinedSocietyDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("MemberJoinedSocietyDomainEvent executing handler....");

            var notificationSubject = $"A new member has joined "+$"{notification.SocietyName}";
            var notificationBody = $"{notification.UserNameJoined}";


            var society = (await _societiesService.getSocietyById(notification.SocietyId)).Data;
            var advisor = (await _mediator.Send(new GetCurrentUserInfo.Query(society!.Advisor.Id, UserType.FacultyMember))).Data;
            var societyMembers = (await _studentsService.getSocietyMembers(notification.SocietyId, true)).Data;

            await _notificationService.SendNotificationToSocietyMembers(notification.SocietyId, notificationSubject, notificationBody);
            await _notificationService.SendNotificationToUser(advisor!.Id, UserType.FacultyMember, notificationSubject, notificationBody);

            await _emailService.SendNewMemberJoinedTheSocietyAlert(
                advisor!.Email,     
                advisor.FullName,
                UserType.FacultyMember,
                notification.SocietyName,
                notification.UserNameJoined);

            foreach (var member in societyMembers!)
            {
                try
                {
                    var user = (await _mediator.Send(new GetCurrentUserInfo.Query(member.Id, UserType.Student))).Data;
                    await _emailService.SendNewMemberJoinedTheSocietyAlert(
                        user!.Email,
                        user.FullName,
                        UserType.Student,
                        notification.SocietyName,
                        notification.UserNameJoined
                        );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send email to {member.FirstName + " " + member.LastName}");
                }
            }
        }
    }
}
