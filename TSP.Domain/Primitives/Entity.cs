using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace TSP.Domain.Primitives;

public abstract class Entity
{
    public Guid Id { get; set; }



    private List<DomainEvent> _domainEvents = [];

    public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void RaiseDomainEvent(DomainEvent eventItem) => _domainEvents.Add(eventItem);

    public void RemoveDomainEvent(DomainEvent eventItem) => _domainEvents.Remove(eventItem);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
