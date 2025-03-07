using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
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
                // If the current user is an Advisor => If the advisor decision is made, return 400 // The advisor can't change his decision
                // If the current user is a Dean or Dean Assistant => If the advisor decision is not made, return 400
                // If the current user is a Dean or Dean Assistant => If the advisor decision is made => 
                //     advisor decision is accepted => continue with the dean decision
                //     advisor decision is rejected => return 400
                var facultyMember = await _context.FacultyMembers.FirstOrDefaultAsync(x => x.Id == request.UserId);
                //User not found
                if (facultyMember == null)
                {
                    return Result.Failure(Error.NotFound("Faculty Member", request.UserId.ToString()));
                }
                //Event not found
                var eventRequest = await _context.EventsApproval.FirstOrDefaultAsync(x => x.Id == request.EventRequestId);
                if (eventRequest == null)
                {
                    return Result.Failure(Error.NotFound("Event", request.EventRequestId.ToString()));
                }
                //If the current user is dean/dean assistant
                if (facultyMember.Rank.Title == "Dean" || facultyMember.Rank.Title == "Dean Assistant")
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
                    else if(eventRequest.AdvisorApproval==true)
                    {
                        if (request.isAccepted == true)
                        {
                            eventRequest.DeanAssistantApproval = true;
                        }
                        else if (request.isAccepted == false)
                        {
                            eventRequest.DeanAssistantApproval = false;
                            eventRequest.Remarks = request.Remark;
                        }
                        eventRequest.DecisionDate = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
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
                        }
                        else if (request.isAccepted == false)
                        {
                            eventRequest.AdvisorApproval = false;
                            eventRequest.Remarks = request.Remark;
                        }
                        eventRequest.DecisionDate = DateTime.Now;
                        await _context.SaveChangesAsync();
                    }
                }
                //If the current User isnt an advisor nor dean/dean assistant
                else
                {
                    return Result.Failure(Error.AccessDenied(request.EventRequestId.ToString()));
                }
                return Result.Success();
            }
        }
    }
}
