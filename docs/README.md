# LindebergsHealth - Clean Architecture

## 🏗️ Architektur

Diese Anwendung folgt einer **sauberen 3-Schichten-Architektur** mit klarer Trennung der Verantwortlichkeiten:

```
LindebergsHealth/
├── src/
│   ├── Business/                        # 💼 Geschäftslogik
│   │   ├── LindebergsHealth.Domain/     # Entitäten, Value Objects, Geschäftsregeln
│   │   └── LindebergsHealth.Application/ # Use Cases, Commands, Queries, Services
│   │
│   ├── DataAccess/                      # 💾 Datenzugriff
│   │   └── LindebergsHealth.DataAccess/ # EF DbContext, Repositories, externe APIs
│   │
│   └── Presentation/                    # 🖥️ Benutzeroberfläche
│       ├── LindebergsHealth.Api/        # ASP.NET Core Web API
│       └── LindebergsHealth.Web/        # Blazor WebAssembly Frontend
│
└── tests/                              # 🧪 Tests (entsprechend strukturiert)
    ├── Business/
    ├── DataAccess/
    └── Presentation/
```

## 🔄 Abhängigkeiten

```
Presentation → Business → DataAccess
```

- **Business** kennt nur sich selbst (keine externen Abhängigkeiten)
- **DataAccess** implementiert Business-Interfaces  
- **Presentation** nutzt Business und koordiniert DataAccess

## 🎯 Schicht-Verantwortlichkeiten

### 💼 Business Layer

**Domain (LindebergsHealth.Domain):**
- Patient, Arzt, Termin Entitäten
- Geschäftsregeln (z.B. `Patient.CanBookAppointment()`)
- Value Objects (z.B. `PatientId`, `Email`)
- Domain Services für komplexe Geschäftslogik

**Application (LindebergsHealth.Application):**
- Use Cases (z.B. `BookAppointmentCommand`)
- Command/Query Handler
- Service Interfaces (z.B. `IPatientRepository`)
- DTOs für interne Kommunikation

### 💾 DataAccess Layer

**LindebergsHealth.DataAccess:**
- Entity Framework DbContext
- Repository Implementierungen
- Datenbank-Konfigurationen
- Externe Service-Integrationen (Email, etc.)

### 🖥️ Presentation Layer

**API (LindebergsHealth.Api):**
- REST API Controllers
- Authentication & Authorization
- API DTOs
- Swagger/OpenAPI Dokumentation

**Web (LindebergsHealth.Web):**
- Blazor WebAssembly Components
- Pages und Layouts
- Client-seitige Services
- UI Models/ViewModels

## 🚀 Erste Schritte

```bash
# Solution bauen
dotnet build

# API starten
dotnet run --project src/Presentation/LindebergsHealth.Api

# Web starten (in anderem Terminal)
dotnet run --project src/Presentation/LindebergsHealth.Web
```

## 📋 Nächste Schritte

1. ✅ **Projektstruktur** - Erstellt!
2. 🔲 **Domain Models** aus altem Projekt übernehmen
3. 🔲 **Entity Framework** konfigurieren
4. 🔲 **Repository Pattern** implementieren
5. 🔲 **API Controllers** erstellen
6. 🔲 **Blazor UI** entwickeln
7. 🔲 **Authentication** einrichten 