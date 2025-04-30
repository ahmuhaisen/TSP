namespace TPS.Application.Areas.AdminArea.Events.Contracts;

public class EventDetailsDTO : EventDTO
{
    public string? Type { get; set; }
    public DateTime EndDateTime { get; set; }
    public bool? IsAdvisorApproved { get; set; }
    public bool? IsDeanAssistantApproved { get; set; }

    public required EventRequestDTO EventRequestDTO { get; set; }

    public required AdvisorBasicDto Advisor { get; set; }

    public required MemberDto EventManager { get; set; }
}

public class MemberDto
{
    public required Guid StudentId { get; set; }
    public required string StudentName { get; set; }
    public required string StudentEmail { get; set; }
    public string? StudentLogoId { get; set; }
    public required string StudentDepartment { get; set; }
    public required int JoinYear { get; set; }
    public required string StudentRole { get; set; }

    public required ICollection<string> JoinedSocietiesNames { get; set; } = [];
}

public class AdvisorBasicDto
{
    public required Guid AdvisorId { get; set; }
    public required string AdvisorName { get; set; }
    public required string AdvisorLogoId { get; set; }
}