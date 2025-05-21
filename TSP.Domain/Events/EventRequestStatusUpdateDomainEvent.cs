
using TSP.Domain.Primitives;

namespace TSP.Domain.Events
{
    public record EventRequestStatusUpdateDomainEvent(Guid Id,
                                                    Guid studentId,
                                                    Guid societyId,
                                                    string societyName, 
                                                    string eventName,
                                                    bool decision,
                                                    string? remark
        ) : DomainEvent(Id);
}
