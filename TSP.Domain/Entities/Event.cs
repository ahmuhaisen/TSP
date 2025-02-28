using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Domain.Primitives;

namespace TSP.Domain.Entities
{
    public class Event : Entity
    {
        /// <summary>
        /// A string represents the Google Map location element
        /// </summary>
        public string? LocationString { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RequestTime { get; set; }
        public string? type { get; set; }
        public Guid SocietyId { get; set; }
        [ForeignKey(nameof(SocietyId))]
        public Society Society { get; set; } = null!;
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public bool IsAttendeesFormEnabled { get; set; } = false;
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
