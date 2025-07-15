using Catalog.Application.Dtos;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            return null;
        }

        return new ProductDto(product.Id, product.Name.Value, product.Description, product.Price.Amount, product.Price.Currency, product.Stock);
    }
}