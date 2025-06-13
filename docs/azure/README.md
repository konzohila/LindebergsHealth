# ☁️ Azure Integration - LindebergsHealth

**Umfassende Azure Cloud Services Integration für das LindebergsHealth System**

## 📋 Übersicht

LindebergsHealth nutzt verschiedene Azure Services für eine moderne, skalierbare und sichere Cloud-Architektur:

- **🔐 Azure Active Directory** - Authentifizierung und Autorisierung
- **🌐 Azure App Service** - API Hosting
- **📱 Azure Static Web Apps** - Blazor Frontend Hosting
- **🗄️ Azure SQL Database** - Datenspeicherung
- **🔑 Azure Key Vault** - Secrets Management
- **📊 Azure Application Insights** - Monitoring und Telemetrie

## 📚 Dokumentation

### 🔐 [Authentifizierung](authentication.md)
**Azure AD & MSAL Setup für Blazor WebAssembly**

- MSAL Konfiguration
- Redirect vs. Popup Authentifizierung
- Cache-Management
- Bekannte Probleme & Lösungen
- JavaScript Integration

### 🚀 [Azure Setup Guide](azure-setup-guide.md)
**Grundkonfiguration der Azure-Umgebung**

- Resource Group Setup
- App Service Konfiguration
- SQL Database Setup
- Key Vault Einrichtung

### 🤖 [MCP Integration](azure-mcp-setup.md)
**AI-Powered Development mit Azure MCP Server**

- GitHub Copilot Integration
- Azure CLI Automatisierung
- Entwicklungsworkflow-Optimierung
- Monitoring und Debugging

### 📊 [Integration Summary](azure-integration-summary.md)
**Übersicht aller Azure Services und deren Verwendung**

- Service-Übersicht
- Kosten-Optimierung
- Best Practices
- Troubleshooting

## 🏗️ Azure Architektur

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Azure AD      │    │  Static Web App │    │   App Service   │
│                 │    │                 │    │                 │
│ • Tenant        │◄──►│ • Blazor WASM   │◄──►│ • REST API      │
│ • App Reg.      │    │ • MSAL Auth     │    │ • JWT Validation│
│ • Users/Groups  │    │ • CDN           │    │ • Health Checks │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Key Vault     │    │ Application     │    │   SQL Database  │
│                 │    │ Insights        │    │                 │
│ • Secrets       │    │                 │    │ • Entity Data   │
│ • Certificates  │    │ • Telemetry     │    │ • Backups       │
│ • Connection    │    │ • Monitoring    │    │ • Scaling       │
│   Strings       │    │ • Alerts        │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🔧 Konfiguration

### Azure AD App Registrations

**Blazor Client App:**
- **Client ID:** `edf1f825-f1b4-4801-a3c1-2812853b880d`
- **Tenant ID:** `bfda0a4e-8d57-4ec0-b5b6-c5642c54e015`
- **Redirect URIs:** Konfiguriert für lokale und Azure-Umgebung

**API App (Optional):**
- **Client ID:** `ed8c66d4-1b5a-401e-9108-f7281ca84447`
- **Scopes:** `api://ed8c66d4-1b5a-401e-9108-f7281ca84447/access_as_user`

### Umgebungen

**Entwicklung (Lokal):**
- API: `http://localhost:5196`
- BlazorApp: `https://localhost:7186`

**Produktion (Azure):**
- API: `https://lindebergs-api-win.azurewebsites.net`
- BlazorApp: `https://[static-web-app].azurestaticapps.net`

## 🚀 Deployment

### Automatisches Deployment

```bash
# GitHub Actions Pipeline
git push origin main
# → Automatisches Build & Deploy nach Azure
```

### Manuelles Deployment

```bash
# API zu Azure App Service
dotnet publish -c Release
# → Deploy über Azure CLI oder Visual Studio

# Blazor App zu Static Web Apps
dotnet publish -c Release
# → Deploy über GitHub Actions oder Azure CLI
```

## 🔍 Monitoring

### Application Insights

- **Performance Monitoring** - Response Times, Throughput
- **Error Tracking** - Exceptions, Failed Requests
- **User Analytics** - Page Views, User Flows
- **Custom Telemetry** - Business Metrics

### Health Checks

```csharp
// API Health Endpoint
GET /health
// → Status aller Services (DB, External APIs, etc.)
```

## 🔒 Sicherheit

### Best Practices

1. **Secrets Management** - Alle Secrets in Azure Key Vault
2. **HTTPS Everywhere** - Verschlüsselte Kommunikation
3. **Least Privilege** - Minimale Berechtigungen
4. **Regular Updates** - Aktuelle Frameworks und Packages
5. **Monitoring** - Kontinuierliche Überwachung

### Compliance

- **GDPR** - Datenschutz-konforme Datenverarbeitung
- **HIPAA** - Gesundheitsdaten-Sicherheit (falls erforderlich)
- **Azure Security Center** - Kontinuierliche Sicherheitsbewertung

## 💰 Kostenoptimierung

### Entwicklung
- **Free Tier** Services nutzen
- **Development SKUs** für nicht-produktive Umgebungen
- **Auto-Shutdown** für Entwicklungs-VMs

### Produktion
- **Reserved Instances** für vorhersagbare Workloads
- **Auto-Scaling** für variable Lasten
- **Cost Alerts** für Budget-Überwachung

## 🛠️ Tools & CLI

### Azure CLI
```bash
# Installation
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash

# Login
az login

# Resource Group erstellen
az group create --name LindebergsRG --location "West Europe"
```

### Azure PowerShell
```powershell
# Installation
Install-Module -Name Az -AllowClobber

# Login
Connect-AzAccount

# Resources auflisten
Get-AzResource -ResourceGroupName "LindebergsRG"
```

## 📞 Support

### Azure Support
- **Azure Portal** - Support Tickets
- **Azure Documentation** - Offizielle Dokumentation
- **Azure Community** - Stack Overflow, Reddit

### Projekt Support
- **GitHub Issues** - Bug Reports und Feature Requests
- **GitHub Discussions** - Allgemeine Fragen
- **Team Chat** - Interne Kommunikation 