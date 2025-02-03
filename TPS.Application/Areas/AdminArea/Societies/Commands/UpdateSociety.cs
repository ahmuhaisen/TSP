using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Societies.Commands;

public sealed class UpdateSociety
{
    public sealed class Command : ICommand<Result<Guid>>
    {
        public string Name { get; private init; } = null!;
        public string Description { get; private init; } = null!;
        public string Logo { get; private init; } = null!;
        public string? ThemeColor { get; private init; }
        public Guid Id { get; private init; }
        public static Command Create(string name, string description, string logo, string? themeColor, Guid Id)
        {
            return new Command
            {
                Name = name.Trim(),
                Description = description.Trim(),
                Logo = logo,
                ThemeColor = themeColor,
                Id = Id
            };
        }
    }

    public sealed class Handler(ApplicationDbContext context, IFileManagerService _FileManager) : ICommandHandler<Command, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {

            var data = context.Societies.FirstOrDefault(s => s.Id == request.Id);
            if (data is null)
                return Result.Failure<Guid>(Error.ValueAlreadyExist(nameof(Society.Name), request.Name));


            var result = await _FileManager.updateFile(nameof(Society), request.Logo);

            if (result.IsFailure)
            {
                return Result.Failure<Guid>(Error.ValueInvalid(result.Error.Message));
            }
            string LogoId = ResponseEnvelope.Success(result.Data!).ResponseData.ToString() ?? "";
            if (string.IsNullOrEmpty(LogoId))
            {
                return Result.Failure<Guid>(Error.ValueInvalid("Null image id"));
            }



            data.Name = request.Name;
            data.Description = request.Description;
            data.ThemeColor = request.ThemeColor;



            var saveResult = await context.SaveChangesAsync(cancellationToken);
            if (saveResult <= 0)
                return Result.Failure<Guid>(Error.ValueInvalid(nameof(Society.Name), data.Name));

            return Result.Success(data.Id);
        }
    }
}

