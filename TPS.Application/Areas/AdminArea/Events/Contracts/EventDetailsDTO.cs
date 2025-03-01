namespace TPS.Application.Areas.AdminArea.Events.Contracts
{
    public class EventDetailsDTO
    {
        public string? Type { get; set; }
        public DateOnly EventDate { get; set; }
        public TimeOnly StartTime { get; set; } //TODO: Rename to StartTime
        public TimeOnly EndTime { get; set; } //TODO: Rename to EndTime
        public required string SocietyDescription { get; set; }
        public required string SocietyLogoId { get; set; }
        public required Guid AdvisorId { get; set; }
        public required string AdvisorName { get; set; }
        public required string AdvisorLogoId { get; set; }
        public required Guid StudentId { get; set; }
        public required string StudentName { get; set; }
        public required string StudentEmail { get; set; }
        public string? StudentLogoId { get; set; }
        public required string StudentDepartment {  get; set; }
        public required int JoinYear { get; set; }
        public required string StudentRole {  get; set; }
        public required ICollection<string> JoinedSocietiesNames { get; set; }
        public required EventDTO EventDTO { get; set; }
        public required EventRequestDTO EventRequestDTO { get; set; }

        //TODO: Add these for application history
        // IsAdvisorApproved: boolean
        // IsDeanApproved: boolean
    }
}
