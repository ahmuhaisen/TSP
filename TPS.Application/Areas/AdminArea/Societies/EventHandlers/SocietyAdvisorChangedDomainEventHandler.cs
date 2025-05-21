using MediatR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;
using TSP.Domain.Events;

namespace TPS.Application.Areas.AdminArea.Societies.EventHandlers
{
    internal sealed class SocietyAdvisorChangedDomainEventHandler(
        ILogger<SocietyAdvisorChangedDomainEventHandler>_logger,
        INotificationService _notificationService,
        IEmailService _emailService,
        ISocietiesService _societiesService,
        IMediator _mediator,
        IStudentsService _studentsService
        )
        :INotificationHandler<SocietyAdvisorChangedDomainEvent>
    {
        public async Task Handle(SocietyAdvisorChangedDomainEvent notification,CancellationToken cancellation)
        {
            _logger.LogWarning("SocietyAdvisorChangedDomainEvent executing handler...");
            var notificationSubject = $"A new advisor has taken the lead";
            var notificationBody = $"{notification.SocietyName}";


            var society = (await _societiesService.getSocietyById(notification.SocietyId)).Data;
            var newAdvisor = (await _mediator.Send(new GetCurrentUserInfo.Query(society!.Advisor.Id, UserType.FacultyMember))).Data;
            var oldAdvisor = (await _mediator.Send(new GetCurrentUserInfo.Query(notification.OldAdvisorId, UserType.FacultyMember))).Data;
            var societyMembers = (await _studentsService.getSocietyMembers(notification.SocietyId, true)).Data;

            await _notificationService.SendNotificationToUser(
                newAdvisor!.Id,
                UserType.FacultyMember,
                $"You have been assigned to a new society",
                $"{notification.SocietyName}");
            await _notificationService.SendNotificationToSocietyMembers(notification.SocietyId,notificationSubject, notificationBody);

            //email to old advisor
            await _emailService.SendSocietyAdvisorChangedAlert(
                oldAdvisor!.Email,
                oldAdvisor.FullName,
                UserType.FacultyMember,
                notification.SocietyName,
                newAdvisor!.FullName,
                oldAdvisor.FullName,
                false,
                true
                );
            //email to new advisor
            await _emailService.SendSocietyAdvisorChangedAlert(
                newAdvisor.Email,
                newAdvisor.FullName,
                UserType.FacultyMember,
                notification.SocietyName,
                newAdvisor.FullName,
                oldAdvisor.FullName,
                true,
                false
                );
            //email to all society members
            foreach(var member in societyMembers!)
            {
                try
                {
                    var user = (await _mediator.Send(new GetCurrentUserInfo.Query(member.Id, UserType.Student))).Data;
                    await _emailService.SendSocietyAdvisorChangedAlert(
                        user!.Email,
                        user.FullName,
                        UserType.Student,
                        notification.SocietyName,
                        newAdvisor.FullName,
                        oldAdvisor.FullName,
                        false,
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
