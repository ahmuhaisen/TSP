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

namespace TPS.Application.Students.Queries
{
    public class GetAllMembers
    {
        public sealed class Query : IQuery<Result<List<MembersListDTO>>>
        {
            public bool IsCommittee { get; set; }
            private Query(bool isCommittee)
            {
                IsCommittee = isCommittee;
            }
            public static Query Create(bool isCommittee) =>new Query(isCommittee);
        }
        public sealed class Handler : IQueryHandler<Query, Result<List<MembersListDTO>>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<List<MembersListDTO>>>Handle(Query request, CancellationToken cancellationToken)
            {
                var data=await _context.SocietiesMembers
                    .Include(x=>x.Society)
                    .Include(x=>x.Student)
                    .AsNoTracking()
                    .Where(x=>x.IsCommittee==request.IsCommittee)
                    .OrderByDescending(x=>x.JoinDate)
                    .Select(x=> new MembersListDTO
                    {
                        FirstName=x.Student.FirstName,
                        LastName=x.Student.LastName,
                        Position=x.Position,
                        JoinDate=x.JoinDate
                    })
                    .ToListAsync(cancellationToken);
                return Result.Success(data);
            }
        }
    }
}
