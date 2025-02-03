namespace TPS.Application.Areas.AdminArea.Societies.Contracts;

public class SocietyDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LogoId { get; set; }
    public DateOnly CreationDate { get; set; }
    public string? ThemeColor { get; set; }

    public int NumberOfMembers { get; set; }
    public required FacultyMemberBasicDTO Advisor { get; set; }
}

public class FacultyMemberBasicDTO
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
}