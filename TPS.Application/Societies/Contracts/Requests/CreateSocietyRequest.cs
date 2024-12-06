using FluentValidation;

namespace TPS.Application.Societies.Contracts.Requests;

public class CreateSocietyRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LogoID { get; set; }
    public DateOnly CreationDate { get; set; }
    public string? ThemeColor { get; set; }
}

public class CreateSocietyRequestValidator : AbstractValidator<CreateSocietyRequest>
{
    public CreateSocietyRequestValidator()
    {
        RuleFor(r => r.Name)
               .NotNull()
               .NotEmpty()
               .MaximumLength(50);

        RuleFor(r => r.Description)
               .NotNull()
               .NotEmpty()
               .MaximumLength(200);

        RuleFor(r => r.LogoID)
               .NotNull()
               .NotEmpty();

        RuleFor(r => r.CreationDate)
               .NotNull()
               .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

        RuleFor(r => r.ThemeColor)
               .Matches(@"^#[0-9A-Fa-f]{6}$")
               .When(r => !string.IsNullOrEmpty(r.ThemeColor));
    }
}