using TSP.Domain.Primitives;

namespace TSP.Domain.Events;


public record FeedbackSubmittedDomainEvent(Guid Id, Guid feedbackId, Guid eventId, decimal rating, string? notes) : DomainEvent(Id);