using TSP.Domain.Primitives;

namespace TSP.Domain.Events
{
    public record SocietyJoinRequestStatusUpdateDomainEvent(
        Guid Id,
        Guid SocietyId,
        Guid StudentId,
        string SocietyName,
        bool decision
        )
        : DomainEvent(Id);
}
