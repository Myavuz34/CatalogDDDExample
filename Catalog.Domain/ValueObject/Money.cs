namespace Catalog.Domain.ValueObject;

public record Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0, currency);
    
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
        {
            throw new InvalidOperationException("Currencies must be the same to add.");
        }
        return new Money(Amount + other.Amount, Currency);
    }
}