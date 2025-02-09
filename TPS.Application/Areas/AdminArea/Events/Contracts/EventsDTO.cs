using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.AdminArea.Events.Contracts
{
    public class EventsDTO
    {
        public required Guid Id { get; set; }
        public required string EventName { get; set; }
        public DateTime DateTime { get; set; }
        public string? LocationString { get; set; }
        public required string ApprovalStatus { get; set; }
        public required string Description { get; set; }
        public required string SocietyName { get; set; }
    }
}
