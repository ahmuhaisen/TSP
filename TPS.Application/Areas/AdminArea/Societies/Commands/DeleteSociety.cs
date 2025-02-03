using MediatR;
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Societies.Commands;

public sealed class DeleteSociety
{
    public sealed class Command : ICommand<Result>
    {
        public Guid Id { get; private init; }

        public static Command Create(Guid id)
        {
            return new Command { Id = id };
        }
    }

    public sealed class Handler : ICommandHandler<Command, Result>
    {
        private ApplicationDbContext _context { get; }
        private readonly IGitHubService _fileManagerService;
        public Handler(ApplicationDbContext context, IGitHubService fileManagerService)
        {
            _context = context;
            _fileManagerService = fileManagerService;
        }

        async Task<Result> IRequestHandler<Command, Result>.Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _context.Societies.AnyAsync(s => s.Id == request.Id))
            {
                return Result.Failure(Error.NotFound(nameof(Society), request.Id.ToString()));
            }
            var result = await _context.Societies.FirstAsync(s => s.Id == request.Id);
            var deleteImageResult = await _fileManagerService.deleteFile(nameof(Society) + "/" + result.LogoId);
            if (deleteImageResult.IsFailure)
            {
                return Result.Failure(Error.InternalServerError($"{deleteImageResult.Error.Message}"));
            }

            _context.Societies.Remove(result);
            var saveResult = _context.SaveChanges();
            if (saveResult <= 0)
            {
                return Result.Failure(Error.InternalServerError("Error: error while deleting entity, (ishi say heek)"));
            }

            return Result.Success();
        }
    }
}