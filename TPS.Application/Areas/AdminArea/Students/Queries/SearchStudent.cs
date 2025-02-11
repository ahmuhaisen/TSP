using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Students.Queries;

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
    public sealed class Handler : IQueryHandler<Query, Result<List<StudentBasicDTO>>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<StudentBasicDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
   
                var data = await _context.Students
                .AsNoTracking()
                .Where(s => (s.FirstName + " " + s.LastName).Contains(request.SearchTerm ?? ""))
                .Select(s => new StudentBasicDTO
                {
                    Id = s.Id,
                    FullName = s.FirstName+" "+s.LastName,
                    LogoId = s.ProfileImageId??""
                })
                .ToListAsync();
         
            return Result.Success(data);
        }

    }
}
