using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.StudentArea.Societies.Contracts.Requests
{
    public class UpdateMembershipRequest
    {
        public Guid UserId { get; set; }
        public Guid MembershipRequestId { get; set; }
        public Guid SocietyId { get; set; }
        public bool isAccepted { get; set; }
    }
    public class UpdateMembershipRequestValidator : AbstractValidator<UpdateMembershipRequest>
    {
        public UpdateMembershipRequestValidator()
        {
            RuleFor(r => r.UserId)
                .NotNull()
                .NotEmpty();
            RuleFor(r => r.MembershipRequestId)
                .NotNull()
                .NotEmpty();
            RuleFor(r=>r.SocietyId)
                .NotNull()
                .NotEmpty();
            RuleFor(r => r.isAccepted)
                .NotNull()
                .NotEmpty();
        }
    }
}
