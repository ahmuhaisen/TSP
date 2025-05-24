using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.Shared.Search;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Students;

public class SearchStudents
{
    public sealed class Query : IQuery<Result<List<SearchBasicDTO>>>
    {
        public string? SearchTerm { get; set; }

        private Query(string? searchTerm)
        {
            SearchTerm = searchTerm;
        }
        public static Query Create(string? searchTerm) => new Query(searchTerm);
    }

    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<List<SearchBasicDTO>>>
    {
        public async Task<Result<List<SearchBasicDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var data = await _context.Students
                .AsNoTracking()
                .Where(s => (s.FirstName+" "+s.LastName+" "+s.Email).Contains(request.SearchTerm??""))
                .Select(s => new SearchBasicDTO
                {
                    Id = s.Id,
                    Name = $"{s.FirstName} {s.LastName}",
                    Description = "",
                    LogoId = s.ProfileImageId,
                })
                .ToListAsync();

            return Result.Success(data);
        }
    }
}
