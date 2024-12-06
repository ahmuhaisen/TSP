using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Societies.Queries;

public class GetAllSocieties
{
    public sealed class Query : IQuery<Result<List<SocietyDTO>>>
    {
        public string? SearchTerm { get; set; }

        private Query(string? searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public static Query Create(string? searchTerm) => new Query(searchTerm);
    }

    public sealed class Handler : IQueryHandler<Query, Result<List<SocietyDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Result<List<SocietyDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var allSocietiesQuery = _context.Societies.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
                allSocietiesQuery = allSocietiesQuery.Where(s => s.Name.Contains(request.SearchTerm));

            var data = await allSocietiesQuery.Select(s => new SocietyDTO
            {
                Name = s.Name,
                Description = s.Description,
                CreationDate = s.CreationDate,
                LogoID = s.LogoID,
                ThemeColor = s.ThemeColor
            }).ToListAsync();

            return Result.Success(data);
        }
    }
}
