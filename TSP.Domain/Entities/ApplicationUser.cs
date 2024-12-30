using Microsoft.AspNetCore.Identity;

namespace TSP.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string? ProfileImageId { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
}

public class ApplicationRole : IdentityRole<Guid> { }

public enum Gender
{
    Male,
    Female
}