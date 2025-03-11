using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Events.Queries
{
    public class EventRequest
    {
        public sealed class Query : IQuery<Result<List<EventDTO>>>
        {
            public Guid UserId { get; set; }

            public Query(Guid userId)
            {
                UserId = userId;
            }

            public static Query Create(Guid userId) => new Query(userId);
        }


        public sealed class Handler : IQueryHandler<Query, Result<List<EventDTO>>>
        {
            private readonly ILogger<Handler> logger;

            private ApplicationDbContext _context { get; }


            public Handler(ApplicationDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<Result<List<EventDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var facultyMember = await _context.FacultyMembers.FirstOrDefaultAsync(y => y.Id == request.UserId);
                if (facultyMember == null)
                {
                    return Result<List<EventDTO>>.Failure<List<EventDTO>>(Error.NotFound("Faculty Member", request.UserId.ToString()));
                }
                // -- case 1 --
                // The user is a dean or dean assistant
                // Return all not decided requests
                if (facultyMember.Rank.Title == "Dean Assistant" || facultyMember.Rank.Title == "Dean")
                {
                    var data = await _context.EventsApproval
                        .Where(x => x.AdvisorApproval == true)
                        .OrderByDescending(x => x.Event.StartTime)
                        .Select(x => new EventDTO
                        {
                            Id = x.Id,
                            EventName = x.Event.Name,
                            DateTime = x.Event.StartTime,
                            LocationString = x.Event.LocationString,
                            Description = x.Event.Description,
                            ApprovalStatus = !(x.DeanAssistantApproval == null)
                            ? (x.DeanAssistantApproval == true
                            ? "Accepted" : "Rejected")
                                : "Pending",
                            SocietyName = x.Event.Society.Name
                        })
                        .ToListAsync();
                    return Result.Success(data);
                }
                // -- case 2 --
                // The user has no advisor role
                // Return empty list
                if (!facultyMember.SocietiesAdvised.Any())
                {
                    return Result.Success(new List<EventDTO>());
                }
                // -- case 3 --
                // The user has and advisor role
                // Return the requests for societies he advises
                var result = await _context.EventsApproval
                    .Where(x => x.Event.Society.AdvisorId==request.UserId)
                    .OrderByDescending(x => x.Event.StartTime)
                    .Select(x => new EventDTO
                    {
                        Id = x.Id,
                        EventName = x.Event.Name,
                        DateTime = x.Event.StartTime,
                        LocationString = x.Event.LocationString,
                        Description = x.Event.Description,
                        ApprovalStatus =x.AdvisorApproval == false ?
                            "Rejected" : "Pending",
                        SocietyName = x.Event.Society.Name
                    })
                    .ToListAsync();
                return Result.Success(result);
                //
                // : Validate the user id if he / she is an advisor of a society or a dean / dean assistant DONE
                // TODO: Edit the query to return only the events that the user is an advisor of DONE
            }
        }
    }
}
