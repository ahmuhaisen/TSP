using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Events.Contracts
{
    public class EventListDTO
    {
        public Guid Id { get; set; }
        public string? LocationString { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RequestTime { get; set; }
        public string? type { get; set; }
    }
}
