
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Societies.Contracts;
using TPS.Application.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Advisors.Queries;

public class GetAllFacultyMembers
{
    public sealed class Query : IQuery<Result<List<FacultyMemberBasicDTO>>>
       { 

        private Query()
        {
        }

        public static Query Create() => new Query();
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
            var data = await _context.FacultyMembers.Select(
                x => new FacultyMemberBasicDTO
                {
                    Id = x.Id,
                    FullName = x.FirstName + " " + x.LastName,
                }
                ).ToListAsync();
            return Result.Success(data);
        }
    }
}
