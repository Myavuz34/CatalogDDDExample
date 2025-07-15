using System.Reflection;
using Catalog.Application.Commands;
using Catalog.Domain.Repositories;
using Catalog.Domain.Services;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;

// MediatR için

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DDD ve Altyapı Bağımlılıkları
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseInMemoryDatabase("CatalogDb")); // Basit örnek için In-Memory DB

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductCreationService>(); // Domain servisini kaydet

// MediatR'ı kaydet
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())); // API assembly'sindeki handler'ları bul
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand)
        .Assembly)); // Application assembly'sindeki handler'ları bul

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();