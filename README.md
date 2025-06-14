# LindebergsHealth

**Gesundheitsmanagementsystem mit Clean Architecture**

## 🚀 Quick Start

```bash
# Azure Health Check (NEW! ✨)
./scripts/azure-health-check.sh

# Solution bauen
dotnet build

# API starten
dotnet run --project src/Presentation/LindebergsHealth.Api

# Web UI starten
dotnet run --project src/Presentation/LindebergsHealth.BlazorApp
```

### 🤖 **AI-Powered Development** ✨

**Azure MCP Server** ist integriert! GitHub Copilot kann direkt mit Azure interagieren:

```
💬 "Show Web App status for LindebergsHealth"
💬 "List all resources in LindebergsRG" 
💬 "Check deployment logs for latest release"
```

**Production App:** [lindebergs-api-win.azurewebsites.net](https://lindebergs-api-win.azurewebsites.net)

## 📁 Projektstruktur

```
├── src/           # Source Code (3-Schichten-Architektur)
├── tests/         # Unit & Integration Tests
├── docs/          # Dokumentation & Architektur
├── build/         # CI/CD Konfigurationen
└── scripts/       # Entwicklungs- und Deployment-Skripte
```

## 📖 Dokumentation

- **[Projektdokumentation](docs/README.md)** - Detaillierte Architektur und Setup
- **[Authentifizierung](docs/azure/authentication.md)** - Azure AD & MSAL Konfiguration
- **[Development Guidelines](docs/development/development-guidelines.md)** - Coding Standards
- **[Branching Strategy](docs/development/branching-strategy.md)** - Git Workflow 