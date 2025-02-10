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
        public sealed class Query : IQuery<Result<List<EventsDTO>>>
        {
            public Guid UserId { get; set; }

            public Query(Guid userId)
            {
                UserId = userId;
            }

            public static Query Create(Guid userId) => new Query(userId);
        }


        public sealed class Handler : IQueryHandler<Query, Result<List<EventsDTO>>>
        {
            private readonly ILogger<Handler> logger;

            private ApplicationDbContext _context { get; }


            public Handler(ApplicationDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<Result<List<EventsDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var data = await _context.Events
                    .OrderByDescending(x => x.StartTime)
                    .Select(x => new EventsDTO
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
