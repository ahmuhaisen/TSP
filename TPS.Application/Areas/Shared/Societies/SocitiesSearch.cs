using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.Shared.Search;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Societies;
public class SearchSocities
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
            var allSocietiesQuery = _context.Societies.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
                allSocietiesQuery = allSocietiesQuery.Where(s => s.Name.Contains(request.SearchTerm) ||
                                                                 s.Description.Contains(request.SearchTerm));
            Console.WriteLine(request.SearchTerm);
            Console.WriteLine(request.SearchTerm);
            Console.WriteLine(request.SearchTerm);
            Console.WriteLine(request.SearchTerm);

            var data = await allSocietiesQuery.Select(s => new SearchBasicDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                LogoId = s.LogoId,
            }).ToListAsync();

            return Result.Success(data);
        }
    }
}
