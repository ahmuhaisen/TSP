
namespace TPS.Application.Areas.AdminArea.Statistics.Contracts;

public class EventAttendanceCountDTO
{
    public required string EventName { get; set; }
    public int count { get; set; }
}
