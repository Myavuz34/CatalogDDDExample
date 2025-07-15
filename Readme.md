# Domain-Driven Design (DDD) ile .NET 9 Web API Örneği: Ürün Kataloğu

Bu depo, **Domain-Driven Design (DDD)** prensiplerini uygulayarak geliştirilmiş basit bir Ürün Kataloğu Web API'sini içermektedir. Proje, .NET 9 kullanılarak oluşturulmuş olup, DDD'nin temel yapı taşları olan Varlıklar (Entities), Değer Nesneleri (Value Objects), Topluluklar (Aggregates), Domain Servisleri ve Ortak Dil (Ubiquitous Language) gibi kavramları pratik bir şekilde göstermeyi amaçlamaktadır. Ayrıca, [MediatR](https://github.com/jbogard/MediatR) kütüphanesi kullanılarak **CQRS (Command Query Responsibility Segregation)** deseninin basit bir uygulaması da mevcuttur.

---

## İçindekiler

- [Domain-Driven Design (DDD) Nedir?](#domain-driven-design-ddd-nedir)
- [Mimari](#mimari)
- [Proje Yapısı](#proje-yapısı)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Test Etme](#test-etme)
- [Katkıda Bulunma](#katkıda-bulunma)
- [Lisans](#lisans)

---

## Domain-Driven Design (DDD) Nedir?

**Domain-Driven Design (DDD)**, yazılımın temel odak noktasının iş alanının (domain) karmaşıklığı olması gerektiğini savunan bir yazılım geliştirme yaklaşımıdır. Eric Evans tarafından popülerleştirilen bu metodoloji, geliştiricilerin, iş uzmanları ve domain uzmanlarıyla yakın işbirliği içinde, iş gereksinimlerini doğru bir şekilde yansıtan bir yazılım modeli oluşturmasını hedefler. Temel amaç, karmaşık iş mantığını anlaşılır, bakımı kolay ve sürdürülebilir bir şekilde kodlamak ve yazılımdaki her kavramın iş dünyasındaki karşılığını bulmasını sağlamaktır.

Bu projede kullanılan temel DDD yapı taşları şunlardır:

* **Varlıklar (Entities):** Benzersiz bir kimliğe sahip ve zaman içinde değişebilen nesnelerdir. (`Product`)
* **Değer Nesneleri (Value Objects):** Kimliği olmayan, değişmez ve nitelikleriyle tanımlanan nesnelerdir. (`Money`, `ProductName`)
* **Topluluklar (Aggregates):** Bir kök varlık etrafında gruplanmış varlık ve değer nesnelerinden oluşan, tutarlılık sınırı olan kümelerdir. (`Product` bu örnekte Aggregate Root'tur.)
* **Domain Hizmetleri (Domain Services):** Bir varlık veya değer nesnesine doğal olarak ait olmayan, ancak iş alanında önemli bir işlemi gerçekleştiren operasyonlardır. (`ProductCreationService`)
* **Domain Olayları (Domain Events):** İş alanında gerçekleşen önemli olayları temsil eder. (`ProductCreatedEvent`)
* **Ortak Dil (Ubiquitous Language):** İş uzmanları ve geliştiriciler arasında ortak bir anlayış ve iletişim sağlamak için kullanılan terimler ve kavramlar bütünü.

---

## Mimari

Bu proje, **Katmanlı Mimari** ve **Temiz Mimari** prensiplerinden ilham alarak DDD prensipleriyle tasarlanmıştır. Ayrıca, **CQRS (Command Query Responsibility Segregation)** deseni, MediatR aracılığıyla komut ve sorgu işlemlerini ayırmak için kullanılmıştır.

Aşağıdaki şema, projenin genel mimarisini ve katmanlar arasındaki ilişkileri göstermektedir:

graph TD
    subgraph Presentation Layer (Catalog.Api)
        A[API Controllers]
    end

    subgraph Application Layer (Catalog.Application)
        B[Commands] --> C(Command Handlers)
        D[Queries] --> E(Query Handlers)
        C -- Uses --> F[Domain Services]
        E -- Uses --> G[Repositories (Interfaces)]
    end

    subgraph Domain Layer (Catalog.Domain)
        F -- Uses --> H[Entities]
        H -- Uses --> I[Value Objects]
        J[Domain Events]
        G[Repositories (Interfaces)]
    end

    subgraph Infrastructure Layer (Catalog.Infrastructure)
        K[Database Context]
        L[Repositories (Implementations)]
    end

    A -- Calls --> C
    A -- Calls --> E
    C -- Interacts With --> G
    L -- Implements --> G
    K -- Manages --> H
    H -- Raises --> J

Katman Sorumlulukları:

Catalog.Api (Presentation Layer): Gelen HTTP isteklerini karşılar, uygulama katmanındaki komut ve sorguları tetikler ve yanıtları döndürür.

Catalog.Application (Application Layer): İş akışlarını yönetir, dış istekleri domain modeline dönüştürür ve domain operasyonlarını çağırır. MediatR aracılığıyla komut ve sorgu işleyicilerini barındırır. DTO'lar (Data Transfer Objects) bu katmanda tanımlanır.

Catalog.Domain (Domain Layer): İş mantığının kalbidir. İş varlıkları, değer nesneleri, topluluklar, domain servisleri ve domain olayları burada tanımlanır. Bu katman diğer katmanlardan bağımsızdır.

Catalog.Infrastructure (Infrastructure Layer): Veri kalıcılığı (Entity Framework Core ile In-Memory Database), dış servis entegrasyonları gibi teknik detayları içerir. Domain katmanında tanımlanan repository arayüzlerinin somut uygulamalarını sağlar.

Proje Yapısı
Proje, aşağıdaki klasör ve alt klasör yapısına sahiptir:

CatalogSolution.sln
├─── Catalog.Api (Web API projesi, Sunum Katmanı)
│    ├─── Controllers           # API Endpoints
│    └─── Program.cs            # Bağımlılık Enjeksiyonu, Middleware
│
├─── Catalog.Application (Sınıf kütüphanesi, Uygulama Katmanı)
│    ├─── Commands              # Komut Tanımları
│    ├─── Dtos                  # Veri Transfer Nesneleri
│    ├─── Queries               # Sorgu Tanımları
│    └─── (Handlers)            # Komut ve Sorgu İşleyicileri (MediatR)
│
├─── Catalog.Domain (Sınıf kütüphanesi, Domain Katmanı)
│    ├─── Entities              # Domain Varlıkları (Aggregate Roots)
│    ├─── Events                # Domain Olayları
│    ├─── Repositories          # Repository Arayüzleri
│    ├─── Services              # Domain Servisleri
│    └─── ValueObjects          # Değer Nesneleri
│
└─── Catalog.Infrastructure (Sınıf kütüphanesi, Altyapı Katmanı)
     ├─── Data                  # DbContext ve EF Core Konfigürasyonları
     └─── Repositories          # Repository Uygulamaları

Kurulum
Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları izleyin:

Ön Koşullar:

.NET 9 SDK (veya daha yeni bir sürüm) yüklü olmalıdır.

Tercihen Visual Studio 2022 veya Visual Studio Code gibi bir IDE.

Projeyi Klonlayın:

git clone https://github.com/Myavuz34/CatalogDDDExample.git
cd CatalogDDDExample

Bağımlılıkları Yükleyin:

dotnet restore

Projeyi Çalıştırın:

dotnet run --project Catalog.Api

Uygulama başarıyla başlatıldığında, konsolda Swagger UI URL'sini (genellikle https://localhost:XXXX/swagger) göreceksiniz.

Kullanım
API, basit bir Ürün Kataloğu yönetimi için aşağıdaki endpoint'leri sunar:

Ürün Oluşturma
Endpoint: POST /api/products

Body (JSON):

{
  "name": "Yeni Ürün Adı",
  "description": "Bu, yeni oluşturulacak ürünün açıklamasıdır.",
  "priceAmount": 99.99,
  "currency": "USD",
  "stock": 100
}

Başarılı Yanıt (201 Created):

"e9d8f7b6-c5a4-3b21-d0e1-f2c3a4b5d6e7" // Oluşturulan ürünün GUID'si

Ürün Detaylarını Getirme
Endpoint: GET /api/products/{id}

Örnek: GET /api/products/e9d8f7b6-c5a4-3b21-d0e1-f2c3a4b5d6e7

Başarılı Yanıt (200 OK):
{
  "id": "e9d8f7b6-c5a4-3b21-d0e1-f2c3a4b5d6e7",
  "name": "Yeni Ürün Adı",
  "description": "Bu, yeni oluşturulacak ürünün açıklamasıdır.",
  "price": 99.99,
  "currency": "USD",
  "stock": 100
}

Ürün Bulunamadı (404 Not Found)

Test Etme
Proje, in-memory veritabanı (Entity Framework Core InMemory) kullandığından, herhangi bir veritabanı kurulumuna gerek kalmadan doğrudan çalıştırılabilir ve test edilebilir. API endpoint'lerini test etmek için Swagger UI'ı veya Postman gibi bir araç kullanabilirsiniz.

Katkıda Bulunma
Bu proje açık kaynaklıdır ve katkılara açıktır. Her türlü geri bildirim, hata raporu veya özellik isteği memnuniyetle karşılanır. Katkıda bulunmak isterseniz lütfen aşağıdaki adımları izleyin:

Depoyu forklayın.

Yeni bir özellik dalı oluşturun (git checkout -b feature/AmazingFeature).

Değişikliklerinizi yapın ve commit edin (git commit -m 'Add some AmazingFeature').

Dalı orijinal depoya push edin (git push origin feature/AmazingFeature).

Bir Pull Request oluşturun.