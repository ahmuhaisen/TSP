using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Application.Attendees.Contracts;
using TPS.Infrastructure.Data;
using TSP.Domain.Shared;

namespace TPS.Application.Attendees.Queries;

public class GetEventAttendees
{
    public record Query(
        Guid EventId
        ) : IQuery<Result<List<AttendeeBasicDetailsDTO>>>;

    public sealed class Handler(ApplicationDbContext _context) : IQueryHandler<Query, Result<List<AttendeeBasicDetailsDTO>>>
    {
        public Task<Result<List<AttendeeBasicDetailsDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var canhandle = CanHandle(request);

            if (canhandle.IsFailure)
            {
                return Task.FromResult(
                    Result.Failure<List<AttendeeBasicDetailsDTO>>(canhandle.Error)
                    );
            }

            var attendees = _context.Attendees
                .AsNoTracking()
                .Include(a => a.Department)
                .Where(a => a.EventId == request.EventId)
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
            if (!_context.Events.Any(e => e.Id == query.EventId && e.IsAttendeesFormEnabled))
            {
                return Result.Failure(
                    Error.CustomError("Event not found or attendees form is disabled.")
                    );
            }

            return Result.Success();
        }
    }
}
