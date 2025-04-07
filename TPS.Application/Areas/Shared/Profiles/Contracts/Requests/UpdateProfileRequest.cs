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
        //public Guid Id { get; set; }
        public string? ProfileImageId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Number {  get; set; }
        public string? userType { get; set; }
    }
    public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(r => r.ProfileImageId)
                .Must(BeValidBase64)
                .WithMessage("ProfileImageId must be a valid Base64 string.")
                .When(r => r.ProfileImageId != null);

            RuleFor(r => r.FullName)
                .NotEmpty()
                .MaximumLength(60)
                .When(r => r.FullName != null);

            RuleFor(r => r.Email)
                .NotEmpty()
                .When(r => r.Email != null);

            RuleFor(r => r.Number)
                .Matches(@"^[A-Za-z]{2}\d{4}$")
                .When(r => r.userType == "Faculty" &&r.Number!=null);
            RuleFor(r => r.Number)
                .Matches(@"^\d{7}$")
                .When(r => r.userType == "Student" &&r.Number!=null);
        }
        private bool BeValidBase64(string? base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return true;

            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
    }
}
