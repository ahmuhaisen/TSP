using TSP.Domain.Enums;
using TSP.Domain.Primitives;

namespace TSP.Domain.Events
{
    public record NewEventRequestSubmittedDomainEvent(
        Guid Id,
        Guid SocietyId,
        Guid UserId,
        UserType UserType,
        string SocietyName,
        string EventName)
        :DomainEvent(Id);
}
