using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;
using TSP.Domain.Shared.Options;

namespace TPS.Application.Areas.AdminArea.Societies.Commands;

public sealed class CreateSociety
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public string Name { get; private init; } = null!;
        public string Description { get; private init; } = null!;
        public string Logo { get; private init; } = null!;
        public DateOnly CreationDate { get; private init; }
        public string? ThemeColor { get; private init; }
        public Guid AdvisorId { get; private init; }
        public static Command Create(string name, string description, string logo, DateOnly creationDate, string? themeColor, Guid AdvisorId)
        {
            return new Command
            {
                Name = name.Trim(),
                Description = description.Trim(),
                Logo = logo,
                CreationDate = creationDate,
                ThemeColor = themeColor,
                AdvisorId = AdvisorId
            };
        }
    }

    public sealed class Handler(ApplicationDbContext context, 
                                IGitHubService _FileManager, 
                                IOptions<GitOptions> _options) : ICommandHandler<Command, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await context.Societies.AnyAsync(s => s.Name.Equals(request.Name), cancellationToken: cancellationToken))
                return Result.Failure<Guid>(Error.ValueAlreadyExist(nameof(Society.Name), request.Name));


            var result = await _FileManager.uploadFile(nameof(Society), request.Logo);

            if (result.IsFailure)
            {
                return Result.Failure<Guid>(Error.ValueInvalid(result.Error.Message));
            }
            string LogoId = ResponseEnvelope.Success(result.Data!).ResponseData.ToString() ?? "";
            if (string.IsNullOrEmpty(LogoId))
            {
                return Result.Failure<Guid>(Error.ValueInvalid("Null image id"));
            }


            LogoId = $"https://raw.githubusercontent.com/{_options.Value.UserName}/{_options.Value.Repo}/refs/heads/main/Society/{LogoId}";
            var society = new Society
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                LogoId = LogoId,
                CreationDate = request.CreationDate,
                ThemeColor = request.ThemeColor,
                AdvisorId = request.AdvisorId
            };

            await context.Societies.AddAsync(society, cancellationToken);

            var saveResult = await context.SaveChangesAsync(cancellationToken);
            if (saveResult <= 0)
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society.Name), society.Name));

            return Result.Success(society.Id);
        }
    }
}
