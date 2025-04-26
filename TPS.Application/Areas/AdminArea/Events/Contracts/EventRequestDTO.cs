using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.AdminArea.Events.Contracts
{
    public class EventRequestDTO
    {
        public DateTime RequestTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public required string AdvisorEmail { get; set; }
        public required bool IsAttendeesFormEnabled { get; set; }
        public required ICollection<ApprovalAdministrators> Admins { get; set; }
    }
    public class ApprovalAdministrators
    {
        public required string FacultyMemberName { get; set; }
        public required string FacultyMemberEmail { get; set; }
        public required string Rank { get; set; }
    }
}
