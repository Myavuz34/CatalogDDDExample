using Catalog.Application.Dtos;
using MediatR;

namespace Catalog.Application.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;