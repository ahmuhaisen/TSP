using TSP.Domain.Primitives;

namespace TSP.Domain.Events
{
    public record MemberLeftSocietyDomainEvent(
        Guid Id,
        Guid SocietyId,
        string SocietyName,
        string UserNameLeft)
        : DomainEvent(Id);
}
