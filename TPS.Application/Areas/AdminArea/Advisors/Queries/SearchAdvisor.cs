
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Advisors.Queries;

public class SearchAdvisor
{
    public sealed class Query : IQuery<Result<List<FacultyMemberBasicDTO>>>
    {
        public string? SearchTerm { get; set; }

        private Query(string? searchTerm)
        {
            SearchTerm = searchTerm;
        }
        public static Query Create(string? searchTerm) => new Query(searchTerm);
    }
    public sealed class Handler : IQueryHandler<Query, Result<List<FacultyMemberBasicDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<FacultyMemberBasicDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var data = await _context.FacultyMembers
                .AsNoTracking()
                .Where(s => (s.FirstName + " " + s.LastName).Contains(request.SearchTerm ?? ""))
                .Select(s => new FacultyMemberBasicDTO
                {
                    Id = s.Id,
                    FullName = s.FirstName + " " + s.LastName,
                    LogoId = s.ProfileImageId??"",
                })
                .ToListAsync();

            return Result.Success(data);
        }

    }
}
