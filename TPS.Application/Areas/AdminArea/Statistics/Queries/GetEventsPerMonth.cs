
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Statistics.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;
namespace TPS.Application.Areas.AdminArea.Statistics.Queries;
public class GetEventsPerMonth
{

    public sealed class Query : IQuery<Result<List<EventsPerMonthDTO>>>
    {
        public int numberOfMonths { get; set; }
        private Query(int num)
        {
            numberOfMonths = num;
        }
        public static Query Create(int num) => new Query(num);
    }

    public sealed class Handler : IQueryHandler<Query, Result<List<EventsPerMonthDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<EventsPerMonthDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {

                var data = await _context.EventsApproval.AsNoTracking()
               .Where(s => s.DeanAssistantApproval==true && s.AdvisorApproval == true)
               .Include(s => s.Event)
               .GroupBy(e => new { e.Event.StartTime.Year, e.Event.StartTime.Month })
               .Select(g => new
               {
                   Date =  ""+g.Key.Year+"-0"+g.Key.Month+"-01",
                   Events = g.Count()
               })
               .OrderByDescending(s=>s.Date)
               .Take(request.numberOfMonths)
               .Select(s => new EventsPerMonthDTO
                {
                    Date = DateOnly.Parse(s.Date).ToString(),
                    Events = s.Events
                })
               .ToListAsync();

            return Result.Success(data);

        }

    }
}
