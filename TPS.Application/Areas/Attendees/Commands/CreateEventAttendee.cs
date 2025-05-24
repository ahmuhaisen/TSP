using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions.Messaging;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Areas.Attendees.Commands;

public class CreateEventAttendee
{
    public record Command(
        string FullName,
        string Email,
        string UniversityNumber,
        string? PhoneNumber,
        string? Notes,
        int DepartmentId,
        Guid EventId
    ) : ICommand<Result>;

    public class Handler(ApplicationDbContext _context) : ICommandHandler<Command, Result>
    {
        public Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var canHandleResult = canHandle(request);

            if (canHandleResult.IsFailure)
                return Task.FromResult(canHandleResult);

            var eventId = GetEventId(request.EventId);

            var attendee = Attendee.Factory.Create(
                request.FullName,
                request.Email,
                request.UniversityNumber,
                request.PhoneNumber,
                request.Notes,
                request.DepartmentId,
                eventId
            );

            _context.Add(attendee);

            var saveResult = _context.SaveChanges();
            if (saveResult <= 0)
                return Task.FromResult(Result.Failure(Error.InternalServerError("Error: error while creating the entity")));

            return Task.FromResult(Result.Success());
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

        private Result canHandle(Command command)
        {
            if (!isValidEventId(command.EventId))
            {
                return Result.Failure(
                    Error.CustomError("Event does not exist or attendees form is disabled.")
                    );
            }

            if (isExistedAttendee(command.Email, command.UniversityNumber, command.EventId))
            {
                return Result.Failure(
                    Error.CustomError($"{command.Email} or {command.UniversityNumber} already registered for this event.")
                    );
            }

            if (!isValidDepartmentId(command.DepartmentId))
            {
                return Result.Failure(
                    Error.NotFound(nameof(Department))
                    );
            }

            return Result.Success();
        }

        private bool isValidEventId(Guid eventId)
        {
            var isExist = _context.Events.Any(e => e.Id == eventId && e.IsAttendeesFormEnabled);

            if (!isExist)
            {
                isExist = _context.EventsApproval.Include(e => e.Event)
                    .Any(e => e.Id == eventId && e.Event.IsAttendeesFormEnabled);
            }
            
            return isExist;
        }

        private bool isExistedAttendee(string email, string universityNumber, Guid eventId)
        {
            return _context.Attendees.Any(a =>
                a.Email.Equals(email) &&
                a.UniversityNumber.Equals(universityNumber) &&
                a.EventId == eventId
            );
        }

        private bool isValidDepartmentId(int departmentId)
        {
            return _context.Departments.Any(d => d.Id == departmentId);
        }
    }
}
