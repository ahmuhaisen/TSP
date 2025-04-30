using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.Shared.Profiles.Contracts.Requests
{
    public class UpdateProfileRequest
    {
        public string? ProfileImageId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Number {  get; set; }
    }
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(r => r.FirstName)
                .NotEmpty()
                .MaximumLength(30)
                .When(r => r.FirstName != null);
            RuleFor(r => r.LastName)
                .NotEmpty()
                .MaximumLength(30)
                .When(r => r.LastName != null);

            RuleFor(r => r.Email)
                .NotEmpty()
                .When(r => r.Email != null);

            RuleFor(r => r.Number)
                .NotEmpty();
        }
    }
}
