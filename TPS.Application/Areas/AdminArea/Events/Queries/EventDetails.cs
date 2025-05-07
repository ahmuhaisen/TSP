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
                    .FirstOrDefaultAsync(x => x.EventId == request.EventRequestId || x.Id == request.EventRequestId);

                if (eventRequest == null)
                    return Result.Failure<EventDetailsDTO>(Error.NotFound(nameof(EventRequest), request.EventRequestId.ToString()));

                var tempEvent = await _context.Events
                    .Include(x => x.Society)
                        .ThenInclude(x => x.Advisor)
                    .Include(x => x.Student)
                        .ThenInclude(x => x.Department)
                    .FirstOrDefaultAsync(x => x.Id == eventRequest!.EventId);

                var eventManager = _context.SocietiesMembers
                    .Where(x => x.StudentId == tempEvent!.StudentId);

                var data = new EventDetailsDTO
                {
                    Type = tempEvent?.Type,
                    EndDateTime = tempEvent!.EndTime,
                    IsAdvisorApproved = eventRequest.AdvisorApproval,
                    IsDeanAssistantApproved = eventRequest.DeanAssistantApproval,

                    Id = eventRequest.Id,
                    EventName = tempEvent.Name,
                    StartDateTime = tempEvent.StartTime,
                    LocationString = tempEvent.LocationString,
                    EventDescription = tempEvent.Description,
                    ApprovalStatus = !(eventRequest.AdvisorApproval == true && eventRequest.DeanAssistantApproval == null)
                            ? (eventRequest.AdvisorApproval == true && eventRequest.DeanAssistantApproval == true
                            ? "Accepted" : "Rejected")
                            : "Pending",
                    EventSociety = new EventSocietyBasicDto
                    {
                        SocietyName = tempEvent.Society.Name,
                        SocietyDescription = tempEvent.Society.Description,
                        SocietyLogoId = tempEvent.Society.LogoId,
                    },

                    EventRequestDTO = new EventRequestDTO
                    {
                        RequestTime = tempEvent.RequestTime,
                        StartTime = tempEvent.StartTime,
                        EndTime = tempEvent.EndTime,
                        AdvisorEmail = tempEvent.Society.Advisor.Email!,
                        IsAttendeesFormEnabled = tempEvent.IsAttendeesFormEnabled,
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
                        AdvisorId = tempEvent.Society.AdvisorId,
                        AdvisorName = $"{tempEvent.Society.Advisor.FirstName} {tempEvent.Society.Advisor.LastName}",
                        AdvisorLogoId = tempEvent.Society.LogoId,
                    },

                    EventManager = new MemberDto
                    {
                        StudentId = tempEvent.StudentId,
                        StudentName = $"{tempEvent.Student.FirstName} {tempEvent.Student.LastName}",
                        StudentEmail = tempEvent.Student.Email ?? "Unknown",
                        StudentLogoId = tempEvent.Student.ProfileImageId,
                        StudentDepartment = tempEvent.Student.Department!.Name,
                        JoinYear = eventManager
                            .Include(y => y.Society)
                            .Where(y => y.SocietyId == tempEvent.SocietyId)
                            .Select(y => y.JoinDate.Year)
                            .SingleOrDefault(),
                        StudentRole = eventManager
                            .Where(y => y.SocietyId == tempEvent.SocietyId)
                            .Select(y => y.Position)
                            .SingleOrDefault() ?? "Unknown",
                        JoinedSocietiesNames = eventManager
                            .Where(y => y.StudentId == tempEvent.StudentId)
                            .Select(y => y.Society.Name)
                            .ToList(),
                    }
                };
                return Result.Success(data);
            }
        }
    }
}
