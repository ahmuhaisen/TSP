using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Home.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Home.Queries
{
    public class GetHomeStatistics
    {
        public sealed class Query:IQuery<Result<HomeStatisticsDTO>>
        {
            public string? SearchTerm { get;}
            private Query(string? searchTerm)
            {
                SearchTerm = searchTerm;
            }
            public static Query Create(string? searchTerm)=> new Query(searchTerm);
        }
        public sealed class Handler : IQueryHandler<Query, Result<HomeStatisticsDTO>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<HomeStatisticsDTO>>Handle(Query request, CancellationToken cancellationToken)
            {
                var totalMembers = await _context.SocietiesMembers
                    .Include(x => x.Student)
                    .AsNoTracking()
                    .Where(x => x.IsActive == true)
                    .CountAsync();

                var totalSocieties = await _context.Societies
                    .CountAsync();

                var totalCompletedEvents = await _context.EventsApproval
                    .Include(x => x.Event)
                    .Where(x => x.IsApproved == true
                            && x.Event.EndTime < DateTime.Now)
                    .CountAsync();

                var homeStatistics = new HomeStatisticsDTO
                {
                    TotalMembers = totalMembers,
                    TotalSocieties = totalSocieties,
                    TotalCompletedEvents = totalCompletedEvents
                };
                return Result.Success(homeStatistics);
            }
        }
    }
}
