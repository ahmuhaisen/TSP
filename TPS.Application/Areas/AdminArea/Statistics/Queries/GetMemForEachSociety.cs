
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

       public async Task<Result<List<SocietyMembersCountDTO>>> Handle(Query request, CancellationToken cancellationToken)
 {
     var l = new List<SocietyMembersCountDTO>();
     var data = await _context.Societies
         .GroupJoin(
            _context.SocietiesMembers,
            Society => Society.Id,
            SocietiesMembers => SocietiesMembers.SocietyId,
            (Society, SocietiesMembers) => new { Society, SocietiesMembers }
         )
         .SelectMany(
             x => x.SocietiesMembers.DefaultIfEmpty(),
             (x, SocietiesMembers) => new { x.Society, SocietiesMembers }
         ).GroupBy(g => new { g.Society.Id, g.Society.Name })
         .Select(s => new SocietyMembersCountDTO
         {
             id = s.Key.Id,
             Name = s.Key.Name,
             count = s.Count(x => x.SocietiesMembers != null)

         }).ToListAsync();

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
