using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPS.Application.Societies.Contracts;
using TPS.Application.Students.Contracts;

namespace TPS.Application.Home.Contracts
{
    public class EventDTO
    {
        public Guid Id { get; set; }
        public string? LocationString { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RequestTime { get; set; }
        public string? type { get; set; }
        public required SocietyDTO Host { get; set; }
        //public required MembersListDTO Member {  get; set; }
    }
}
public class SocietyDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
