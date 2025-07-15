namespace Catalog.Application.Dtos;

public record ProductDto(Guid Id, string Name, string Description, decimal Price, string Currency, int Stock);