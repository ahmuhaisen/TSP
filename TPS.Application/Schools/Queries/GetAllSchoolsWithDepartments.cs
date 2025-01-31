using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Attendees.Contracts;
using TPS.Application.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Schools.Queries;

public class GetAllSchoolsWithDepartments
{
    public record Query(): IQuery<Result<List<SchoolWithDepartmentsBasicDetailsDTO>>>;

    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<List<SchoolWithDepartmentsBasicDetailsDTO>>>
    {
        public async Task<Result<List<SchoolWithDepartmentsBasicDetailsDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var schoolsWithDepartments = await _context.Schools
                .AsNoTracking()
                .Include(s => s.Departments)
                .Select(s => new SchoolWithDepartmentsBasicDetailsDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Departments = s.Departments.Select(d => new DepartmentBasicDetailsDTO
                    {
                        Id = d.Id,
                        Name = d.Name
                    }).ToList()
                })
                .ToListAsync();

            return Result.Success(schoolsWithDepartments);
        }
    }
}
