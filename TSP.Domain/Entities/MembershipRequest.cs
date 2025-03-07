using TSP.Domain.Primitives;

namespace TSP.Domain.Entities;

public class MembershipRequest : Entity
{
    public Guid SocietyId { get; set; }
    public Society Society { get; set; } = new();

    public Guid StudentId { get; set; }
    public Student Student { get; set; } = new();

    public string Section { get; set; } = null!;
    public string Motivation { get; set; } = null!;
    public DateTime RequestedOn { get; set; }
    public RequestStatus Status { get; set; }
}

public enum RequestStatus
{
    Pending = 0,
    Accepted = 1,
    Rejected = 2
}