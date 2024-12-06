namespace TPS.Application.Societies.Contracts;

public class SocietyDTO
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string LogoID { get; set; }
    public DateOnly CreationDate { get; set; }
    public string? ThemeColor { get; set; }
}