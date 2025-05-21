using TSP.Domain.Primitives;

namespace TSP.Domain.Events
{
    public record SocietyCommitteeChangedDomainEvent(
        Guid Id,
        Guid SocietyId,
        Guid CommitteeId,
        string SocietyName
        )
        :DomainEvent(Id);
}
