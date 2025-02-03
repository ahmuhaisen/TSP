namespace TPS.Application.Areas.AdminArea.Home.Contracts
{
    public class HomeStatisticsDTO
    {
        public required int TotalMembers { get; set; }
        public required int TotalSocieties { get; set; }
        public required int TotalCompletedEvents { get; set; }
        public int? TotalAttendees { get; set; }
    }
}
