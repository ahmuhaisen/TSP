using TSP.Domain.Entities;

namespace TPS.Application.Areas.AdminArea.Advisors.Contracts;
public class FacultyMemberDTO
{
    public string EmployeeNumber { get; set; } = null!;
    public int RankId { get; set; }
    public Rank Rank { get; set; } = null!;
    public ICollection<SocietyDTO> SocietiesAdvised { get; set; } = new List<SocietyDTO>();
}