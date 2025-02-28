using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Home.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Home.Queries
{
    public class GetHomeEvents
    {
        public sealed class Query : IQuery<Result<List<EventListDTO>>>
        {
            public Guid AdvisorId { get; }

            private Query(Guid id)
            {
                AdvisorId = id;
            }
            public static Query Create(Guid AdvisorId) => new Query(AdvisorId);
        }
        public sealed class Handler : IQueryHandler<Query, Result<List<EventListDTO>>>
        {
            private ApplicationDbContext _context { get; }
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<List<EventListDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var resultEvents = new List<EventListDTO>();

                var today = DateTime.Now;
                var no = 4;
                var allEventsQuery = _context.EventsApproval;

                var finishedEvent = await allEventsQuery
                    .Include(s => s.Event)
                    .Where(s => s.AdvisorApproval==true
                            && s.DeanAssistantApproval==true
                            && s.FacultyMemberId == request.AdvisorId
                            && s.Event.EndTime < today)
                    .OrderByDescending(s => s.Event.EndTime)
                    .Select(s => new EventListDTO
                    {
                        Id = s.Id,
                        EventName = s.Event.Name,
                        SocietyName = s.Event.Society.Name,
                        LogoId = s.Event.Society.LogoId,
                        LocationString = s.Event.LocationString,
                        StartTime = s.Event.StartTime,
                        isAdvised = true,
                        isFinished = true
                    })
                    .FirstOrDefaultAsync();
                if (finishedEvent != null)
                {
                    resultEvents.Add(finishedEvent);
                    --no;
                }

                var upcomingEvent = await allEventsQuery
                    .Include(s => s.Event)
                    .Where(s => s.AdvisorApproval == true && s.DeanAssistantApproval==true
                            && s.Event.StartTime > today)
                    .OrderBy(s => s.Event.StartTime)
                    .Take(no)
                    .Select(s => new EventListDTO
                    {
                        Id = s.Id,
                        EventName = s.Event.Name,
                        SocietyName = s.Event.Society.Name,
                        LogoId = s.Event.Society.LogoId,
                        LocationString = s.Event.LocationString,
                        StartTime = s.Event.StartTime,
                        isAdvised = s.FacultyMemberId == request.AdvisorId ? true : false,
                        isFinished = false
                    })
                    .ToListAsync();
                return Result.Success(resultEvents);
            }
        }
    }
}
