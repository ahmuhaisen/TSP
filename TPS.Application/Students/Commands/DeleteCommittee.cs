
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
        public static Command Create(Guid id)
        {
            return new Command
            {
                StudentId = id,
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
                .Include(x => x.Student)
                .Include(x=>x.Society)
                .FirstOrDefaultAsync(s=>s.StudentId == request.StudentId);
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
