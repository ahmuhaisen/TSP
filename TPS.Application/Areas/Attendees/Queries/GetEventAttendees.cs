using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Areas.Attendees.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Attendees.Queries;

public class GetEventAttendees
{
    public record Query(
        Guid EventId
        ) : IQuery<Result<List<AttendeeBasicDetailsDTO>>>;

    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<List<AttendeeBasicDetailsDTO>>>
    {
        public Task<Result<List<AttendeeBasicDetailsDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var canHandle = CanHandle(request);

            if (canHandle.IsFailure)
            {
                return Task.FromResult(
                    Result.Failure<List<AttendeeBasicDetailsDTO>>(canHandle.Error)
                    );
            }

            var eventId = GetEventId(request.EventId);

            var attendees = _context.Attendees
                .AsNoTracking()
                .Include(a => a.Department)
                .Where(a => a.EventId == eventId)
                .Select(a => new AttendeeBasicDetailsDTO
                {
                    FullName = a.FullName,
                    Email = a.Email,
                    UniversityNumber = a.UniversityNumber,
                    DepartmentName = a.Department.Name,
                    Notes = a.Notes
                })
                .ToList();

            return Task.FromResult(Result.Success(attendees));
        }

        private Result CanHandle(Query query)
        {
            if (
                !_context.EventsApproval
                .Include(e => e.Event)
                .Any(e => (e.Id == query.EventId || e.Event.Id == query.EventId) && e.Event.IsAttendeesFormEnabled)
                )
            {
                return Result.Failure(
                    Error.CustomError("Event not found or attendees form is disabled.")
                    );
            }

            return Result.Success();
        }

        private Guid GetEventId(Guid eventId)
        {
            var id = _context.Events
                .Where(e => e.Id == eventId)
                .Select(e => e.Id)
                .FirstOrDefault();

            if (id != Guid.Empty)
                return id;

            id = _context.EventsApproval
                .Include(e => e.Event)
                .Where(e => e.Id == eventId)
                .Select(e => e.Event.Id)
                .FirstOrDefault();

            return id;
        }
    }
}
