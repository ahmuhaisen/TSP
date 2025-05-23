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
using TSP.Domain.Enums;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Societies.Queries
{
    public class MembershipRequestsOfManagedSocieties
    {
        public sealed class Query : IQuery<Result<List<MembershipRequestDTO>>>
        {
            public Guid SocietyId { get; set; }
            public Guid LoggedInUser { get; set; }
            public UserType UserType { get; set; }
            public Query(Guid SocietyId, Guid LoggedInUser, UserType UserType)
            {
                this.SocietyId = SocietyId;
                this.LoggedInUser = LoggedInUser;
                this.UserType = UserType;
            }
            public static Query Create(Guid SocietyId, Guid LoggedInUser, UserType UserType)
                => new Query(SocietyId, LoggedInUser, UserType);
        }
        public sealed class Handler(ApplicationDbContext context) : IQueryHandler<Query, Result<List<MembershipRequestDTO>>>
        {
            public async Task<Result<List<MembershipRequestDTO>>> Handle(Query request, CancellationToken cancellation)
            {
                var society = await context.Societies
                    .Include(x => x.MembershipRequests)
                        .ThenInclude(x=>x.Student)
                    .FirstOrDefaultAsync(x => x.Id == request.SocietyId);

                if (request.UserType == UserType.Student)
                {
                    if (!(society!.MembershipRequests.Any(x => x.StudentId == request.LoggedInUser)))
                        return Result.Failure<List<MembershipRequestDTO>>(Error.AccessDenied("Society"));
                }
                if (request.UserType == UserType.FacultyMember)
                {
                    if (!(society!.AdvisorId == request.LoggedInUser))
                        return Result.Failure<List<MembershipRequestDTO>>(Error.AccessDenied("Society"));
                }

                var data = society!.MembershipRequests
                    .Select(x => new MembershipRequestDTO
                    {
                        Id = x.Id,
                        Section = x.Section,
                        ReasonForJoining = x.Motivation,
                        Status = x.Status,
                        RequestedOn = x.RequestedOn,
                        SocietyLogo = society!.LogoId,
                        StudentBasicDTO = new StudentBasicDTO
                        {
                            Id = x.Id,
                            FullName = $"{x.Student.FirstName} {x.Student.LastName}",
                            LogoId = x.Student.ProfileImageId
                        }
                    })
                    .ToList();
                return Result.Success(data);
            }
        }
    }
}
