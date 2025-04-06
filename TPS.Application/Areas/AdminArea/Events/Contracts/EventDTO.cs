using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TPS.Application.Areas.AdminArea.Events.Contracts;

public class EventDTO
{
    public required Guid Id { get; set; }
    public required string EventName { get; set; }
    public DateTime StartDateTime { get; set; }
    public string? LocationString { get; set; }
    public required string ApprovalStatus { get; set; }
    public required string EventDescription { get; set; }
    public required EventSocietyBasicDto EventSociety { get; set; }
}
public class EventSocietyBasicDto
{
    public required string SocietyName { get; set; }
    public required string SocietyDescription { get; set; }
    public required string SocietyLogoId { get; set; }
}