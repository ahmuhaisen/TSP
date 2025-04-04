
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

    public sealed class Query : IQuery<Result<List<SocietyCountDTO>>>
    {
        public int numberOfSocieties { get; set; }
        private Query(int num)
        {
            numberOfSocieties = num;
        }
        public static Query Create(int num) => new Query(num);
    }

    public sealed class Handler : IQueryHandler<Query, Result<List<SocietyCountDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<Result<List<SocietyCountDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var l = new List<SocietyCountDTO>();
            var data = await _context.SocietiesMembers
                .AsNoTracking()
                .GroupBy(s => s.Society.Id)
                .Select(s => new SocietyCountDTO
                {
                    Name = s.First().Society.Name,
                    count = s.Count()
                })
                .OrderByDescending(s=>s.count)
                .ToListAsync();



            if (data.Count == 0)
            {
                return Result.Success(l);
            }

            var temp = new SocietyCountDTO { Name = "others", count = 0 };
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
