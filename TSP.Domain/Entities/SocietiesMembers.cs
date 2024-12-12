namespace TSP.Domain.Entities;

public class SocietiesMembers
{
    public Guid SocietyId { get; set; }
    public Society Society { get; set; } = null!;

    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public DateOnly MemberSince { get; set; }
}