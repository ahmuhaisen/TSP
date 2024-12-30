using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;

public sealed class Department : SystemTable
{
    public string Name { get; set; } = null!;
    public string Abbreviation { get; set; } = null!;

    public int SchoolId { get; set; }
    public School School { get; set; } = null!;

    public ICollection<ApplicationUser>? Users { get; set; }
}