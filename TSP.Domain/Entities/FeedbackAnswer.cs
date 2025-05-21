using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;


public class FeedbackAnswer : Entity
{
    public Guid EventId { get; set; }
    public decimal Rating { get; set; } // 1 to 5
    public string? Notes { get; set; }
    public DateTime SubmittedAt { get; set; }

    public Event Event { get; set; } = null!;
}
