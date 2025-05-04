namespace TPS.Application.Areas.Feedback.Contracts;

public class FeedbackWindowDto
{
    public DateTime OpenAt { get; set; }
    public DateTime CloseAt { get; set; }
    public bool CanSubmit { get; set; }
}