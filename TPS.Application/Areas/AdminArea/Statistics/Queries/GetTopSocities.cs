
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

    public sealed class Query : IQuery<Result<List<SocietyCountDTO>>>
    {
        public int numberOfSocieties{ get; set; }
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
            var data = await _context.EventsApproval
                .AsNoTracking()
                .Where(s => s.DeanAssistantApproval == true && s.AdvisorApproval == true)
                .GroupBy(s => s.Event.SocietyId)
                .Select(
                    s => new SocietyCountDTO
                    {
                        Name = s.First().Event.Society.Name,
                        count = s.Count()
                    }
                )
                .OrderByDescending(s=>s.count)
                .Take(request.numberOfSocieties)
                .ToListAsync();
            return Result.Success(data);
        }


    }
}
