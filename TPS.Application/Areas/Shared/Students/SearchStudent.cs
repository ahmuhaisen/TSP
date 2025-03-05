
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Students;

public class SearchStudent
{
    public sealed class Query : IQuery<Result<List<StudentBasicDTO>>>
    {
        public string? SearchTerm { get; set; }

        private Query(string? searchTerm)
        {
            SearchTerm = searchTerm;
        }
        public static Query Create(string? searchTerm) => new Query(searchTerm);
    }

    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<List<StudentBasicDTO>>>
    {


        public async Task<Result<List<StudentBasicDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var data = await _context.Students
                .AsNoTracking()
                .Where(s=>(s.FirstName+" "+s.LastName).Contains(request.SearchTerm))
                .Select(s=> new StudentBasicDTO
                {
                    Id = s.Id,  
                 FullName = s.FirstName+" "+s.LastName,
                 LogoId = s.ProfileImageId,
                }
                
                )
                .ToListAsync();

            return Result.Success(data);
        }
    }
}
