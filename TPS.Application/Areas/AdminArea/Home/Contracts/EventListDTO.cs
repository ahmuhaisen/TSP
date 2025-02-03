namespace TPS.Application.Areas.AdminArea.Home.Contracts
{
    public class EventListDTO
    {
        public Guid Id { get; set; }
        public required string EventName { get; set; }
        public required string SocietyName { get; set; }
        public required string LogoId { get; set; }
        public string? LocationString { get; set; }
        public DateTime StartTime { get; set; }
        public required bool isAdvised { get; set; }
        public required bool isFinished { get; set; }
    }
}
