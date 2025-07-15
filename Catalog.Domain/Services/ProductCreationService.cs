using Catalog.Domain.Entities;
using Catalog.Domain.ValueObject;

namespace Catalog.Domain.Services;

public class ProductCreationService
{
    public Product CreateProduct(string name, string description, decimal priceAmount, string currency, int stock)
    {
        // Domain kuralları burada uygulanabilir, örneğin, ürün adının benzersizliği kontrolü.
        // Bu örnekte basit tutulmuştur.
        ProductName productName = new(name);
        Money productPrice = new(priceAmount, currency);

        // Product.Create metodu doğrudan Domain Events'i tetikler
        return Product.Create(productName, description, productPrice, stock);
    }
}