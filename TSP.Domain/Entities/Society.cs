using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;


public class Society : Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string LogoId { get; set; } = null!;
    public DateOnly CreationDate { get; set; }
    public string? ThemeColor { get; set; }

    public Guid AdvisorId { get; set; }
    public FacultyMember Advisor { get; set; } = null!;
    public ICollection<SocietiesMembers> SocietiesMembers { get; set; } = new List<SocietiesMembers>();
    public ICollection<Event>Events {get; set; } = new List<Event>();

}
