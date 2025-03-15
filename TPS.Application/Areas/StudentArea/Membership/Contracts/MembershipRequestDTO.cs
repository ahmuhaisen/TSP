using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TSP.Domain.Entities;

namespace TPS.Application.Areas.StudentArea.Membership.Contracts
{
    public class MembershipRequestDTO
    {
        public string Section { get; set; } = null!;
        public string ReasonForJoining { get; set; } = null!;
        public RequestStatus Status { get; set; }
        public required StudentBasicDTO StudentBasicDTO { get; set; }
    }
}
