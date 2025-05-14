using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.StudentArea.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Students.Queries
{
    public class MembershipRequestsOfStudent
    {
        public sealed class Query : IQuery<Result<List<MembershipBasicDTO>>>
        {
            public Guid LoggedInUser { get; }
            public Query(Guid LoggedInUser)
            {
                this.LoggedInUser = LoggedInUser;
            }
            public static Query Create(Guid LoggedInUser) => new Query(LoggedInUser);
        }
        public sealed class Handler(ApplicationDbContext context) : IQueryHandler<Query, Result<List<MembershipBasicDTO>>>
        {
            public async Task<Result<List<MembershipBasicDTO>>>Handle(Query request,CancellationToken cancellationToken)
            {
                var data = await context.MembershipsRequests
                    .Include(x => x.Society)
                    .Where(x => x.StudentId == request.LoggedInUser)
                    .Select(x => new MembershipBasicDTO
                    {
                        Section = x.Section,
                        SubmissionDate = DateOnly.FromDateTime(x.RequestedOn),
                        Status = x.Status,
                        SocietyName = x.Society.Name
                    })
                    .ToListAsync();
                return Result.Success(data);
            }
        }
    }
}
