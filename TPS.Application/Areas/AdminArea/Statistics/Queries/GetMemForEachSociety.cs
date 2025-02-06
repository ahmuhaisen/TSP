
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Statistics.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;
namespace TPS.Application.Areas.AdminArea.Statistics.Queries;
public class GetMemForEachSociety
{

    public sealed class Query : IQuery<Result<List<SocietyMembersCountDTO>>>
    {
        public int numberOfSocieties { get; set; }
        private Query(int num)
        {
            numberOfSocieties = num;
        }
        public static Query Create(int num) => new Query(num);
    }

    public sealed class Handler : IQueryHandler<Query, Result<List<SocietyMembersCountDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<Result<List<SocietyMembersCountDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var l = new List<SocietyMembersCountDTO>();
            var data = await _context.EventsApproval
                .AsNoTracking()
                .Where(s => s.DeanAssistantApproval && s.AdvisorApproval)
                .Include(s => s.Event)
                .ThenInclude(s => s.Society)
                .GroupBy(s => s.Event.SocietyId)
                .Select(s => new SocietyMembersCountDTO
                {
                    Name = s.First().Event.Society.Name,
                    count = s.Count()
                })
                .OrderByDescending(s=>s.count)
                .ToListAsync();



            if (data.Count == 0)
            {
                return Result.Success(l);
            }

            var temp = new SocietyMembersCountDTO { Name = "others", count = 0 };
            int sum = 0;
            for (int x = 0; x < data.Count&& x < request.numberOfSocieties; x++)
            {
                l.Add(data[x]);
            }

            for (int x = request.numberOfSocieties; x< data.Count; x++)
            {
                sum += data[x].count;
            }
            temp.count = sum;
            l.Add(temp);

            
            return Result.Success(l);
        }


    }
}
