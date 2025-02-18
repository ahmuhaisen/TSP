using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Home.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Home.Queries
{
    public class StudentGetHomeEvents
    {
        public sealed class Query : IQuery<Result<List<StudentEventListDTO>>>
        {
            public Guid LoggedInStudent { get; }
            public Query(Guid id)
            {
                LoggedInStudent = id;
            }
            public static Query Create(Guid LoggedInStudent) => new Query(LoggedInStudent);
        }
        public sealed class Handler : IQueryHandler<Query, Result<List<StudentEventListDTO>>>
        {
            private ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<List<StudentEventListDTO>>>Handle(Query request, CancellationToken cancellationToken)
            {
                var today=DateTime.Now;
                var resultEvents = await _context.EventsApproval
                    .Include(x => x.Event)
                    .Where(x => x.AdvisorApproval == true
                            && x.DeanAssistantApproval == true
                            && x.Event.StartTime > today)
                    .OrderBy(x => x.Event.StartTime)
                    .Take(4)
                    .Select(x => new StudentEventListDTO
                    {
                        Id = x.Id,
                        EventName = x.Event.Name,
                        SocietyName = x.Event.Society.Name,
                        LogoId = x.Event.Society.LogoId,
                        LocationString = x.Event.LocationString,
                        StartTime = x.Event.StartTime,
                        isActiveMember = x.Event.Society.SocietiesMembers
                            .Any(xm => xm.SocietyId == request.LoggedInStudent && xm.IsActive == true) ? true : false
                    })
                    .ToListAsync();
                return Result.Success(resultEvents);
            }
        }
    }
}
