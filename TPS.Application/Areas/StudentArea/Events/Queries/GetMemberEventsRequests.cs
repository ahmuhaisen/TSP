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

public class GetMemberEventsRequests
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
            var data = await context.Events
                .AsNoTracking()
                .Where(
                    s => s.StudentId == request.UserId
                )
               .Select(s => new MemberEventDetailsDTO
               {
                   CommitteeId = request.UserId,
                   SocietyId = s.SocietyId,
                   EventId = s.Id,
                   Title = s.Name,
                   Description = s.Description,
                   Location = s.LocationString,
                   Type = s.Type,
                   StartDate = s.StartTime,
                   EndDate = s.EndTime,
                   AdvisorApproval = null,
                   DeanAssistantApproval = null
               })
               .ToListAsync();

            foreach (var e in data)
            {
                var tempData = await context.EventsApproval
                    .FirstOrDefaultAsync(s => s.Event.Id == e.EventId);

                if (tempData != null)
                {
                    e.AdvisorApproval = tempData.AdvisorApproval;
                    e.DeanAssistantApproval = tempData.DeanAssistantApproval;
                }
            }


            return Result.Success(data);
        }
    }
}
