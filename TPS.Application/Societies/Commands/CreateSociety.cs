using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Societies.Commands;

public class CreateSociety
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string LogoID { get; set; } = null!;
        public DateOnly CreationDate { get; set; }
        public string? ThemeColor { get; set; }

        public static Command Create(string name, string description, string logoId, DateOnly creationDate, string? themeColor)
        {
            return new Command
            {
                Name = name.Trim(),
                Description = description.Trim(),
                LogoID = logoId,
                CreationDate = creationDate,
                ThemeColor = themeColor
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
            if (await _context.Societies.AnyAsync(s => s.Name.Equals(request.Name)))
                return Result.Failure<Guid>(Error.ValueAlreadyExist(nameof(Society.Name), request.Name));

            var society = new Society
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                LogoId = request.LogoID,
                CreationDate = request.CreationDate,
                ThemeColor = request.ThemeColor
            };

            await _context.Societies.AddAsync(society);

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult <= 0)
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society.Name), society.Name));

            return Result.Success(society.Id);
        }
    }
}
