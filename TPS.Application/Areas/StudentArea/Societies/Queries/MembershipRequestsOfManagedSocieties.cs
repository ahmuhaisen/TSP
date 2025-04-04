using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.StudentArea.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Societies.Queries
{
    public class MembershipRequestsOfManagedSocieties
    {
        public sealed class Query : IQuery<Result<List<MembershipRequestDTO>>>
        {
            public Guid SocietyId { get; set; }
            public Guid LoggedInUser { get; set; }
            public Query(Guid SocietyId, Guid LoggedInUser)
            {
                this.SocietyId = SocietyId;
                this.LoggedInUser = LoggedInUser;
            }
            public static Query Create(Guid SocietyId, Guid LoggedInUser) => new Query(SocietyId,LoggedInUser);
        }
        public sealed class Handler(ApplicationDbContext context) : IQueryHandler<Query, Result<List<MembershipRequestDTO>>>
        {
            public async Task<Result<List<MembershipRequestDTO>>> Handle(Query request, CancellationToken cancellation)
            {
                if (await context.Societies
                    .Include(x=>x.SocietiesMembers)
                    .AnyAsync(s => s.SocietiesMembers
                            .Any(x => x.StudentId == request.LoggedInUser && x.SocietyId == request.SocietyId)))
                    return Result.Failure<List<MembershipRequestDTO>>(Error.AccessDenied("Society"));

                var data = await context.MembershipsRequests
                    .Include(x=>x.Student)
                    .Where(x => x.SocietyId == request.SocietyId)
                    .Select(x => new MembershipRequestDTO
                    {
                        Section = x.Section,
                        ReasonForJoining = x.Motivation,
                        Status = x.Status,
                        StudentBasicDTO = new StudentBasicDTO
                        {
                            Id = x.Id,
                            FullName = $"{x.Student.FirstName} {x.Student.LastName}",
                            LogoId = x.Student.ProfileImageId
                        }
                    })
                    .ToListAsync(cancellationToken: cancellation);
                return Result.Success(data);
            }
        }
    }
}
