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
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Number {  get; set; }
    }
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(r => r.FullName)
                .NotEmpty()
                .MaximumLength(60)
                .When(r => r.FullName != null);

            RuleFor(r => r.Email)
                .NotEmpty()
                .When(r => r.Email != null);

            RuleFor(r => r.Number)
                .NotEmpty();
            //    .Matches(@"^[A-Za-z]{2}\d{4}$")
            //    .When(r => r.userType == "Faculty" &&r.Number!=null);
            //RuleFor(r => r.Number)
            //    .Matches(@"^\d{7}$")
            //    .When(r => r.userType == "Student" && r.Number != null);
        }
    }
}
