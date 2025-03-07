using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Application.Areas.StudentArea.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Events.Queries;

//TODO: Rename: GetMemberEventsRequests
public class GetMemberEvents
{
    public sealed class Query : IQuery<Result<List<MemberEventDetailsDTO>>>
    {
        public Guid UserId { get; set; }

        public Query(Guid userId)
        {
            UserId = userId;
        }

        public static Query Create(Guid userId) => new Query(userId);
    }
    public sealed class Handler(ApplicationDbContext context) : IQueryHandler<Query, Result<List<MemberEventDetailsDTO>>>
    {
        public async Task<Result<List<MemberEventDetailsDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var data = await context.EventsApproval
                .AsNoTracking()
                .Include(s => s.Event)
                .Where(
                    s => s.Event.StudentId == request.UserId
                )
               .Select(s => new MemberEventDetailsDTO
               {
                   CommitteeId = request.UserId,
                   SocietyId = s.Event.SocietyId,
                   EventId = s.EventId,
                   Title = s.Event.Name,
                   Description = s.Event.Description,
                   Location = s.Event.LocationString,
                   Type = s.Event.Type,
                   StartDate = s.Event.StartTime,
                   EndDate = s.Event.EndTime,
                   DeanAssistantApproval = s.DeanAssistantApproval,
                   AdvisorApproval = s.AdvisorApproval,

               }).ToListAsync();


            return Result.Success(data);
        }
    }
}
