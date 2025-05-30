using Microsoft.AspNetCore.Identity;
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

namespace TPS.Application.Areas.Shared.Profiles.Command;

public class UpdatePassword
{
    public sealed class Command : ICommand<Result<bool>>
    {
        public Guid Id { get; set; }
        public required string Password { get; set; }
        public required string Token { get; set; }
        public static Command Create(Guid id,string password,string token)
        {
            return new Command { Id=id ,Password = password ,Token = token};
        }
    }

    public sealed class Handler(ApplicationDbContext context,
        UserManager<ApplicationUser> _userManager) : ICommandHandler<Command, Result<bool>>
    {
        public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(s=>s.Id == request.Id);
            if (user == null)
            {
                return Result.Failure<bool>(Error.NotFound(nameof(Guid),request.Id.ToString()));
            }
            var result = await _userManager.ResetPasswordAsync(user,request.Token,request.Password);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                return Result.Failure<bool>(Error.ValueInvalid(errorMessage));
            }

            return Result.Success(true);
        }
    }
}
