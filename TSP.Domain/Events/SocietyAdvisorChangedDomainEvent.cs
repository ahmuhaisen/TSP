using TSP.Domain.Primitives;

namespace TSP.Domain.Events
{
    public record SocietyAdvisorChangedDomainEvent(
        Guid Id,
        Guid SocietyId,
        Guid OldAdvisorId,
        string SocietyName
        )
        :DomainEvent(Id);
}
