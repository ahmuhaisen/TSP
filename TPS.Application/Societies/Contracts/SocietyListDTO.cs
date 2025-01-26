namespace TPS.Application.Societies.Contracts;

public class SocietyListDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string ?Description { get; set; }
    public required string LogoId { get; set; }
    public DateOnly CreationDate { get; set; }
    public string? ThemeColor { get; set; }
}
