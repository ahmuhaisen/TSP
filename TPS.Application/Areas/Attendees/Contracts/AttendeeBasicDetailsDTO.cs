namespace TPS.Application.Areas.Attendees.Contracts;

public class AttendeeBasicDetailsDTO
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UniversityNumber { get; set; } = null!;
    public string DepartmentName { get; set; } = null!;
    public string? Notes { get; set; }
}
