using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Events.Queries
{
    public class EventRequest
    {
        public sealed class Query : IQuery<Result<List<EventDTO>>>
        {
            public Guid UserId { get; set; }

            public Query(Guid userId)
            {
                UserId = userId;
            }

            public static Query Create(Guid userId) => new Query(userId);
        }


        public sealed class Handler : IQueryHandler<Query, Result<List<EventDTO>>>
        {
            private readonly ILogger<Handler> logger;

            private ApplicationDbContext _context { get; }


            public Handler(ApplicationDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<Result<List<EventDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var facultyMember = await _context.FacultyMembers
                    .Include(f => f.Rank) 
                    .Include(f => f.SocietiesAdvised)  
                    .FirstOrDefaultAsync(y => y.Id == request.UserId);

                if (facultyMember == null)
                {
                    return Result<List<EventDTO>>.Failure<List<EventDTO>>(Error.NotFound("Faculty Member", request.UserId.ToString()));
                }

                if (facultyMember.Rank != null &&
                   (facultyMember.Rank.Title == "Dean Assistant" || facultyMember.Rank.Title == "Dean"))
                {
                    Console.WriteLine("second if");

                    var data = await _context.EventsApproval
                        .Where(x => x.AdvisorApproval == true)
                        .OrderByDescending(x => x.Event.StartTime)
                        .Select(x => new EventDTO
                        {
                            Id = x.Id,
                            EventName = x.Event.Name,
                            DateTime = x.Event.StartTime,
                            LocationString = x.Event.LocationString,
                            Description = x.Event.Description,
                            ApprovalStatus = x.DeanAssistantApproval != null
                                ? (x.DeanAssistantApproval == true ? "Accepted" : "Rejected")
                                : "Pending",
                            SocietyName = x.Event.Society != null ? x.Event.Society.Name : "Unknown"
                        })
                        .ToListAsync();
                    return Result.Success(data);
                }

                if (facultyMember.SocietiesAdvised == null || !facultyMember.SocietiesAdvised.Any())
                {
                    return Result.Success(new List<EventDTO>());
                }

                var result = await _context.EventsApproval
                    .Where(x => x.Event != null && x.Event.Society != null && x.Event.Society.AdvisorId == request.UserId)
                    .OrderByDescending(x => x.Event.StartTime)
                    .Select(x => new EventDTO
                    {
                        Id = x.Id,
                        EventName = x.Event.Name,
                        DateTime = x.Event.StartTime,
                        LocationString = x.Event.LocationString,
                        Description = x.Event.Description,
                        ApprovalStatus = x.AdvisorApproval == false ? "Rejected" : "Pending",
                        SocietyName = x.Event.Society.Name
                    })
                    .ToListAsync();

                return Result.Success(result);
            }

        }
    }
}
