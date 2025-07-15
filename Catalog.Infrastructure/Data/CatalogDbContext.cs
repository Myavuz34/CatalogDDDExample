using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data;

public class CatalogDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {
            builder.HasKey(p => p.Id);
            builder.OwnsOne(p => p.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value).HasColumnName("Name").IsRequired();
            });
            builder.OwnsOne(p => p.Price, priceBuilder =>
            {
                priceBuilder.Property(m => m.Amount).HasColumnName("PriceAmount").HasColumnType("decimal(18,2)").IsRequired();
                priceBuilder.Property(m => m.Currency).HasColumnName("PriceCurrency").HasMaxLength(3).IsRequired();
            });
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.Stock).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}