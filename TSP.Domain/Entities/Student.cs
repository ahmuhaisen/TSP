namespace TSP.Domain.Entities;

public class Student : ApplicationUser
{
    public string UniversityNumber { get; set; } = null!;

    public ICollection<SocietiesMembers> SocietiesMembers { get; set; } = [];
    public ICollection<Event> RequestedEvents { get; set; } = [];
    public ICollection<MembershipRequest> MembershipRequests { get; set; } = [];
}
