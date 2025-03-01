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
    public class GetAllEvents
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
                // -- case 1 --
                // Current = heba / 
                // Validation heba has no advisor role
                // return empty list

                // -- case 2 --
                // Current = Mousa alakhras / cs
                // Only return the events that Mousa is an advisor of

                // TODO: Validate the user id if he / she is an advisor of a society or a dean / dean assistant
                // TODO: Edit the query to return only the events that the user is an advisor of
                var data = await _context.Events
                    .OrderByDescending(x => x.StartTime)
                    .Select(x => new EventDTO
                    {
                        Id = x.Id,
                        EventName = x.Name,
                        DateTime = x.StartTime,
                        LocationString = x.LocationString,
                        Description = x.Description,
                        ApprovalStatus = _context.EventsApproval.Any(y => y.EventId == x.Id && !(y.AdvisorApproval == true && y.DeanAssistantApproval == null))
                            ? (_context.EventsApproval.Any(y => y.AdvisorApproval == true && y.DeanAssistantApproval == true)
                            ? "Accepted" : "Rejected")
                        : "Pending",
                        SocietyName = x.Society.Name
                    })
                    .ToListAsync();

                return Result.Success(data);
            }
    }
}
}
