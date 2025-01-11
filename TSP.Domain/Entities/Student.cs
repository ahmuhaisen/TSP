namespace TSP.Domain.Entities;

public class Student : ApplicationUser
{
    public string UniversityNumber { get; set; } = null!;

    public ICollection<SocietiesMembers> SocietiesMembers { get; set; } = new List<SocietiesMembers>();
    public ICollection<Event> RequestedEvents { get; set; } = new List<Event>();
}
