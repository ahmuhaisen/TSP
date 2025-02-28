using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Events.Commands
{
    public class AcceptEvent
    {
        public sealed class Command : ICommand<Result>
        {
            public Guid UserId { get; set; }
            public Guid EventId { get; set; }
            public Command(Guid userId, Guid eventId)
            {
                UserId = userId;
                EventId = eventId;
            }
            public static Command Create(Guid userId, Guid eventId) => new Command(userId, eventId);
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
                //Check if the user is authorized to do the action
                var LoggedUser = await _context.Events
                    .Where(x => x.Id == request.EventId && x.Society.AdvisorId == request.UserId)
                    .SingleOrDefaultAsync();
                if (LoggedUser != null)
                {
                    return Result.Failure(Error.AccessDenied($"{request.EventId}"));
                }

                var eventStatus = await _context.EventsApproval
                    .Where(x => x.Event.Id == request.EventId)
                    .SingleOrDefaultAsync(cancellationToken);

                var advisorId = LoggedUser!.Society.AdvisorId;
                var deanAssistant = await _context.FacultyMembers
                    .Where(x => x.Rank.Title == "Dean Assistant")
                    .SingleOrDefaultAsync();
                //EventStatus still not decided by advisor nor deanasistant
                if (eventStatus == null && request.UserId == advisorId)
                {
                    var eventApproval = new EventApproval
                    {
                        Id = Guid.NewGuid(),
                        AdvisorApproval = true,
                        DecisionDate = DateTime.Now,
                        EventId = request.EventId,
                        FacultyMemberId = advisorId
                    };
                    _context.EventsApproval.Add(eventApproval);
                    await _context.SaveChangesAsync();
                    return Result.Success();
                }
                else if (eventStatus != null
                    && request.UserId == deanAssistant!.Id
                    && eventStatus.AdvisorApproval == true)//EventStatus when decision is decided by advisor
                {
                    eventStatus.DeanAssistantApproval = true;
                    eventStatus.DecisionDate = DateTime.Now;
                    _context.EventsApproval.Update(eventStatus);
                    await _context.SaveChangesAsync();
                    return Result.Success();
                }
                return Result.Failure(Error.AccessDenied($"{request.EventId}"));
            }
        }
    }
}
