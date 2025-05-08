using TSP.Domain.Enums;
using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;

public class FeedbackSummary : Entity
{
    public Guid EventId { get; set; }

    public decimal AverageRating { get; set; }
    public int TotalResponses { get; set; }
    public Sentiment? Sentiment { get; set; } // e.g., "Positive", "Mixed", "Negative"
    public string? Topics { get; set; } // e.g., "Speaker, Venue, Timing"
    public string? AiSummary { get; set; } // LLM-generated text summary

    public DateTime CalculatedAt { get; set; }

    public Event Event { get; set; } = null!;
}


