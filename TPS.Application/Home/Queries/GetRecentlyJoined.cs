using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Home.Contracts;
using TPS.Application.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Home.Queries
{
    public class GetRecentlyJoined
    {
        public sealed class Query : IQuery<Result<List<RecentlyJoinedDTO>>>
        {
            public string? SearchTerm { get; set; }
            private Query(string? searchTerm)
            {
                SearchTerm = searchTerm;
            }
            public static Query Create(string? searchTerm) => new Query(searchTerm);
        }
        public sealed class Handler : IQueryHandler<Query, Result<List<RecentlyJoinedDTO>>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<List<RecentlyJoinedDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var recentlyJoinedMembers = await _context.SocietiesMembers
                    .Include(x => x.Society)
                    .Include(x=>x.Student)
                    .AsNoTracking()
                    .OrderByDescending(x => x.JoinDate)
                    .Take(4)
                    .Select(x => new RecentlyJoinedDTO
                    {
                        Id=x.Student.Id,
                        ProfileImageId=x.Student.ProfileImageId,
                        FirstName=x.Student.FirstName,
                        LastName=x.Student.LastName,
                        DepartmentName=x.Student.Department.Name,
                        JoinDate=x.JoinDate,
                        SocietyName=x.Society.Name
                    })
                    .ToListAsync();
                return Result.Success(recentlyJoinedMembers);
            }
        }
    }
}
