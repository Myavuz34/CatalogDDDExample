namespace Catalog.Domain.Events;

public record ProductCreatedEvent(Guid ProductId, string ProductName, decimal Price, string Currency);