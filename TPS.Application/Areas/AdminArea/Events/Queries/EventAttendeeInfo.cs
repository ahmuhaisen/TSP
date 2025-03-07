using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Events.Queries
{
    public class EventAttendeeInfo
    {
        public sealed class Query : IQuery<Result<List<EventAttendeeDTO>>>
        {
            public Guid EventRequestId { get; }
            public Query(Guid EventRequestId)
            {
                this.EventRequestId = EventRequestId;
            }
            public static Query Create(Guid eventRequestId) => new Query(eventRequestId);
        }
        public sealed class Handler : IQueryHandler<Query, Result<List<EventAttendeeDTO>>>
        {
            private readonly ILogger<Handler> logger;
            private ApplicationDbContext _context { get; }
            public Handler(ApplicationDbContext context,ILogger<Handler>logger)
            {
                _context = context;
                this.logger = logger;
            }
            public async Task<Result<List<EventAttendeeDTO>>>Handle(Query request,CancellationToken cancellationToken)
            {
                var eventRequest = await _context.EventsApproval.FirstOrDefaultAsync(x => x.Id == request.EventRequestId);
                if (eventRequest == null)
                    return Result<List<EventAttendeeDTO>>.Failure<List<EventAttendeeDTO>>(Error.NotFound(request.EventRequestId.ToString()));
                if (eventRequest.Event.IsAttendeesFormEnabled == false)
                    return Result.Success(new List<EventAttendeeDTO>());

                var attendeesList =eventRequest.Event.Attendees
                    .Select(x => new EventAttendeeDTO
                    {
                        Id = x.Id,
                        FullName = x.FullName,
                        Email = x.Email,
                        UniversityNumber=x.UniversityNumber,
                        PhoneNumber=x.PhoneNumber,
                        Notes=x.Notes,
                        RegistrationDateTime=x.SubmittedAt
                    })
                    .ToList();
                return Result.Success(attendeesList);
            }
        }
    }
}
