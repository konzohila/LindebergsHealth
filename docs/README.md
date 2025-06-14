# 📚 LindebergsHealth - Dokumentation

**Umfassende Dokumentation für das LindebergsHealth Gesundheitsmanagementsystem**

## 📁 Dokumentationsstruktur

### 🏗️ [Architecture](architecture/)
*Systemarchitektur und Design-Entscheidungen*

- **Clean Architecture** - 3-Schichten-Modell
- **Domain-Driven Design** - Fachliche Modellierung
- **CQRS & Event Sourcing** - Command/Query Separation

### ☁️ [Azure](azure/)
*Cloud-Integration und Azure-Services*

- **[Authentifizierung](azure/authentication.md)** - Azure AD & MSAL Setup
- **[Azure Setup Guide](azure/azure-setup-guide.md)** - Grundkonfiguration
- **[MCP Integration](azure/azure-mcp-setup.md)** - AI-Powered Development
- **[Integration Summary](azure/azure-integration-summary.md)** - Übersicht aller Services

### 👨‍💻 [Development](development/)
*Entwicklungsrichtlinien und Workflows*

- **[Development Guidelines](development/development-guidelines.md)** - Coding Standards
- **[Branching Strategy](development/branching-strategy.md)** - Git Workflow

## 🚀 Quick Start

### Lokale Entwicklung

```bash
# Repository klonen
git clone https://github.com/username/LindebergsHealth.git
cd LindebergsHealth

# Dependencies installieren
dotnet restore

# API starten (Terminal 1)
dotnet run --project src/Presentation/LindebergsHealth.Api

# BlazorApp starten (Terminal 2)
dotnet run --project src/Presentation/LindebergsHealth.BlazorApp
```

### URLs

- **API:** http://localhost:5196
- **BlazorApp:** https://localhost:7186 / http://localhost:5104
- **Swagger:** http://localhost:5196/swagger

## 🏗️ Projektarchitektur

```
LindebergsHealth/
├── src/
│   ├── Core/
│   │   ├── LindebergsHealth.Domain/      # Geschäftslogik & Entitäten
│   │   └── LindebergsHealth.Application/ # Use Cases & Services
│   ├── Infrastructure/
│   │   └── LindebergsHealth.Infrastructure/ # Datenbank & externe Services
│   └── Presentation/
│       ├── LindebergsHealth.Api/         # REST API
│       └── LindebergsHealth.BlazorApp/   # Web UI (Azure AD Auth)
├── tests/                                # Unit & Integration Tests
├── docs/                                 # Diese Dokumentation
└── scripts/                              # Build & Deployment Skripte
```

## 🔐 Authentifizierung

Das System verwendet **Azure Active Directory (Azure AD)** mit **MSAL** für sichere Authentifizierung.

**📖 Detaillierte Dokumentation:** [azure/authentication.md](azure/authentication.md)

**Quick Start:**
```bash
# BlazorApp mit Authentifizierung starten
dotnet run --project src/Presentation/LindebergsHealth.BlazorApp

# URLs:
# https://localhost:7186 (HTTPS)
# http://localhost:5104 (HTTP)
```

## 🤖 AI-Powered Development

**Azure MCP Server** ist integriert! GitHub Copilot kann direkt mit Azure interagieren:

```
💬 "Show Web App status for LindebergsHealth"
💬 "List all resources in LindebergsRG" 
💬 "Check deployment logs for latest release"
```

**📖 Setup Guide:** [azure/azure-mcp-setup.md](azure/azure-mcp-setup.md)

## 📋 Nächste Schritte

1. ✅ **Projektstruktur** - Erstellt!
2. ✅ **Authentication** - Azure AD mit MSAL konfiguriert!
3. ✅ **Dokumentation** - Strukturiert und umfassend!
4. 🔲 **Domain Models** aus altem Projekt übernehmen
5. 🔲 **Entity Framework** konfigurieren
6. 🔲 **Repository Pattern** implementieren
7. 🔲 **API Controllers** erstellen
8. 🔲 **Blazor UI** entwickeln

## 🛠️ Entwicklungstools

- **.NET 8** - Framework
- **Blazor WebAssembly** - Frontend
- **Entity Framework Core** - ORM
- **Azure AD** - Authentifizierung
- **Swagger/OpenAPI** - API Dokumentation
- **xUnit** - Testing Framework

## 📞 Support

Bei Fragen zur Dokumentation oder zum Projekt:

1. **Issues** - GitHub Issues für Bugs und Feature Requests
2. **Discussions** - GitHub Discussions für allgemeine Fragen
3. **Wiki** - Zusätzliche Dokumentation im GitHub Wiki 