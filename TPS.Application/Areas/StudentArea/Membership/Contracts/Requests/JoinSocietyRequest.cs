using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.StudentArea.Membership.Contracts.Requests
{
    public class JoinSocietyRequest
    {
        public Guid StudentId { get; set; }
        public string SocietyName { get; private init; } = null!;
        public string Section { get; set; } = null!;
        public string Motivation { get; set; } = null!;
    }
    public class JoinSocietyRequestValidator : AbstractValidator<JoinSocietyRequest>
    {
        public JoinSocietyRequestValidator()
        {
            RuleFor(r => r.StudentId);
            RuleFor(r => r.SocietyName);
            RuleFor(r => r.Section)
                .NotNull()
                .NotEmpty();
            RuleFor(r => r.Motivation)
                .NotNull()
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
