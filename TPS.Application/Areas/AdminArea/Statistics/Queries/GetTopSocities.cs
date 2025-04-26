
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Statistics.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;
namespace TPS.Application.Areas.AdminArea.Statistics.Queries;
public class GetTopSocities
{

    public sealed class Query : IQuery<Result<List<SocietyDataDTO>>>
    {
        public int numberOfSocities { get; set; }
        private Query(int num)
        {
            numberOfSocities = num;
        }
        public static Query Create(int num) => new Query(num);
    }

    public sealed class Handler : IQueryHandler<Query, Result<List<SocietyDataDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context; 
        }

        public async Task<Result<List<SocietyDataDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var data = await _context.Societies
          
           .Select(society => new SocietyDataDTO
           {
               id = society.Id,
               SocietyName = society.Name,
               Members = _context.SocietiesMembers.Count(sm => sm.SocietyId == society.Id),
               Events = _context.Events.Count(e => e.SocietyId == society.Id)
           })
           .OrderByDescending(s=>s.Events)
           .ThenByDescending(s=>s.Members)
           .Take(request.numberOfSocities)
            .ToListAsync();


            return Result.Success(data);
        }


    }
}
