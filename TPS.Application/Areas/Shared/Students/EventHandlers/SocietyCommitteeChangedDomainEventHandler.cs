using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;
using TSP.Domain.Events;

namespace TPS.Application.Areas.Shared.Students.EventHandlers
{
    internal sealed class SocietyCommitteeChangedDomainEventHandler(
        ILogger<SocietyCommitteeChangedDomainEventHandler>_logger,
        INotificationService _notificationService,
        IEmailService _emailService,
        IMediator _mediator,
        ISocietiesService _societiesService,
        IStudentsService _studentsService
        )
    {
        public async Task Handle(SocietyCommitteeChangedDomainEvent notification,CancellationToken cancellation)
        {
            _logger.LogWarning("SocietyCommitteeChangedDomainEvent executing handler...");

            var notifictaionSubject = $"Society committee has been updated";
            var notificationBody = $"{notification.SocietyName}";

            await _notificationService.SendNotificationToSocietyMembers(notification.SocietyId, notifictaionSubject, notificationBody);

            var society = (await _societiesService.getSocietyById(notification.SocietyId)).Data;
            var newCommittee = (await _mediator.Send(new GetCurrentUserInfo.Query(notification.CommitteeId,UserType.Student))).Data;
            var advisor = (await _mediator.Send(new GetCurrentUserInfo.Query(society!.Advisor.Id, UserType.FacultyMember))).Data;
            var societyMembers = (await _studentsService.getSocietyMembers(notification.SocietyId, true)).Data;

            
            //send email to committee
            await _emailService.SendCommitteeChangesAlert(
                newCommittee!.Email,
                newCommittee.FullName,
                UserType.Student,
                notification.SocietyName,
                newCommittee.FullName,
                true
                );

            //send email to advisor
            await _emailService.SendCommitteeChangesAlert(
                advisor!.Email,
                advisor.FullName,
                UserType.FacultyMember,
                notification.SocietyName,
                newCommittee.FullName,
                false
                );

            foreach (var member in societyMembers!)
            {
                try
                {
                    var user = (await _mediator.Send(new GetCurrentUserInfo.Query(member.Id, UserType.Student))).Data;
                    await _emailService.SendCommitteeChangesAlert(
                        user!.Email,
                        user.FullName,
                        UserType.Student,
                        notification.SocietyName,
                        newCommittee.FullName,
                        false
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
