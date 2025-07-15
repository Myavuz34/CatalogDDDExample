using Catalog.Domain.Events;
using Catalog.Domain.ValueObject;

namespace Catalog.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public ProductName Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public int Stock { get; private set; }

    // EF Core için boş kurucu (private olarak)
    private Product() { } 

    private Product(ProductName name, string description, Money price, int stock)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }

    public static Product Create(ProductName name, string description, Money price, int stock)
    {
        var product = new Product(name, description, price, stock);
        // Domain Event'i yayınla (Gerçek uygulamada bir Event Dispatcher kullanılır)
        DomainEvents.Raise(new ProductCreatedEvent(product.Id, product.Name.Value, product.Price.Amount, product.Price.Currency));
        return product;
    }

    public void UpdateDetails(ProductName newName, string newDescription, Money newPrice)
    {
        // İş kuralları burada uygulanır
        if (newPrice.Amount < 0)
        {
            throw new ArgumentException("Price cannot be negative.");
        }
        Name = newName;
        Description = newDescription;
        Price = newPrice;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive.");
        }
        Stock += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive.");
        }
        if (Stock < quantity)
        {
            throw new InvalidOperationException("Not enough stock.");
        }
        Stock -= quantity;
    }
}

// Basit bir DomainEvents sınıfı (Gerçek uygulamalarda daha gelişmiş bir yapı kullanılır)
public static class DomainEvents
{
    private static List<object> _events = new List<object>();

    public static void Raise<TEvent>(TEvent @event)
    {
        _events.Add(@event);
        // Burada gerçek bir olay yayınlama mekanizması (örneğin MediatR) kullanılabilir.
        Console.WriteLine($"Domain Event Raised: {@event.GetType().Name}");
    }

    public static IReadOnlyList<object> GetAndClearEvents()
    {
        var events = _events.AsReadOnly();
        _events = new List<object>();
        return events;
    }
}