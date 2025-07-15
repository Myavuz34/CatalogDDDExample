# CatalogDDDExample

Bu proje, Domain-Driven Design (DDD) yaklaşımı ile geliştirilmiş bir ürün katalog yönetim sistemidir. Katmanlı mimari ve modern .NET teknolojileri kullanılarak oluşturulmuştur.

## Proje Yapısı

```
CatalogDDDExample.sln
Catalog.Api/           # API katmanı (Web API)
Catalog.Application/   # Uygulama servisleri ve CQRS
Catalog.Domain/        # Domain modelleri, servisler, eventler
Catalog.Infrastructure/# Veri erişim katmanı (EF Core)
```
## Proje Şeması

```
           +-------------------+
           |   Catalog.Api     |
           +-------------------+
                    |
                    v
           +-------------------+
           | Catalog.Application|
           +-------------------+
                    |
                    v
           +-------------------+
           |  Catalog.Domain   |
           +-------------------+
                    |
                    v
           +-------------------+
           |Catalog.Infrastructure|
           +-------------------+
                    |
                    v
           +-------------------+
           |   Database        |
           +-------------------+
```

## Dosya ve Klasör Yapısı Şeması

```
CatalogDDDExample/
├── CatalogDDDExample.sln
├── Catalog.Api/
│   ├── Controllers/
│   ├── Properties/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── ...
├── Catalog.Application/
│   ├── Commands/
│   ├── Dtos/
│   ├── Queries/
│   └── ...
├── Catalog.Domain/
│   ├── Entities/
│   ├── Events/
│   ├── Repositories/
│   ├── Services/
│   ├── ValueObject/
│   └── ...
├── Catalog.Infrastructure/
│   ├── Data/
│   ├── Respositories/
│   └── ...
└── Readme.md
```

- **Catalog.Infrastructure**: Veri tabanı işlemleri ve repository implementasyonları.

## Kurulum
1. Depoyu klonlayın:
   ```bash
   git clone https://github.com/Myavuz34/CatalogDDDExample.git
   ```
2. Gerekli .NET SDK'yı yükleyin (.NET 9.0 önerilir).
3. Bağımlılıkları yükleyin:
   ```bash
   dotnet restore
   ```

## Çalıştırma
API'yi başlatmak için:
```bash
cd Catalog.Api
dotnet run
```
Varsayılan olarak `https://localhost:5001` adresinde çalışır.

## Temel Özellikler
- Ürün ekleme, güncelleme, silme ve listeleme
- CQRS ve MediatR ile ayrık komut/sorgu işleme
- Entity, Value Object ve Domain Event kullanımı
- Katmanlı ve test edilebilir mimari


## Kullanılan Teknolojiler
- .NET 9.0
- Entity Framework Core
- MediatR
- DDD Prensipleri


## Kullanım

API, basit bir Ürün Kataloğu yönetimi için aşağıdaki endpoint'leri sunar:

### Ürün Oluşturma

   •	Endpoint: POST /api/products
   •	Body (JSON): JSON  {
   •	  "name": "Yeni Ürün Adı",
   •	  "description": "Bu, yeni oluşturulacak ürünün açıklamasıdır.",
   •	  "priceAmount": 99.99,
   •	  "currency": "USD",
   •	  "stock": 100
   •	}
   •	   
   •	Başarılı Yanıt (201 Created): JSON  "e9d8f7b6-c5a4-3b21-d0e1-f2c3a4b5d6e7" // Oluşturulan ürünün GUID'si
   •	   

### Ürün Detaylarını Getirme

   •	Endpoint: GET /api/products/{id}
   •	Örnek: GET /api/products/e9d8f7b6-c5a4-3b21-d0e1-f2c3a4b5d6e7
   •	Başarılı Yanıt (200 OK): JSON  {
   •	  "id": "e9d8f7b6-c5a4-3b21-d0e1-f2c3a4b5d6e7",
   •	  "name": "Yeni Ürün Adı",
   •	  "description": "Bu, yeni oluşturulacak ürünün açıklamasıdır.",
   •	  "price": 99.99,
   •	  "currency": "USD",
   •	  "stock": 100
   •	}
   •	   
   •	Ürün Bulunamadı (404 Not Found)

## Test Etme

Proje, in-memory veritabanı (Entity Framework Core InMemory) kullandığından, herhangi bir veritabanı kurulumuna gerek kalmadan doğrudan çalıştırılabilir ve test edilebilir. API endpoint'lerini test etmek için Swagger UI'ı veya Postman gibi bir araç kullanabilirsiniz.

## Katkı Sağlama
Pull request'ler ve issue'lar ile katkıda bulunabilirsiniz.

## Lisans
MIT
