
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
        public int numberOfSocities { get; set; }
        private Query(int num)
        {
            numberOfSocities = num;
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

            var data = await _context.EventsApproval
                .AsNoTracking()
                .Where(s => s.DeanAssistantApproval && s.AdvisorApproval)
                .Include(s => s.Event)
                .GroupBy(e => new { e.Event.StartTime.Year, e.Event.StartTime.Month })
                .Select(g => new
                {
                    Date = new DateOnly(g.Key.Year, g.Key.Month, 1),
                    Events = g.Count()
                })
                .ToListAsync();

            var result = data
                .OrderByDescending(s => s.Date)
                .Take(request.numberOfSocities)
                .Select(s => new EventsPerMonthDTO
                {
                    Date = s.Date.ToString("yyyy/MMMM"),
                    Events = s.Events
                })
                .ToList();

            return Result.Success(result);
        }


    }
}
