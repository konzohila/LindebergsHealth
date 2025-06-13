# 👨‍💻 Development - LindebergsHealth

**Entwicklungsrichtlinien und Workflows für das LindebergsHealth Projekt**

## 📋 Übersicht

Diese Sektion enthält alle wichtigen Informationen für Entwickler, die am LindebergsHealth Projekt arbeiten:

- **📏 Coding Standards** - Einheitliche Code-Qualität
- **🌿 Git Workflow** - Branching Strategy und Commit Guidelines
- **🧪 Testing Guidelines** - Unit, Integration und E2E Tests
- **🔄 CI/CD Pipeline** - Automatisierte Builds und Deployments
- **📊 Code Review Process** - Qualitätssicherung

## 📚 Dokumentation

### 📏 [Development Guidelines](development-guidelines.md)
**Coding Standards und Best Practices**

- C# Coding Conventions
- Blazor Component Guidelines
- API Design Principles
- Error Handling Standards
- Performance Best Practices
- Security Guidelines

### 🌿 [Branching Strategy](branching-strategy.md)
**Git Workflow und Branch Management**

- GitFlow Workflow
- Branch Naming Conventions
- Commit Message Standards
- Pull Request Process
- Release Management
- Hotfix Procedures

## 🛠️ Entwicklungsumgebung

### Voraussetzungen

```bash
# .NET 8 SDK
dotnet --version
# → 8.0.x oder höher

# Node.js (für Blazor WASM)
node --version
# → 18.x oder höher

# Git
git --version
# → 2.x oder höher
```

### IDE Setup

**Visual Studio 2022:**
- Workload: ASP.NET and web development
- Workload: Azure development
- Extension: GitHub Copilot
- Extension: SonarLint

**Visual Studio Code:**
- Extension: C# Dev Kit
- Extension: Azure Tools
- Extension: GitLens
- Extension: GitHub Copilot

### Lokale Entwicklung

```bash
# Repository klonen
git clone https://github.com/username/LindebergsHealth.git
cd LindebergsHealth

# Dependencies installieren
dotnet restore

# Entwicklungsumgebung starten
./scripts/dev-start.sh

# Oder manuell:
# Terminal 1: API
dotnet run --project src/Presentation/LindebergsHealth.Api

# Terminal 2: BlazorApp
dotnet run --project src/Presentation/LindebergsHealth.BlazorApp
```

## 🧪 Testing

### Test-Struktur

```
tests/
├── Core/
│   ├── LindebergsHealth.Domain.Tests/     # Unit Tests - Domain Logic
│   └── LindebergsHealth.Application.Tests/ # Unit Tests - Use Cases
├── Infrastructure/
│   └── LindebergsHealth.Infrastructure.Tests/ # Integration Tests
└── Presentation/
    ├── LindebergsHealth.Api.Tests/        # API Integration Tests
    └── LindebergsHealth.BlazorApp.Tests/  # UI Component Tests
```

### Test Commands

```bash
# Alle Tests ausführen
dotnet test

# Mit Coverage Report
dotnet test --collect:"XPlat Code Coverage"

# Nur Unit Tests
dotnet test --filter Category=Unit

# Nur Integration Tests
dotnet test --filter Category=Integration
```

### Test Guidelines

1. **AAA Pattern** - Arrange, Act, Assert
2. **Descriptive Names** - Test method names beschreiben das Verhalten
3. **One Assertion** - Ein Test, eine Assertion
4. **Mock External Dependencies** - Isolierte Unit Tests
5. **Test Data Builders** - Konsistente Test-Daten

## 🔄 CI/CD Pipeline

### GitHub Actions Workflow

```yaml
# .github/workflows/ci.yml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 8.0.x
      - name: Test
        run: dotnet test --configuration Release
```

### Pipeline Stages

1. **Build** - Code kompilieren
2. **Test** - Unit und Integration Tests
3. **Code Quality** - SonarQube Analysis
4. **Security Scan** - Dependency Check
5. **Deploy** - Azure App Service (nur main branch)

## 📊 Code Quality

### Static Code Analysis

```bash
# SonarQube Scanner
dotnet sonarscanner begin /k:"LindebergsHealth"
dotnet build
dotnet test --collect:"XPlat Code Coverage"
dotnet sonarscanner end
```

### Code Metrics

- **Code Coverage** - Minimum 80%
- **Cyclomatic Complexity** - Maximum 10 pro Methode
- **Maintainability Index** - Minimum 70
- **Technical Debt** - Maximum 5% der Entwicklungszeit

### Linting Rules

```xml
<!-- .editorconfig -->
[*.cs]
dotnet_analyzer_diagnostic.category-style.severity = warning
dotnet_analyzer_diagnostic.category-maintainability.severity = warning
```

## 🔍 Debugging

### Lokales Debugging

```bash
# API mit Debug-Modus
dotnet run --project src/Presentation/LindebergsHealth.Api --configuration Debug

# Blazor mit Hot Reload
dotnet watch run --project src/Presentation/LindebergsHealth.BlazorApp
```

### Azure Debugging

```bash
# Remote Debugging (App Service)
az webapp log tail --name lindebergs-api-win --resource-group LindebergsRG

# Application Insights Logs
az monitor app-insights query --app LindebergsHealth-AI --analytics-query "traces | limit 100"
```

## 📝 Documentation

### Code Documentation

```csharp
/// <summary>
/// Erstellt einen neuen Patienten im System
/// </summary>
/// <param name="command">Patient-Erstellungskommando</param>
/// <returns>Erstellter Patient mit generierter ID</returns>
/// <exception cref="ValidationException">Wenn Patientendaten ungültig sind</exception>
public async Task<Patient> CreatePatientAsync(CreatePatientCommand command)
{
    // Implementation...
}
```

### API Documentation

- **Swagger/OpenAPI** - Automatisch generiert
- **XML Comments** - Für alle öffentlichen APIs
- **Example Requests/Responses** - In Controller-Attributen

## 🚀 Performance

### Profiling Tools

```bash
# dotnet-trace für Performance-Analyse
dotnet tool install --global dotnet-trace
dotnet trace collect --process-id <PID>

# dotnet-counters für Metriken
dotnet tool install --global dotnet-counters
dotnet counters monitor --process-id <PID>
```

### Performance Guidelines

1. **Async/Await** - Für alle I/O-Operationen
2. **Connection Pooling** - Datenbankverbindungen
3. **Caching** - Für häufige Abfragen
4. **Lazy Loading** - Nur bei Bedarf laden
5. **Response Compression** - Für API-Responses

## 📞 Support

### Entwickler-Support

- **GitHub Issues** - Bug Reports und Feature Requests
- **GitHub Discussions** - Technische Fragen
- **Team Chat** - Schnelle Hilfe
- **Code Reviews** - Peer-to-Peer Learning

### Externe Ressourcen

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/)
- [Azure Documentation](https://docs.microsoft.com/en-us/azure/)
- [Clean Architecture Guide](https://docs.microsoft.com/en-us/dotnet/architecture/) 