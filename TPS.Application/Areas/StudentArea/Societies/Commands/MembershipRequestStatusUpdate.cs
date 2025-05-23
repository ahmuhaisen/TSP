using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Users.Contracts;
using TPS.Application.Areas.StudentArea.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Enums;
using TSP.Domain.Events;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Societies.Commands
{
    public class MembershipRequestStatusUpdate
    {
        public sealed class Command : ICommand<Result>
        {
            public Guid LoggedInUser { get; set; }
            public Guid MembershipRequestId { get; set; }
            public Guid SocietyId { get; set; }
            public bool isAccepted { get; set; }
            public UserType UserType { get; set; }
            public Command(Guid MembershipRequestId, Guid SocietyId, bool isAccepted, Guid LoggedInUser,UserType UserType)
            {
                this.LoggedInUser = LoggedInUser;
                this.MembershipRequestId = MembershipRequestId;
                this.SocietyId = SocietyId;
                this.isAccepted = isAccepted;
                this.UserType = UserType;
            }
            public static Command Create(Guid membershipRequestId, Guid societyId, bool isAccepted, Guid loggedInUser,UserType userType)
                => new Command(membershipRequestId, societyId, isAccepted, loggedInUser,userType);
        }
        public sealed class Handler(ApplicationDbContext context) : ICommandHandler<Command, Result>
        {
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var membershipRequest = await context.MembershipsRequests
                    .Include(x=>x.Student)
                    .FirstOrDefaultAsync(x => x.Id == request.MembershipRequestId);
                //Membership Request not found
                if (membershipRequest == null)
                {
                    return Result.Failure(Error.NotFound("Membership Request", request.MembershipRequestId.ToString()));
                }
                //Society not found
                var society = await context.Societies
                    .Include(x=>x.SocietiesMembers)
                    .FirstOrDefaultAsync(x => x.Id == membershipRequest.SocietyId);
                if (membershipRequest == null)
                {
                    return Result.Failure(Error.NotFound("Society", request.SocietyId.ToString()));
                }

                if (request.UserType==UserType.Student)
                {
                    var student = await context.Students
                        .FirstOrDefaultAsync(x => x.Id == request.LoggedInUser);
                    //User not found
                    if (student == null)
                    {
                        return Result.Failure(Error.NotFound("Student", request.LoggedInUser.ToString()));
                    }
                    //User is not authorized for this action
                    if(!(society!.SocietiesMembers
                        .Any(x=>x.StudentId==request.LoggedInUser&&x.IsCommittee==true)))
                        return Result.Failure<List<MembershipRequestDTO>>(Error.AccessDenied("Society"));
                }
                else if (request.UserType == UserType.FacultyMember)
                {
                    var facultyMember = await context.FacultyMembers
                        .FirstOrDefaultAsync(x => x.Id == request.LoggedInUser);
                    //User not found
                    if (facultyMember == null)
                    {
                        return Result.Failure(Error.NotFound("Faculty Member", request.LoggedInUser.ToString()));
                    }
                    //User is not authorized for this action
                    if (society!.AdvisorId!=facultyMember.Id)
                        return Result.Failure<List<MembershipRequestDTO>>(Error.AccessDenied("Society"));
                }

                if (request.isAccepted == false)
                {
                    membershipRequest.Status = RequestStatus.Rejected;
                    var checkChanges = context.SaveChanges();
                    society!.RaiseDomainEvent(new SocietyJoinRequestStatusUpdateDomainEvent(
                        Guid.NewGuid(),
                        society.Id,
                        membershipRequest.StudentId,
                        society.Name,
                        true
                        ));
                    if (checkChanges <= 0)
                    {
                        return Result.Failure<Guid>(Error.InternalServerError("could not save record"));
                    }
                }
                else if (request.isAccepted == true)
                {
                    membershipRequest.Status = RequestStatus.Accepted;

                    context.MembershipsRequests.Update(membershipRequest);

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

                    society!.RaiseDomainEvent(new MemberJoinedSocietyDomainEvent(
                        Guid.NewGuid(),
                        society.Id,
                        society.Name,
                        membershipRequest.Student.FirstName + " " + membershipRequest.Student.LastName
                        ));
                    society.RaiseDomainEvent(new SocietyJoinRequestStatusUpdateDomainEvent(
                        Guid.NewGuid(),
                        society.Id,
                        membershipRequest.StudentId,
                        society.Name,
                        true
                        ));
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
