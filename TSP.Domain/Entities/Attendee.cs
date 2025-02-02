using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;

public class Attendee : Entity
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UniversityNumber { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Notes { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;

    public DateTime SubmittedAt { get; set; }


    public static class Factory
    {
        public static Attendee Create(
            string fullName,
            string email,
            string universityNumber,
            string? phoneNumber,
            string? notes,
            int departmentId,
            Guid eventId
        )
        {
            return new Attendee
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Email = email,
                UniversityNumber = universityNumber,
                PhoneNumber = phoneNumber,
                Notes = notes,
                DepartmentId = departmentId,
                EventId = eventId,
                SubmittedAt = DateTime.Now
            };
        }
    }

}
