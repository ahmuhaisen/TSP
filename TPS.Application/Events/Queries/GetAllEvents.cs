using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Events.Queries
{
    public class GetAllEvents
    {
        public sealed class Query : IQuery<Result<List<EventListDTO>>>
        {
            public string? SearchTerm { get; set; }
            private Query(string? searchTerm)
            {
                SearchTerm = searchTerm;
            }
            public static Query Create(string? searchTerm) => new Query(searchTerm);
            public sealed class Handler : IQueryHandler<Query, Result<List<EventListDTO>>>
            {
                private ApplicationDbContext _context { get; }
                public Handler(ApplicationDbContext context)
                {
                    _context = context;
                }
                public async Task<Result<List<EventListDTO>>>Handle(Query request, CancellationToken cancellationToken)
                {
                    var today=DateTime.Now;
                    var allEventsQuery =_context.Events.AsQueryable();
                    if (!string.IsNullOrEmpty(request.SearchTerm))
                    {
                        allEventsQuery = allEventsQuery.Where(s => s.Name.Contains(request.SearchTerm));
                    }
                    var data = await allEventsQuery
                        .Where(s=>s.EndTime>=today)
                        .OrderBy(s=>s.StartTime)
                        .Select(s => new EventListDTO
                    {
                        Id = s.Id,
                        LocationString = s.LocationString,
                        Name = s.Name,
                        Description = s.Description,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        RequestTime = s.RequestTime,
                        type = s.type
                    }).ToListAsync();
                    return Result.Success(data);
                }
            }
        }
    }
}
