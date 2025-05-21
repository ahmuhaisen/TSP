using FluentValidation;
using TPS.Application.Areas.AdminArea.Societies.Contracts.Requests;

namespace TPS.Application.Areas.Feedback.Contracts;


public class FeedbackAnswerRequest
{
    public Guid EventId { get; set; }
    public decimal Rating { get; set; }
    public string? Notes { get; set; }
}



public class FeedbackAnswerRequestValidator : AbstractValidator<FeedbackAnswerRequest>
{
    public FeedbackAnswerRequestValidator()
    {
        RuleFor(r => r.EventId)
            .NotEmpty()
            .NotNull();

        RuleFor(r => r.Rating)
            .NotEmpty()
            .NotNull()
            .InclusiveBetween(0, 5);

        RuleFor(r => r.Notes)
               .MaximumLength(1500)
               .When(r => !string.IsNullOrEmpty(r.Notes));
    }
}