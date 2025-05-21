using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Domain.Primitives;

namespace TSP.Domain.Events
{
    public record NewEventScheduledDomainEvent(
        Guid Id,
        Guid SocietyId,
        string SocietyName,
        string EventName) 
        : DomainEvent(Id);
}
