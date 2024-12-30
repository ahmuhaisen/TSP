using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;

public class School : SystemTable
{
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// A string represents the Google Map location element
    /// </summary>
    public string? LocationString { get; set; }

    public ICollection<Department> Departments { get; set; } = new List<Department>();
}