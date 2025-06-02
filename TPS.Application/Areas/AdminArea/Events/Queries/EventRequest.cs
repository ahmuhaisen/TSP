using Bogus;
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
                // TODO: Check when faculty member is dean/dean assistant and advisor at the same time
                if (facultyMember.Rank != null &&
                   (facultyMember.Rank.Title == "Dean Assistant" || facultyMember.Rank.Title == "Dean"))
                {

                    var data = await _context.EventsApproval
                        .Include(x => x.Event)
                            .ThenInclude(x => x.Society)
                        .Where(x => x.AdvisorApproval == true || x.Event.Society.AdvisorId == facultyMember.Id)
                        .OrderByDescending(x => x.Event.StartTime)
                        .Select(x => new EventDTO
                        {
                            Id = x.EventId,
                            EventName = x.Event.Name,
                            StartDateTime = x.Event.StartTime,
                            LocationString = x.Event.LocationString,
                            ApprovalStatus = getEventStatus(x),
                            EventDescription = x.Event.Description,
                            EventSociety = new EventSocietyBasicDto
                            {
                                SocietyName = x.Event.Society != null ? x.Event.Society.Name : "Unknown",
                                SocietyDescription = x.Event.Society != null ? x.Event.Society.Description : "Unknown",
                                SocietyLogoId = x.Event.Society != null ? x.Event.Society.LogoId : "Unkown"
                            }
                        })
                        .ToListAsync();
                    return Result.Success(data);
                }

                if (facultyMember.SocietiesAdvised == null || !facultyMember.SocietiesAdvised.Any())
                {
                    return Result.Success(new List<EventDTO>());
                }

                var result = await _context.EventsApproval
                    .Include(x => x.Event)
                    .ThenInclude(x => x.Society)
                    .Where(x => x.Event.Society.AdvisorId == request.UserId)
                    .OrderByDescending(x => x.Event.StartTime)
                    .Select(x => new EventDTO
                    {
                        Id = x.EventId,
                        EventName = x.Event.Name,
                        StartDateTime = x.Event.StartTime,
                        LocationString = x.Event.LocationString,
                        ApprovalStatus = getEventStatus(x),
                        EventDescription = x.Event.Description,
                        EventSociety = new EventSocietyBasicDto
                        {
                            SocietyName = x.Event.Society != null ? x.Event.Society.Name : "Unknown",
                            SocietyDescription = x.Event.Society != null ? x.Event.Society.Description : "Unknown",
                            SocietyLogoId = x.Event.Society != null ? x.Event.Society.LogoId : "Unkown"
                        },
                    })
                    .ToListAsync();

                return Result.Success(result);
            }

        }
        static private string getEventStatus(EventApproval eventApproval)
        {
            bool? advisorStatus = eventApproval.AdvisorApproval;
            bool? deanAssistantStatus = eventApproval.DeanAssistantApproval;

            if (deanAssistantStatus == null && (advisorStatus == true || advisorStatus == null))
            {
                return "Pending";
            }
            if (advisorStatus == false || deanAssistantStatus == false)
            {
                return "Rejected";
            }
            return "Accepted";
        }

    }
}
