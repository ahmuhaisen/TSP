using FluentValidation;

namespace TPS.Application.Attendees.Contracts.Requests;

public record CreateEventAttendeeRequest(
    string FullName,
    string Email,
    string UniversityNumber,
    string? PhoneNumber,
    string? Notes,
    int DepartmentId,
    Guid EventId
);

public class CreateEventAttendeeRequestValidator : AbstractValidator<CreateEventAttendeeRequest>
{
    public CreateEventAttendeeRequestValidator()
    {
        RuleFor(x => x.FullName)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.UniversityNumber)
            .MaximumLength(20)
            .NotEmpty();

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0);

        RuleFor(x => x.EventId)
            .NotEmpty();
    }
}