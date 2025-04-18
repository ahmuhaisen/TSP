using MediatR;

namespace TSP.Domain.Primitives;


public record DomainEvent(Guid Id) : INotification;
