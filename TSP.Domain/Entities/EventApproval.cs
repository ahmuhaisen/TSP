using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Domain.Primitives;

namespace TSP.Domain.Entities
{
    public class EventApproval :Entity
    {
        public bool AdvisorApproval{ get; set; }
        public bool DeanAssistantApproval { get; set; }
        public string? Remarks { get; set; }
        public DateTime DecisionDate { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }= null!;
        public Guid FacultyMemberId { get; set; }
        public FacultyMember FacultyMember { get; set; } = null!;
    }
}
