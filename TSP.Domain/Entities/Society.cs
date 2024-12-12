using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;


public class Society : Entity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LogoId { get; set; }
    public DateOnly CreationDate { get; set; }
    public string? ThemeColor { get; set; }

    public Guid AdvisorId { get; set; }
    public FacultyMember Advisor { get; set; } = null!;

    public ICollection<SocietiesMembers> SocietiesMembers { get; set; } = new List<SocietiesMembers>();
}
