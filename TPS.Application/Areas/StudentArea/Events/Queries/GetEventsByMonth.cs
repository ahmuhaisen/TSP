using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.StudentArea.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.StudentArea.Events.Queries;

public class GetEventsByMonth
{
    public sealed class Query : IQuery<Result<List<EventSimpleDTO>>>
    {
        public required string Date { get; set; }

        public static Query Create(string Date)
        {
            return new Query { Date = Date };
        }
        
    }

    public sealed class Handler(ApplicationDbContext context) : IQueryHandler<Query, Result<List<EventSimpleDTO>>>
    {
        public async Task<Result<List<EventSimpleDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            int year =Int32.Parse(request.Date.Split("-")[0]);
            int month =Int32.Parse(request.Date.Split("-")[1]);

            var data = await context.EventsApproval
                .AsNoTracking()
                .Where(s=>
                s.DeanAssistantApproval==true&&
                s.AdvisorApproval==true&&
                s.Event.StartTime.Year == year&&
                s.Event.StartTime.Month == month
                ).Select(s => new EventSimpleDTO
                {
                    Id = s.EventId,
                    IsAttendeesFormEnabled = s.Event.IsAttendeesFormEnabled,
                    Description = s.Event.Description,
                    Name = s.Event.Name,
                    SocietyName = s.Event.Society.Name,
                    StartTime = s.Event.StartTime,
                    EndTime = s.Event.EndTime,
                    Location = s.Event.LocationString
                }).ToListAsync();
                
            return Result.Success(data);
        }
    }
}
