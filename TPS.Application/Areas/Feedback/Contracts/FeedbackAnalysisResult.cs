using System.Text.Json.Serialization;
using TSP.Domain.Enums;

namespace TPS.Application.Areas.Feedback.Contracts;


public class FeedbackAnalysisResult
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Sentiment Sentiment { get; set; }

    public string Topics { get; set; } = null!;

    public string Summary { get; set; } = null!;
}