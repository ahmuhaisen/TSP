using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Events.Queries
{
    public class GetAllEvents
    {
        public sealed class Query : IQuery<Result<List<EventsDTO>>>
        {
            private Query() { }
            public static Query Create() => new Query();
        }
        public sealed class Handler : IQueryHandler<Query, Result<List<EventsDTO>>>
        {
            private ApplicationDbContext _context { get; }
            public Handler(ApplicationDbContext context)
            {
                _context = context;
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
