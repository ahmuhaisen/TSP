using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.StudentArea.Membership.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Membership.Commands
{
    public class MembershipRequestStatusUpdate
    {
        public sealed class Command : ICommand<Result>
        {
            public Guid LoggedInUser { get; set; }
            public Guid MembershipRequestId { get; set; }
            public Guid SocietyId { get; set; }
            public bool isAccepted { get; set; }
            public Command(Guid MembershipRequestId, Guid SocietyId, bool isAccepted, Guid LoggedInUser)
            {
                this.LoggedInUser = LoggedInUser;
                this.MembershipRequestId = MembershipRequestId;
                this.SocietyId = SocietyId;
                this.isAccepted = isAccepted;
            }
            public static Command Create(Guid membershipRequestId, Guid societyId, bool isAccepted, Guid loggedInUser)
                => new Command(membershipRequestId, societyId, isAccepted, loggedInUser);
        }
        public sealed class Handler(ApplicationDbContext context) : ICommandHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var student = await context.Students
                    .FirstOrDefaultAsync(x => x.Id == request.LoggedInUser);
                //User not found
                if (student == null)
                {
                    return Result.Failure(Error.NotFound("Student", request.LoggedInUser.ToString()));
                }
                var membershipRequest = await context.MembershipsRequests
                    .FirstOrDefaultAsync(x => x.Id == request.MembershipRequestId);
                //Membership Request not found
                if (membershipRequest == null)
                {
                    return Result.Failure(Error.NotFound("Membership Request", request.MembershipRequestId.ToString()));
                }
                //User is not authorized for this action
                if (await context.Societies
                    .AnyAsync(s => s.SocietiesMembers
                            .Any(x => x.StudentId == request.LoggedInUser && x.SocietyId == request.SocietyId && x.IsCommittee==true)))
                    return Result<List<MembershipRequestDTO>>.Failure<List<MembershipRequestDTO>>(Error.AccessDenied("Society"));

                if (request.isAccepted == false)
                {
                    membershipRequest.Status = RequestStatus.Rejected;
                }
                else if (request.isAccepted == true)
                {
                    membershipRequest.Status = RequestStatus.Accepted;
                    var newMember = new SocietiesMembers
                    {
                        SocietyId = request.SocietyId,
                        StudentId = membershipRequest.StudentId,
                        Position = membershipRequest.Section,
                        JoinDate = DateOnly.FromDateTime(DateTime.Now),
                        IsActive = true,
                        IsCommittee = false
                    };
                    await context.SocietiesMembers.AddAsync(newMember);
                    var checkChanges = context.SaveChanges();
                    if (checkChanges <= 0)
                    {
                        return Result.Failure<Guid>(Error.InternalServerError("could not save record"));
                    }
                }
                return Result.Success();
            }
        }
    }
}
