using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Domain.Entities;

namespace TPS.Application.Areas.AdminArea.Events.Contracts
{
    public class EventDetailsDTO
    {
        public string? type { get; set; }
        public DateOnly DateOnly { get; set; }
        public TimeOnly TimeOnly { get; set; }
        public required string SocietyDescription { get; set; }
        public required string SocietyLogoId { get; set; }
        public required Guid AdvisorId { get; set; }
        public required string AdvisorName { get; set; }
        public required string AdvisorLogoId { get; set; }
        public required Guid StudentId { get; set; }
        public required string StudentName { get; set; }
        public required string StudentEmail { get; set; }
        public string? StudentLogoId { get; set; }
        public required string StudentDepartment {  get; set; }
        public required int JoinYear { get; set; }
        public required string StudentRole {  get; set; }
        public required ICollection<string> JoinedSocietiesNames { get; set; }
        public required EventsDTO EventDTO { get; set; }
        public required EventRequestDTO EventRequestDTO { get; set; }
    }
}
