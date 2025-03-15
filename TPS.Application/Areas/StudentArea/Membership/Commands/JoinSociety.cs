using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Membership.Commands
{
    public class JoinSociety
    {
        public sealed class Command : ICommand<Result<Guid>>
        {
            public Guid StudentId { get; private init; }
            public string SocietyName { get; private init; } = null!;
            public string Section { get; private init; } = null!;
            public string Motivation { get; private init; } = null!;
            public static Command Create(Guid id, string section, string name, string motivation)
            {
                return new Command
                {
                    StudentId = id,
                    Section = section,
                    SocietyName = name,
                    Motivation = motivation
                };
            }
        }
        public sealed class Handler(ApplicationDbContext context) : ICommandHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var societyId = await context.Societies
                    .Where(s => s.Name == request.SocietyName)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (societyId==Guid.Empty)
                    return Result.Failure<Guid>(Error.NotFound(request.SocietyName));

                if (await context.Societies
                    .AnyAsync(s => s.SocietiesMembers
                        .Any(x => x.StudentId == request.StudentId && x.SocietyId == societyId), cancellationToken: cancellationToken))
                    return Result.Failure<Guid>(Error.ValueAlreadyExist("Student Already A Member", request.StudentId.ToString()));

                var membership = new MembershipRequest
                {
                    Id=Guid.NewGuid(),
                    SocietyId=societyId,
                    StudentId=request.StudentId,
                    Section=request.Section,
                    Motivation=request.Motivation,
                    RequestedOn=DateTime.Now,
                    Status=RequestStatus.Pending
                };

                await context.MembershipsRequests.AddAsync(membership, cancellationToken);
                var saveResult = await context.SaveChangesAsync(cancellationToken);
                if(saveResult<=0)
                    return Result.Failure<Guid>(Error.ValueInvalid("Membership Request"));

                return Result.Success(membership.Id);
            }
        }
    }
}
