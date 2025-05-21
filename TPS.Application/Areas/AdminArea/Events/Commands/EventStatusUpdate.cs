using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Events.Queries;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Enums;
using TSP.Domain.Events;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Events.Commands
{
    public class EventStatusUpdate
    {
        public sealed class Command : ICommand<Result>
        {
            public Guid UserId { get; set; }
            public Guid EventRequestId { get; set; }
            public bool isAccepted { get; set; }
            public string? Remark { get; set; }
            public Command(Guid userId, Guid eventRequestId, bool isAccepted, string? remark)
            {
                UserId = userId;
                EventRequestId = eventRequestId;
                this.isAccepted = isAccepted;
                Remark = remark;
            }
            public static Command Create(Guid userId, Guid eventRequestId, bool isAccepted, string remark)
                => new Command(userId, eventRequestId, isAccepted, remark);
        }
        public sealed class Handler : ICommandHandler<Command, Result>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var facultyMember = await _context.FacultyMembers
                    .Include(x => x.Rank)
                    .FirstOrDefaultAsync(x => x.Id == request.UserId);
                //User not found
                if (facultyMember == null)
                {
                    return Result.Failure(Error.NotFound("Faculty Member", request.UserId.ToString()));
                }
                //Event not found
                var eventRequest = await _context.EventsApproval
                    .Include(x => x.Event)
                        .ThenInclude(x => x.Society)
                    .FirstOrDefaultAsync(x => x.Event.Id == request.EventRequestId);
                if (eventRequest == null)
                {
                    return Result.Failure(Error.NotFound("Event", 
                        request.EventRequestId.ToString()));
                }
                //If the current user is dean/dean assistant
                if (facultyMember.Rank.Title == "Dean" || facultyMember.Rank.Title == "Dean Assistant" &&
                    eventRequest.AdvisorApproval == true)
                {
                    if (eventRequest.DeanAssistantApproval != null)
                    {
                        return Result.Failure(Error.ValueAlreadyExist("Decision Already Made", request.EventRequestId.ToString()));
                    }
                    else if (eventRequest.AdvisorApproval == false)
                    {
                        return Result.Failure(Error.ValueAlreadyExist("Request Already Rejected", request.EventRequestId.ToString()));
                    }
                    else if (eventRequest.AdvisorApproval == null)
                    {
                        return Result.Failure(Error.AccessDenied(request.EventRequestId.ToString()));
                    }
                    if (request.isAccepted == true)
                    {
                        eventRequest.DeanAssistantApproval = true;
                        RaiseStatusUpdateDomainEvent(eventRequest, true, null);
                        eventRequest.RaiseDomainEvent(new NewEventScheduledDomainEvent(
                            Guid.NewGuid(),
                            eventRequest.Event.SocietyId,
                            eventRequest.Event.Society.Name,
                            eventRequest.Event.Name
                            ));
                    }
                    else if (request.isAccepted == false)
                    {
                        eventRequest.DeanAssistantApproval = false;
                        eventRequest.Remarks = request.Remark;
                        RaiseStatusUpdateDomainEvent(eventRequest, false, request.Remark);
                    }
                    eventRequest.DecisionDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return Result.Success();
                }
                //If the current user is the Advisor
                if (eventRequest.Event.Society.AdvisorId == facultyMember.Id)
                {
                    if (eventRequest.AdvisorApproval != null)
                    {
                        return Result.Failure(Error.ValueAlreadyExist("Decision Already Made", request.EventRequestId.ToString()));
                    }
                    else
                    {
                        if (request.isAccepted == true)
                        {
                            eventRequest.AdvisorApproval = true;
                            var deanAssistant = await _context.FacultyMembers
                                .Include(x=>x.Rank)
                                .FirstOrDefaultAsync(x => x.Rank.Title == "Dean" || x.Rank.Title == "Dean Assistant");

                            if (deanAssistant is null)
                            {
                                throw new InvalidOperationException("No Dean or Dean Assistant found.");
                            }

                            eventRequest.RaiseDomainEvent(new NewEventRequestSubmittedDomainEvent(
                                Guid.NewGuid(),
                                eventRequest.Event.SocietyId,
                                deanAssistant.Id,
                                UserType.FacultyMember,
                                eventRequest.Event.Society.Name,
                                eventRequest.Event.Name
                                ));
                        }
                        else if (request.isAccepted == false)
                        {
                            eventRequest.AdvisorApproval = false;
                            eventRequest.Remarks = request.Remark;
                            RaiseStatusUpdateDomainEvent(eventRequest, false, request.Remark);
                        }
                        eventRequest.DecisionDate = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                    return Result.Success();
                }
                //If the current User isnt an advisor nor dean/dean assistant
                return Result.Failure(Error.AccessDenied(request.EventRequestId.ToString()));
            }
            private void RaiseStatusUpdateDomainEvent(EventApproval eventRequest, bool isAccepted, string? remark)
            {
                var domainEvent = new EventRequestStatusUpdateDomainEvent(
                    Guid.NewGuid(),
                    eventRequest.Event.StudentId,
                    eventRequest.Event.SocietyId,
                    eventRequest.Event.Society.Name,
                    eventRequest.Event.Name,
                    isAccepted,
                    remark
                );

                eventRequest.RaiseDomainEvent(domainEvent);
            }
        }
    }
}
