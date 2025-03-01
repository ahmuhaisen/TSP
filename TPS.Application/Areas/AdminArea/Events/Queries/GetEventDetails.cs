using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.AdminArea.Events.Queries
{
    public class GetEventDetails
    {
        public sealed class Query : IQuery<Result<EventDetailsDTO>>
        {
            public Guid EventId { get; set; }
            public Query(Guid id)
            {
                EventId = id;
            }
            public static Query Create(Guid EventId) => new Query(EventId);
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
                if (!_context.Events.Any(s => s.Id == request.EventId))
                    return Result.Failure<EventDetailsDTO>(Error.NotFound(nameof(Event), request.EventId.ToString()));

                var accessSocietiesMembers = _context.SocietiesMembers;
                var data = await _context.Events
                    .Where(x => x.Id == request.EventId)
                    .Select(x => new EventDetailsDTO
                    {
                        Type = x.type,
                        EventDate = DateOnly.FromDateTime(x.StartTime),
                        StartTime = TimeOnly.FromDateTime(x.EndTime),
                        SocietyDescription = x.Society.Description,
                        SocietyLogoId = x.Society.LogoId,
                        AdvisorId = x.Society.AdvisorId,
                        AdvisorName = $"{x.Society.Advisor.FirstName} {x.Society.Advisor.LastName}",
                        AdvisorLogoId = x.Society.LogoId,
                        StudentId = x.StudentId,
                        StudentName = $"{x.Student.FirstName} {x.Student.LastName}",
                        StudentEmail = x.Student.Email ?? "Unknown",
                        StudentLogoId = x.Student.ProfileImageId,
                        StudentDepartment = x.Student.Department!.Name,
                        JoinYear = accessSocietiesMembers
                            .Where(y => y.StudentId == x.StudentId && y.SocietyId == x.SocietyId)
                            .Select(y => y.JoinDate.Year)
                            .SingleOrDefault(),
                        StudentRole = accessSocietiesMembers
                            .Where(y => y.StudentId == x.StudentId && y.SocietyId == x.SocietyId)
                            .Select(y => y.Position)
                            .SingleOrDefault() ?? "Unknown",
                        JoinedSocietiesNames = accessSocietiesMembers
                            .Where(y => y.StudentId == x.StudentId)
                            .Select(y => y.Society.Name)
                            .ToList(),
                        EventDTO = new EventDTO
                        {
                            Id = x.Id,
                            EventName = x.Name,
                            LocationString = x.LocationString,
                            Description = x.Description,
                            SocietyName = x.Society.Name,

                            ApprovalStatus = _context.EventsApproval.Any(y => y.EventId == x.Id && !(y.AdvisorApproval == true && y.DeanAssistantApproval == null))
                            ? (_context.EventsApproval.Any(y => y.AdvisorApproval == true && y.DeanAssistantApproval == true)
                            ? "Accepted" : "Rejected")
                            : "Pending"
                        },
                        EventRequestDTO = new EventRequestDTO
                        {
                            RequestTime = x.RequestTime,
                            StartTime = x.StartTime,
                            EndTime = x.EndTime,
                            AdvisorEmail = x.Society.Advisor.Email!,
                            IsAttendeesFormEnabled = x.IsAttendeesFormEnabled,
                            Admins = _context.FacultyMembers
                                .Where(y => y.Rank.Title.ToLower() == "Dean".ToLower() || y.Rank.Title.ToLower() == "Dean Assistant".ToLower())
                                .Select(y => new ApprovalAdministrators
                                {
                                    FacultyMemberName = $"{y.FirstName} {y.LastName}",
                                    FacultyMemberEmail = y.Email!,
                                    Rank = y.Rank.Title
                                })
                                .ToList()
                        }
                    }).SingleAsync(cancellationToken);
                return Result.Success(data);
            }
        }
    }
}
