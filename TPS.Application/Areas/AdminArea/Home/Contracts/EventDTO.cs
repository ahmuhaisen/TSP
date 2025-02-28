namespace TPS.Application.Areas.AdminArea.Home.Contracts
{
    public class SocietySimpleDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }

    public class EventDTO
    {
        public Guid Id { get; set; }
        public string? LocationString { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RequestTime { get; set; }
        public string? type { get; set; }
        public required SocietySimpleDTO Host { get; set; }
        //public required MembersListDTO Member {  get; set; }
    }
}
