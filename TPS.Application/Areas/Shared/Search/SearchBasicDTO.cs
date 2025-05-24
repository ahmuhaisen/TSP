
namespace TPS.Application.Areas.Shared.Search;

public class SearchBasicDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public string ?Description { get; set; }
    public string ?LogoId { get; set; }
    public bool IsFacultyMember { get; set; }
}
