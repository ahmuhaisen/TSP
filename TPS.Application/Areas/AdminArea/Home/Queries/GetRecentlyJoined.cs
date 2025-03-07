using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Home.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Home.Queries
{
    public class GetRecentlyJoined
    {
        public sealed class Query : IQuery<Result<List<RecentlyJoinedDTO>>>
        {
            public Guid LoggedInUser { get; set; }
            private Query(Guid LoggedInUser)
            {
                this.LoggedInUser = LoggedInUser;
            }
            public static Query Create(Guid LoggedInUser) => new Query(LoggedInUser);
        }
        public sealed class Handler : IQueryHandler<Query, Result<List<RecentlyJoinedDTO>>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<List<RecentlyJoinedDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var recentlyJoinedMembers = await _context.SocietiesMembers
                    .Include(x => x.Society)
                    .Include(x => x.Student)
                    .ThenInclude(x=>x.Department)
                    .AsNoTracking()
                    .OrderByDescending(x => x.JoinDate)
                    .Take(4)
                    .Select(x => new RecentlyJoinedDTO
                    {
                        Id = x.Student.Id,
                        ProfileImageId = x.Student.ProfileImageId,
                        FirstName = x.Student.FirstName,
                        LastName = x.Student.LastName,
                        DepartmentName = x.Student.Department!.Name,
                        JoinDate = x.JoinDate,
                        SocietyName = x.Society.Name
                    })
                    .ToListAsync();
                return Result.Success(recentlyJoinedMembers);
            }
        }
    }
}
