using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Events;
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
            var memberRecord =  await context.SocietiesMembers
                .Include(x=>x.Student)
                .FirstOrDefaultAsync(s=>s.SocietyId==request.SocietyId&&s.StudentId==request.MemberId);
            if (memberRecord == null)
            {
                return Result.Failure(Error.NotFound("The member does not exist"));
            }

            context.SocietiesMembers.Remove(memberRecord);
            var result = await context.SaveChangesAsync();

            var society = await context.Societies.FirstOrDefaultAsync(x => x.Id == memberRecord.SocietyId);

            society!.RaiseDomainEvent(new MemberLeftSocietyDomainEvent(
                Guid.NewGuid(),
                society.Id,
                society.Name,
                memberRecord.Student.FirstName + " " + memberRecord.Student.LastName
                ));

            if (result <= 0)
            {
                return Result.Failure(Error.CustomError("Something wrong in kicking proccess"));
            }

            return Result.Success(); 

        }
    }

}
