using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Events.Queries
{
    public class EventDetails
    {
        public sealed class Query : IQuery<Result<EventDetailsDTO>>
        {
            public Guid EventRequestId { get; set; }
            public Query(Guid eventRequestId)
            {
                EventRequestId = eventRequestId;
            }
            public static Query Create(Guid EventRequestId) => new Query(EventRequestId);
        }
        public sealed class Handler : IQueryHandler<Query, Result<EventDetailsDTO>>
        {
            private ApplicationDbContext _context { get; }
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Result<EventDetailsDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var eventRequest = await _context.EventsApproval
                    .Include(x => x.Event)
                        .ThenInclude(x => x.Society)
                            .ThenInclude(x => x.Advisor)
                    .Include(x => x.Event)
                        .ThenInclude(x => x.Student)
                            .ThenInclude(x => x.Department)
                    .FirstOrDefaultAsync(x => x.Id == request.EventRequestId);

                if (eventRequest == null)
                    return Result.Failure<EventDetailsDTO>(Error.NotFound(nameof(Event), request.EventRequestId.ToString()));

                var eventManager = _context.SocietiesMembers
                    .Where(x => x.StudentId == eventRequest.Event.StudentId);

                var data = new EventDetailsDTO
                {
                    Type = eventRequest.Event.Type,
                    EndDateTime = eventRequest.Event.EndTime,
                    IsAdvisorApproved = eventRequest.AdvisorApproval,
                    IsDeanAssistantApproved = eventRequest.DeanAssistantApproval,

                    Id = eventRequest.Id,
                    EventName = eventRequest.Event.Name,
                    StartDateTime = eventRequest.Event.StartTime,
                    LocationString = eventRequest.Event.LocationString,
                    EventDescription = eventRequest.Event.Description,
                    ApprovalStatus = !(eventRequest.AdvisorApproval == true && eventRequest.DeanAssistantApproval == null)
                            ? (eventRequest.AdvisorApproval == true && eventRequest.DeanAssistantApproval == true
                            ? "Accepted" : "Rejected")
                            : "Pending",
                    EventSociety = new EventSocietyBasicDto
                    {
                        SocietyName = eventRequest.Event.Society.Name,
                        SocietyDescription = eventRequest.Event.Society.Description,
                        SocietyLogoId = eventRequest.Event.Society.LogoId,
                    },

                    EventRequestDTO = new EventRequestDTO
                    {
                        RequestTime = eventRequest.Event.RequestTime,
                        StartTime = eventRequest.Event.StartTime,
                        EndTime = eventRequest.Event.EndTime,
                        AdvisorEmail = eventRequest.Event.Society.Advisor.Email!,
                        IsAttendeesFormEnabled = eventRequest.Event.IsAttendeesFormEnabled,
                        Admins = _context.FacultyMembers
                             .Include(y => y.Rank)
                             .Where(y => y.Rank.Title.ToLower() == "Dean".ToLower() || y.Rank.Title.ToLower() == "Dean Assistant".ToLower())
                             .Select(y => new ApprovalAdministrators
                             {
                                 FacultyMemberName = $"{y.FirstName} {y.LastName}",
                                 FacultyMemberEmail = y.Email!,
                                 Rank = y.Rank.Title
                             })
                             .ToList()
                    },

                    Advisor = new AdvisorBasicDto
                    {
                        AdvisorId = eventRequest.Event.Society.AdvisorId,
                        AdvisorName = $"{eventRequest.Event.Society.Advisor.FirstName} {eventRequest.Event.Society.Advisor.LastName}",
                        AdvisorLogoId = eventRequest.Event.Society.LogoId,
                    },

                    EventManager = new MemberDto
                    {
                        StudentId = eventRequest.Event.StudentId,
                        StudentName = $"{eventRequest.Event.Student.FirstName} {eventRequest.Event.Student.LastName}",
                        StudentEmail = eventRequest.Event.Student.Email ?? "Unknown",
                        StudentLogoId = eventRequest.Event.Student.ProfileImageId,
                        StudentDepartment = eventRequest.Event.Student.Department!.Name,
                        JoinYear = eventManager
                            .Include(y => y.Society)
                            .Where(y => y.SocietyId == eventRequest.Event.SocietyId)
                            .Select(y => y.JoinDate.Year)
                            .SingleOrDefault(),
                        StudentRole = eventManager
                            .Where(y => y.SocietyId == eventRequest.Event.SocietyId)
                            .Select(y => y.Position)
                            .SingleOrDefault() ?? "Unknown",
                        JoinedSocietiesNames = eventManager
                            .Where(y => y.StudentId == eventRequest.Event.StudentId)
                            .Select(y => y.Society.Name)
                            .ToList(),
                    }
                };
                return Result.Success(default(EventDetailsDTO)!);
            }
        }
    }
}
