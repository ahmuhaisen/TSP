namespace TPS.Application.Areas.Shared.Notifications.Contracts;


public class NotificationDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsSeen { get; set; }
    public string? ImageId { get; set; }

}
