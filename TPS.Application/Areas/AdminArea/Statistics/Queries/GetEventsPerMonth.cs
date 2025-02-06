
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
        public int numberOfSocities{ get; set; }
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
            List<EventsPerMonthDTO> l = new List<EventsPerMonthDTO>();
            var data = await _context.EventsApproval
                .Where(s => s.DeanAssistantApproval && s.AdvisorApproval)
                .Include(s => s.Event)
                .GroupBy(e => new { e.Event.StartTime.Year, e.Event.StartTime.Month })
                .Select(
                    s => new
                    {
                        Date = new DateTime(s.Key.Year, s.Key.Month, 1),
                        Events = s.Count()
                    }
                )
                .OrderByDescending(s => s.Date)
                .Take(request.numberOfSocities)
                .ToListAsync();

            for (int x = 0; x < data.Count(); x++)
            {
                l.Add(new EventsPerMonthDTO
                {
                    Date = data[x].Date.ToString("yyyy/MMMM"),
                    Events = data[x].Events
                });

            }
            return Result.Success(l);
        }


    }
}
