

using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Students.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Students.Commands;

public class AddCommittee
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public Guid StudentId { get; set; }
        public required string StudentPosition {  get; set; }
        public DateOnly StudentDate { get; set; }
        public static Command Create(Guid id,string position,DateOnly date)
        {
            return new Command
            {
                StudentId = id,
                StudentPosition = position,
                StudentDate = date
            };
        }
    }

    public sealed class Handler: ICommandHandler<Command, Result<Guid>>
    {
        private ApplicationDbContext _context { get; }

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var data = await _context.SocietiesMembers
                .Include(x => x.Student)
                .Include(x => x.Society)
                .FirstOrDefaultAsync(s => s.StudentId == request.StudentId);
            if (data is null)
            {
                return Result.Failure<Guid>(Error.GuidInvalid(request.StudentId));

            }
            if (data.IsCommittee == true)
            {
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Student), request.StudentId.ToString()));
            }
            data.IsCommittee = true;
            data.Position = request.StudentPosition;
            data.JoinDate =request.StudentDate;
            var check = _context.SaveChanges();
            if(check>0)
            {
                return Result.Success(request.StudentId);
            }
            else
            {
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Student), request.StudentId.ToString()))
            }

        }
    }
}
