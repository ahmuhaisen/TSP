using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;


public class KickMember
{
    public sealed class Command : ICommand<Result>
    {
        public Guid SocietyId { get; set; }
        public Guid MemberId { get; set; }

        public Guid CommitteeId { get; set; }
        

        public static Command Create(Guid SocietyId, Guid MemberId, Guid CommitteeId)
        {
            return new Command
            {
                MemberId = MemberId,
                SocietyId = SocietyId,
                CommitteeId = CommitteeId
            };
        }
    }
    public sealed class Handler(ApplicationDbContext context) : ICommandHandler<Command, Result>
    {

        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var checkCommittee = await context.SocietiesMembers.AnyAsync(
                s=>s.IsCommittee==true&&s.SocietyId==request.SocietyId&&s.StudentId==request.CommitteeId
                 );
            if (!checkCommittee)
            {
                return Result.Failure(Error.AccessDenied("You are not a committee member"));
            }
            var memberRecord =  await context.SocietiesMembers.FirstOrDefaultAsync(
                s=>s.SocietyId==request.SocietyId&&s.StudentId==request.MemberId);
            if (memberRecord == null)
            {
                return Result.Failure(Error.NotFound("The member does not exist"));
            }

            context.SocietiesMembers.Remove(memberRecord);
            var result = await context.SaveChangesAsync();
            if (result <= 0)
            {
                return Result.Failure(Error.CustomError("Something wrong in kicking proccess"));
            }

            return Result.Success(); 

        }
    }

}
