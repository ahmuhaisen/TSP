namespace TSP.Domain.Entities;

public class SocietiesMembers
{
    public Guid SocietyId { get; set; }
    public Society Society { get; set; } = null!;

    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public required string Position { get; set; }
    public DateOnly JoinDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsCommittee { get; set; }
}