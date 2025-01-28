
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Students.Commands;

public class DeleteCommittee
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public Guid StudentId { get; set; }
        public Guid SocietyId { get; set; }
        public static Command Create(Guid StudentId, Guid SocietyId)
        {
            return new Command
            {
                StudentId = StudentId,
                SocietyId = SocietyId
            };
        }
    }

    public sealed class Handler : ICommandHandler<Command, Result<Guid>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var data = await _context.SocietiesMembers
                .FirstOrDefaultAsync(
                s=>s.StudentId == request.StudentId&&
                s.SocietyId == request.SocietyId
                );
            if (data is null)
            {
                return Result.Failure<Guid>(Error.GuidInvalid(request.StudentId));
            }
            if(data.IsCommittee == true)
            {
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Student), request.StudentId.ToString()));

            }
            data.IsCommittee = false;
            data.Position = "member";
            var check = _context.SaveChanges();
            if(check>0)
            {
                return Result.Success(data.StudentId);
            }
            else
            {
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Student), request.StudentId.ToString()));
            }
        }
    }
}
