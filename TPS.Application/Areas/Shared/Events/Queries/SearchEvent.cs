using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.Shared.Events.Contracts;
using TPS.Application.Areas.Shared.Search;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Events.Queries;

public class SearchEvent
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
    public sealed class Handler : IQueryHandler<Query, Result<List<SearchBasicDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<SearchBasicDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {

            var data = await _context.Events
                .AsNoTracking()
                .Where(
                s => s.Name.Contains(request.SearchTerm ?? "")||
                     s.Description.Contains(request.SearchTerm??"")
                )
                .Select(s => new SearchBasicDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    LogoId = ""
                })
                .ToListAsync();

            return Result.Success(data);
        }
    }
}