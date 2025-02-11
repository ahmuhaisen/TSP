

namespace TPS.Application.Areas.AdminArea.Students.Contracts;

public class StudentBasicDTO
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string LogoId { get; set; }
}
