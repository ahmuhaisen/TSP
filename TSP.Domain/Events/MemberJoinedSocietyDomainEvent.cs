using TSP.Domain.Primitives;

namespace TSP.Domain.Events
{
    public record MemberJoinedSocietyDomainEvent(
        Guid Id,
        Guid SocietyId,
        string SocietyName,
        string UserNameJoined)
        : DomainEvent(Id);
}
