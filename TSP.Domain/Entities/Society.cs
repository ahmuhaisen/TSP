using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;


public class Society : Entity
{
    public override Guid ID { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LogoID { get; set; }
    public DateOnly CreationDate { get; set; }

    public string? ThemeColor { get; set; }

    //TODO: Foreign Key for the advisor
}
