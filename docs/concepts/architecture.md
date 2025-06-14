# 🏗️ LindebergsHealth - Systemarchitektur

## Clean Architecture Übersicht

Diese Anwendung folgt einer **sauberen 3-Schichten-Architektur** mit klarer Trennung der Verantwortlichkeiten:

```
LindebergsHealth/
├── src/
│   ├── Core/                             # 💼 Geschäftslogik (Domain + Application)
│   │   ├── LindebergsHealth.Domain/      # Entitäten, Value Objects, Geschäftsregeln
│   │   └── LindebergsHealth.Application/ # Use Cases, Commands, Queries, Services
│   │
│   ├── Infrastructure/                   # 💾 Datenzugriff & externe Services
│   │   └── LindebergsHealth.Infrastructure/ # EF DbContext, Repositories, externe APIs
│   │
│   └── Presentation/                     # 🖥️ Benutzeroberfläche
│       ├── LindebergsHealth.Api/         # ASP.NET Core Web API
│       └── LindebergsHealth.BlazorApp/   # Blazor WebAssembly Frontend
│
└── tests/                               # 🧪 Tests (entsprechend strukturiert)
    ├── Core/
    ├── Infrastructure/
    └── Presentation/
```

## 🔄 Abhängigkeitsrichtung

```
Presentation → Core ← Infrastructure
```

- **Core** (Domain + Application) kennt nur sich selbst (keine externen Abhängigkeiten)
- **Infrastructure** implementiert Core-Interfaces  
- **Presentation** nutzt Core und koordiniert Infrastructure über Dependency Injection

## 🎯 Schicht-Verantwortlichkeiten

### 💼 Core Layer

**Domain (LindebergsHealth.Domain):**
- Patient, Arzt, Termin Entitäten
- Geschäftsregeln (z.B. `Patient.CanBookAppointment()`)
- Value Objects (z.B. `PatientId`, `Email`)
- Domain Services für komplexe Geschäftslogik
- Domain Events

**Application (LindebergsHealth.Application):**
- Use Cases (z.B. `BookAppointmentCommand`)
- Command/Query Handler (CQRS Pattern)
- Service Interfaces (z.B. `IPatientRepository`)
- DTOs für interne Kommunikation
- Application Services

### 💾 Infrastructure Layer

**LindebergsHealth.Infrastructure:**
- Entity Framework DbContext
- Repository Implementierungen
- Datenbank-Konfigurationen
- Externe Service-Integrationen (Email, SMS, etc.)
- Caching-Implementierungen
- File Storage Services

### 🖥️ Presentation Layer

**API (LindebergsHealth.Api):**
- REST API Controllers
- Authentication & Authorization
- API DTOs/Models
- Swagger/OpenAPI Dokumentation
- Middleware
- Health Checks

**BlazorApp (LindebergsHealth.BlazorApp):**
- Blazor WebAssembly Components
- Pages und Layouts
- Client-seitige Services
- UI Models/ViewModels
- Azure AD Integration

## 🔧 Design Patterns

### CQRS (Command Query Responsibility Segregation)
- **Commands** - Ändern den Zustand (Create, Update, Delete)
- **Queries** - Lesen Daten (Read-only)
- Getrennte Handler für bessere Skalierbarkeit

### Repository Pattern
- Abstraktion der Datenzugriffsschicht
- Testbarkeit durch Interface-basierte Implementierung
- Einheitliche API für verschiedene Datenquellen

### Dependency Injection
- Lose Kopplung zwischen Schichten
- Einfache Testbarkeit
- Konfiguration in `Program.cs`

### Domain-Driven Design (DDD)
- Fachliche Modellierung im Zentrum
- Ubiquitous Language
- Bounded Contexts

## 📊 Datenfluss

```
1. HTTP Request → API Controller
2. Controller → Application Service/Handler
3. Handler → Domain Service (Geschäftslogik)
4. Domain Service → Repository Interface
5. Repository → Database/External Service
6. Response ← zurück durch alle Schichten
```

## 🔒 Sicherheitsarchitektur

### Authentifizierung
- **Azure AD** mit OAuth 2.0/OpenID Connect
- **MSAL** für Token-Management
- **JWT Bearer Tokens** für API-Zugriff

### Autorisierung
- **Role-based Access Control (RBAC)**
- **Policy-based Authorization**
- **Resource-based Authorization** für feinere Kontrolle

### Datenschutz
- **HTTPS** für alle Kommunikation
- **Data Protection APIs** für sensible Daten
- **GDPR-konforme** Datenverarbeitung

## 🚀 Deployment-Architektur

### Azure Services
- **Azure App Service** - API Hosting
- **Azure Static Web Apps** - Blazor Frontend
- **Azure SQL Database** - Datenspeicherung
- **Azure Key Vault** - Secrets Management
- **Azure Application Insights** - Monitoring

### CI/CD Pipeline
- **GitHub Actions** für automatische Builds
- **Infrastructure as Code** mit ARM Templates
- **Blue-Green Deployment** für Zero-Downtime

## 📈 Skalierbarkeit

### Horizontal Scaling
- **Stateless API Design**
- **Load Balancer** für mehrere Instanzen
- **Database Connection Pooling**

### Caching Strategy
- **In-Memory Caching** für häufige Abfragen
- **Distributed Caching** mit Redis
- **CDN** für statische Assets

### Performance Optimierung
- **Async/Await** für I/O-Operationen
- **Entity Framework Optimierungen**
- **Response Compression**

## 🧪 Testing-Strategie

Alle fachlich und technisch relevanten Kernfunktionen sind durch automatisierte Unit-Tests (NUnit) abgedeckt. Die Teststrategie ist im [Testkonzept](../guides/testing.md) dokumentiert. Für Details zu Coding-Standards und Testphilosophie siehe [Development Guidelines](../guides/development.md).

- Tests werden bei jedem Commit/PR automatisch ausgeführt (CI/CD, GitHub Actions).
- Coverage-Philosophie: Qualität vor Quantität – getestet wird, wo Logik oder Fehleranfälligkeit besteht.
- Keine Tests für reine Daten- oder Mapping-Entities ohne Logik.

### Unit Tests
- **Domain Logic** – Geschäftsregeln testen
- **Application Services** – Use Case Logik
- **Mocking** für externe Abhängigkeiten

### Integration Tests
- **API Endpoints** – End-to-End Tests
- **Database Integration** – Repository Tests
- **External Services** – Service Integration

### E2E Tests
- **Blazor Components** – UI Tests
- **User Workflows** – Komplette Szenarien
- **Cross-Browser Testing**

## 📚 Weiterführende Dokumentation

- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://martinfowler.com/bliki/DomainDrivenDesign.html)
- [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design) 