namespace TPS.Application.Areas.AdminArea.Events.Contracts;

public class EventDTO
{
    public required Guid Id { get; set; }
    public required string EventName { get; set; }
    public DateTime StartDateTime { get; set; }
    public string? LocationString { get; set; }
    public required string ApprovalStatus { get; set; }

    //TODO: remove these society related properties
    public required string Description { get; set; }
    public required string SocietyName { get; set; }

    
    //public required EventSocietyBasicDto Society { get; set; }
}

