using MediatR;

namespace Catalog.Application.Commands;

public record CreateProductCommand(string Name, string Description, decimal PriceAmount, string Currency, int Stock) : IRequest<Guid>;