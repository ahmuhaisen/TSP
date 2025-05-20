using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TSP.Domain.Entities;

namespace TPS.Application.Areas.StudentArea.Societies.Contracts
{
    public class MembershipRequestDTO
    {
        public Guid Id { get; set; }
        public string Section { get; set; } = null!;
        public string ReasonForJoining { get; set; } = null!;
        public string SocietyLogo { get; set; } = string.Empty!;
        public RequestStatus Status { get; set; }
        public DateTime RequestedOn { get; set; }
        public required StudentBasicDTO StudentBasicDTO { get; set; }
    }
}
