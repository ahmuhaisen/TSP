
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Statistics.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;
namespace TPS.Application.Areas.AdminArea.Statistics.Queries;
public class GetattendanceForEachEvent
{

    public sealed class Query : IQuery<Result<List<EventAttendanceCountDTO>>>
    {
        public int numberOfEvents{ get; set; }
        private Query(int num)
        {
            numberOfEvents = num;
        }
        public static Query Create(int num) => new Query(num);
    }

    public sealed class Handler : IQueryHandler<Query, Result<List<EventAttendanceCountDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<Result<List<EventAttendanceCountDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var data = await _context.Attendees
                .AsNoTracking()
                .Include(s => s.Event)
                .GroupBy(s => s.EventId)
                .Select(
                    s => new EventAttendanceCountDTO
                    {
                        EventName = s.First().Event.Name,
                        count = s.Count()
                    }
                )
                .OrderByDescending(s=>s.count)
                .Take(request.numberOfEvents)
                .ToListAsync();
            return Result.Success(data);
        }


    }
}
