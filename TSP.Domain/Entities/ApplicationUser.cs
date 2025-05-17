using Microsoft.AspNetCore.Identity;
using TSP.Domain.Enums;

namespace TSP.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Gender Gender { get; set; }
    public string? ProfileImageId { get; set; }
    
    public int DepartmentId { get; set; }
    public Department? Department { get; set; }

    public bool IsActive { get; set; } = false;
    public DateTime RegisteredAt { get; set; }
}

public class ApplicationRole : IdentityRole<Guid> { }