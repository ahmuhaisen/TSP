using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;


public class Notification : Entity
{
    public Guid UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = default!;

    public string Subject { get; set; } = default!;
    public string? Body { get; set; }

    public bool IsSeen { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? SeenAt { get; set; }

    public string? ImageId { get; set; }
}
