# Feature-Workflow in Clean Architecture (LindebergsHealth)

**Stand: 2025-06-16**

Dieses Dokument beschreibt den empfohlenen Workflow zur Implementierung neuer Features in der bestehenden Clean Architecture/CQRS-Struktur von LindebergsHealth. Es basiert auf den Development Guidelines und dem Testkonzept.

---

## 1. Domain Layer
- **Entity/ValueObject anlegen oder erweitern**
  - Pfad: `src/LindebergsHealth.Domain/Entities/`
  - Beispiel: `public class NeueEntity { ... }`
- **Repository-Interface anpassen (wenn nötig)**
  - Pfad: `src/LindebergsHealth.Domain/Feature/IFeatureRepository.cs`
  - Beispiel: `Task<NeueEntity> GetByIdAsync(Guid id);`

---

## 2. Infrastructure Layer
- **Repository-Implementierung anpassen/erstellen**
  - Pfad: `src/LindebergsHealth.Infrastructure/Repositories/FeatureRepository.cs`
  - Implementiert das Interface aus dem Domain-Layer.
- **Dependency Injection registrieren**
  - Pfad: `src/LindebergsHealth.Infrastructure/DependencyInjection.cs`
  - Beispiel: `services.AddScoped<IFeatureRepository, FeatureRepository>();`

---

## 3. Application Layer (CQRS/MediatR)
- **Command/Query-DTOs anlegen**
  - Pfad: `src/LindebergsHealth.Application/Feature/Commands/` oder `/Queries/`
  - Beispiel: `public record CreateFeatureCommand(...) : IRequest<FeatureDto>;`
- **Handler für Command/Query anlegen**
  - Pfad: `src/LindebergsHealth.Application/Feature/Commands/CreateFeatureHandler.cs`
  - Implementiert `IRequestHandler<,>`
- **Mapping/Validierung (optional)**
  - Mapping zwischen Domain und DTOs, ggf. FluentValidation.

---

## 4. API Layer
- **Controller/Endpoint anlegen oder erweitern**
  - Pfad: `src/Presentation/LindebergsHealth.Api/Controllers/FeatureController.cs`
  - Methoden: POST/GET/PUT/DELETE (je nach Use Case)
  - Ruft Application-Layer via MediatR auf.

---

## 5. Blazor Client (UI)
- **ViewModel/DTOs anlegen**
  - Pfad: `src/Presentation/LindebergsHealth.BlazorApp/ViewModels/`
- **Razor Page/Komponente anlegen oder erweitern**
  - Pfad: `src/Presentation/LindebergsHealth.BlazorApp/Pages/Feature.razor`
  - Bindet an API via `HttpClient`
- **UI-Logik und Fehlerbehandlung**
  - Nutzerfeedback, Ladeanzeigen, Validierung, etc.

---

## 6. Tests
- **Unit-Tests für Domain und Application Layer**
  - Pfad: `tests/LindebergsHealth.Domain.Tests/`, `tests/LindebergsHealth.Application.Tests/`
- **Integrationstests für API/Repository**
  - Pfad: `tests/LindebergsHealth.Infrastructure.Tests/`, `tests/Presentation/LindebergsHealth.Api.Tests/`
- **E2E-Tests für UI**
  - Pfad: `tests/LindebergsHealth.UiTests/`
- **Tests automatisiert in CI/CD einbinden**

---

## 7. Dokumentation & Commit
- **Feature und API dokumentieren**
  - guides/feature-name.md, OpenAPI/Swagger aktualisieren
- **Commit mit sprechender Message**
  - Beispiel:  
    `git commit -am "Feature: NeueEntity – CQRS, API, UI und Tests für ... implementiert"`

---

## Zusammengefasst
> Nach dem initialen Setup musst du für jedes neue Feature nur noch diese Schritte abarbeiten. Die Struktur, Patterns und Teststrategie bleiben gleich – du kannst dich auf die Business-Logik konzentrieren!

**Tipp:** Lege dir für wiederkehrende Muster (z.B. Command/Handler, Repository) eigene Snippets oder Templates an – das beschleunigt die Entwicklung weiter und sorgt für Konsistenz.

---

**Dieses Dokument ist Teil der offiziellen Entwicklungsdokumentation und orientiert sich an guides/development.md sowie dem Testkonzept.**
