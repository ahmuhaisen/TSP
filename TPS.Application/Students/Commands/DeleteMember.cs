using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Students.Commands;

public class DeleteMember
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
                .Include(x => x.Society)
                .FirstOrDefaultAsync(s => s.StudentId == request.StudentId);
            if (data is null)
            {
                return Result.Failure<Guid>(Error.GuidInvalid(request.StudentId));
            }
           
            _context.SocietiesMembers.Remove(data);
            var check = await _context.SaveChangesAsync();
            if (check > 0)
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
