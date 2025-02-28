using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Schools.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Schools.Queries;

public class GetAllRanks
{
    public sealed class Query : IQuery<Result<List<RankBasicDetailsDTO>>> { }

    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<List<RankBasicDetailsDTO>>>
    {
        public async Task<Result<List<RankBasicDetailsDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var data = await _context.Ranks.Select(r => new RankBasicDetailsDTO(r.Id, r.Title)).ToListAsync();
            return Result.Success(data);
        }
    }
}
