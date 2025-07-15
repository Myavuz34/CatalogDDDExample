namespace Catalog.Domain.ValueObject;

public record ProductName
{
    public string Value { get; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Product name cannot be null or empty.", nameof(value));
        }
        Value = value;
    }

    public static implicit operator string(ProductName name) => name.Value;
    public static implicit operator ProductName(string value) => new(value);
}