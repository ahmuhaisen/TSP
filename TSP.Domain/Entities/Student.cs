namespace TSP.Domain.Entities;

public class Student : ApplicationUser
{
    public string Major { get; set; } = null!;
    public string UniversityNumber { get; set; } = null!;

    public ICollection<SocietiesMembers> SocietiesMembers { get; set; } = new List<SocietiesMembers>();
}
