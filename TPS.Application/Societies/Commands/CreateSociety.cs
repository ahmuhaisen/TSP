using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Societies.Commands;

public sealed class CreateSociety
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public string Name { get; private init; } = null!;
        public string Description { get; private init; } = null!;
        public string LogoId { get; private init; } = null!;
        public DateOnly CreationDate { get; private init; }
        public string? ThemeColor { get; private init; }

        public static Command Create(string name, string description, string logoId, DateOnly creationDate, string? themeColor)
        {
            return new Command
            {
                Name = name.Trim(),
                Description = description.Trim(),
                LogoId = logoId,
                CreationDate = creationDate,
                ThemeColor = themeColor
            };
        }
    }

    public sealed class Handler(ApplicationDbContext context) : ICommandHandler<Command, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await context.Societies.AnyAsync(s => s.Name.Equals(request.Name), cancellationToken: cancellationToken))
                return Result.Failure<Guid>(Error.ValueAlreadyExist(nameof(Society.Name), request.Name));

            var society = new Society
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                LogoId = request.LogoId,
                CreationDate = request.CreationDate,
                ThemeColor = request.ThemeColor,
                AdvisorId = Guid.Parse("8B431911-5256-411E-A4C7-11649C5F516D")
            };

            await context.Societies.AddAsync(society, cancellationToken);

            var saveResult = await context.SaveChangesAsync(cancellationToken);
            if (saveResult <= 0)
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society.Name), society.Name));

            return Result.Success(society.Id);
        }
    }
}
