using Catalog.Domain.Repositories;
using Catalog.Domain.Services;
using MediatR;

namespace Catalog.Application.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductCreationService _productCreationService;

    public CreateProductCommandHandler(IProductRepository productRepository, ProductCreationService productCreationService)
    {
        _productRepository = productRepository;
        _productCreationService = productCreationService;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _productCreationService.CreateProduct(
            request.Name,
            request.Description,
            request.PriceAmount,
            request.Currency,
            request.Stock
        );

        await _productRepository.AddAsync(product);
        return product.Id;
    }
}