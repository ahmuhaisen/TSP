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

public class EditMember
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public Guid StudentId { get; set; }
        public Guid SocietyId { get; set; }
        public required string Position { get; set; }
        public static Command Create(Guid id, Guid SocietyId,string Position)
        {
            return new Command
            {
                StudentId = id,
                SocietyId = SocietyId,
                Position = Position
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
            if (await _context.Societies.FirstOrDefaultAsync(s=>s.Id==request.SocietyId) is null)
            {
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society), request.StudentId.ToString()));

            }
            var data = await _context.SocietiesMembers
                .Include(x=>x.Society)
                .FirstOrDefaultAsync(s=>s.StudentId == request.StudentId
                &&s.SocietyId==request.SocietyId);

            if (data is null)
            {
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society), request.StudentId.ToString()));
            }

            data.Position = request.Position;
            var check = await _context.SaveChangesAsync();
            if (check <= 0) {
                return Result.Failure<Guid>(Error.InternalServerError(request.StudentId.ToString()));
            }
            return Result.Success(data.StudentId);
        }
    }
}
