using MediatR;
using TSP.Domain.Primitives;

namespace TSP.Domain.Events;


public record SocietyCreatedDomainEvent(Guid Id, Guid SocietyId, string SocietyName) : DomainEvent(Id);
