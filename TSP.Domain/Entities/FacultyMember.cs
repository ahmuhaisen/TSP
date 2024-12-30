namespace TSP.Domain.Entities;

public class FacultyMember : ApplicationUser
{
    public string EmployeeNumber { get; set; } = null!;

    public int RankId { get; set; }
    public Rank Rank { get; set; } = null!;

    public ICollection<Society> SocietiesAdvised { get; set; } = new List<Society>();
}
