using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Events;
using TSP.Domain.Shared;
using TSP.Domain.Shared.Options;

namespace TPS.Application.Areas.AdminArea.Societies.Commands;

public sealed class UpdateSociety
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public Guid Id { get; private init; }
        public string Name { get; private init; } = null!;
        public string Description { get; private init; } = null!;
        public string Logo { get; private init; } = null!;
        public string? ThemeColor { get; private init; }
        public DateOnly CreationDate { get; private init; }
        public Guid AdvisorId { get; private init; }
        public static Command Create(
            Guid Id,
            string name,
            string description,
            string logo,
            string? themeColor,
            DateOnly creationDate,
            Guid advisorId)
        {
            return new Command
            {
                Id = Id,
                Name = name.Trim(),
                Description = description.Trim(),
                Logo = logo,
                ThemeColor = themeColor,
                CreationDate = creationDate,
                AdvisorId = advisorId
            };
        }
    }

    public sealed class Handler(ApplicationDbContext context, IGitHubService _FileManager, IOptions<GitOptions> _options) : ICommandHandler<Command, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {

            var data = context.Societies.FirstOrDefault(s => s.Id == request.Id);
            if (data is null)
                return Result.Failure<Guid>(Error.ValueAlreadyExist(nameof(Society.Name), request.Name));

            if (!request.Logo.IsNullOrEmpty())
            {
                var result = await _FileManager.updateFile($"{nameof(Society)}/{data.LogoId}", request.Logo);

                if (result.IsFailure)
                {
                    return Result.Failure<Guid>(Error.ValueInvalid(result.Error.Message));
                }
                string LogoId = ResponseEnvelope.Success(result.Data!).ResponseData.ToString() ?? "";
                LogoId = $"https://raw.githubusercontent.com/{_options.Value.UserName}/{_options.Value.Repo}/refs/heads/main/Society/{LogoId}";
                if (string.IsNullOrEmpty(LogoId))
                {
                    return Result.Failure<Guid>(Error.ValueInvalid("Null image id"));
                }
                data.LogoId = LogoId;
            }
            
            data.Name = request.Name;
            data.Description = request.Description;
            data.ThemeColor = request.ThemeColor;
            if (data.AdvisorId != request.AdvisorId)
            {
                data.RaiseDomainEvent(new SocietyAdvisorChangedDomainEvent(
                    Guid.NewGuid(),
                    data.Id,
                    data.AdvisorId,
                    data.Name
                    ));
                data.AdvisorId=request.AdvisorId;
            }


            var saveResult = await context.SaveChangesAsync(cancellationToken);
            if (saveResult <= 0)
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society.Name), data.Name));

            return Result.Success(data.Id);
        }
    }
}

