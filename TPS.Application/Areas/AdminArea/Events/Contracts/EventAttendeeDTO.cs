using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Domain.Entities;

namespace TPS.Application.Areas.AdminArea.Events.Contracts
{
    public class EventAttendeeDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UniversityNumber { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Notes { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public Guid EventId { get; set; }
        public Event Event { get; set; } = null!;

        public DateTime RegistrationDateTime { get; set; }
    }
}
