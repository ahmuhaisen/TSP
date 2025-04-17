using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Shared.Profiles.Queries
{
    public class HasProfileImage
    {
        public record Query(Guid UserId) : IQuery<Result<bool>>;

        public class Handler : IQueryHandler<Query, Result<bool>>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
                
                if (user == null)
                    return Result.Failure<bool>(Error.NotFound("User", request.UserId.ToString()));

                return Result.Success(!string.IsNullOrEmpty(user.ProfileImageId));
            }
        }
    }
} 