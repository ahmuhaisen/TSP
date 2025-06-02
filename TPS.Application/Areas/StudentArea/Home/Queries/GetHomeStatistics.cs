using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.StudentArea.Home.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Home.Queries
{
    public class GetHomeStatistics
    {
        public sealed class Query : IQuery<Result<StudentHomeStatisticsDTO>>
        {
            public Guid LoggedInStudent { get; }
            public Query(Guid id)
            {
                LoggedInStudent = id;
            }
            public static Query Create(Guid id) => new Query(id);
        }
        public sealed class Handler : IQueryHandler<Query, Result<StudentHomeStatisticsDTO>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<StudentHomeStatisticsDTO>>Handle(Query request, CancellationToken cancellationToken)
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(
                    s => s.Id == request.LoggedInStudent
                    );
                if (student == null)
                {
                    return Result.Failure<StudentHomeStatisticsDTO>(
                        Error.NotFound(nameof(Guid),
                        request.LoggedInStudent.ToString()));
                }

                var totalSocieties = await _context.Societies
                    .AsNoTracking()
                    .Where(x => x.SocietiesMembers
                        .Any(xm => xm.StudentId == request.LoggedInStudent && xm.IsActive == true))
                    .CountAsync(cancellationToken);

                var totalAttendedEvents = await _context.Events
                    .Where(x => x.Attendees
                        .Any(xm => xm.Email == student.Email))
                    .CountAsync(cancellationToken);
                Console.WriteLine(totalAttendedEvents);
                Console.WriteLine(totalSocieties);

                var studentStatistics = new StudentHomeStatisticsDTO
                {
                    NumSocieties=totalSocieties,
                    NumAttendedEvents = totalAttendedEvents
                };
                return Result.Success(studentStatistics);
            }
        }
    }
}
