namespace TPS.Application.Areas.SuperAdmin.Contracts;


public class PendingAccountBasicDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserType { get; set; } = null!;
    public string? Rank { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime RegisteredAt { get; set; }
}
