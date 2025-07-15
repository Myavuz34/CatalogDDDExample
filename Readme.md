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

```mermaid
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