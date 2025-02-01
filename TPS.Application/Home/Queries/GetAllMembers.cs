using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Home.Queries
{
    public class GetAllMembers
    {
        public sealed class Query : IQuery<Result<List<MembersListDTO>>>
        {
            public string? SearchTerm { get; set; }
            private Query(string? searchTerm)
            {
                SearchTerm = searchTerm;
            }
            public static Query Create(string? searchTerm) => new Query(searchTerm);
        }
        public sealed class Handler : IQueryHandler<Query, Result<List<MembersListDTO>>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<List<MembersListDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var recentlyJoinedMembers = await _context.SocietiesMembers
                    .Include(x => x.Society)
                    .Include(x => x.Student)
                    .AsNoTracking()
                    .OrderByDescending(x => x.JoinDate)
                    .Select(x => new MembersListDTO
                    {
                        FirstName = x.Student.FirstName,
                        LastName = x.Student.LastName,
                        Position = x.Position,
                        JoinDate = x.JoinDate
                    })
                    .ToListAsync();
                return Result.Success(recentlyJoinedMembers);
            }
        }
    }
}
