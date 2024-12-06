using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Societies.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;
namespace TPS.Application.Societies.Commands;

public class DeleteSociety
{
    public sealed class Command : ICommand<Result>
    {
        public Guid Id { get; set; }
        public static Command Create(Guid id)
        {
            return new Command { Id = id };
        }
    }
    public sealed class Handler : ICommandHandler<Command, Result>
    {
        private ApplicationDbContext _context { get; }
        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }
        async Task<Result> IRequestHandler<Command, Result>.Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _context.Societies.AnyAsync(s => s.ID == request.Id))
            {
                return Result.Failure(Error.NotFound(nameof(Society), request.Id.ToString()));
            }

             _context.Societies.Remove(
                await _context.Societies.FirstAsync(s => s.ID == request.Id)
                );

            var saveResult = _context.SaveChanges();
            if (saveResult <= 0)
            {
                return Result.Failure(Error.InternalServerError("Error: error while deleting entity, (ishi say heek)"));
            }
            return Result.Success();
        }
    }
}