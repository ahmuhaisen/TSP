using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Application.Areas.StudentArea.Home.Contracts
{
    public class StudentEventListDTO
    {
        public Guid Id { get; set; }
        public required string EventName { get; set; }
        public required string SocietyName { get; set; }
        public required string LogoId { get; set; }
        public string? LocationString { get; set; }
        public DateTime StartTime { get; set; }
        public required bool isActiveMember { get; set; }
    }
}
