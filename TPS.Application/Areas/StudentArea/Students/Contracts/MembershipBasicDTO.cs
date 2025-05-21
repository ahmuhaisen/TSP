using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Domain.Entities;

namespace TPS.Application.Areas.StudentArea.Students.Contracts
{
    public class MembershipBasicDTO
    {
        public string Section { get; set; } = null!;
        public DateOnly SubmissionDate { get; set; }
        public RequestStatus Status { get; set; }
        public string SocietyName { get; set; } = null!;
        public string societyLogo { get; set; } = string.Empty!;
    }
}
