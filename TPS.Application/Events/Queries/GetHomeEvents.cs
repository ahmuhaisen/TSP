using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Events.Contracts;
using TPS.Application.Societies.Contracts;
using TPS.Application.Societies.Queries;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Events.Queries
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
                    .Where(s => s.IsApproved == true
                            && s.FacultyMemberId == request.AdvisorId
                            && s.Event.EndTime < today)
                    .OrderByDescending(s => s.Event.EndTime)
                    .Select(s => new EventListDTO
                    {
                        Id = s.Id,
                        Name = s.Event.Name,
                        LocationString = s.Event.LocationString,
                        Description = s.Event.Description,
                        StartTime = s.Event.StartTime,
                        EndTime = s.Event.EndTime,
                        RequestTime = s.Event.StartTime,
                        type = s.Event.type
                    })
                    .FirstOrDefaultAsync();
                if (finishedEvent != null)
                {
                    resultEvents.Add(finishedEvent);
                    --no;
                }

                var upcomingEvent = await allEventsQuery
                    .Include(s => s.Event)
                    .Where(s => s.IsApproved == true
                            && s.FacultyMemberId == request.AdvisorId
                            && s.Event.StartTime > today)
                    .OrderBy(s => s.Event.StartTime)
                    .Take(no)
                    .Select(s => new EventListDTO
                    {
                        Id = s.Id,
                        Name = s.Event.Name,
                        LocationString = s.Event.LocationString,
                        Description = s.Event.Description,
                        StartTime = s.Event.StartTime,
                        EndTime = s.Event.EndTime,
                        RequestTime = s.Event.StartTime,
                        type = s.Event.type
                    })
                    .ToListAsync();
                return Result.Success(resultEvents);
            }
        }
    }
}
