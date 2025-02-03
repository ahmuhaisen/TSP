namespace TPS.Application.Areas.AdminArea.Home.Contracts
{
    public class RecentlyJoinedDTO
    {
        public Guid Id { get; set; }
        public string? ProfileImageId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? DepartmentName { get; set; }
        public DateOnly JoinDate { get; set; }
        public required string SocietyName { get; set; }
    }
}
