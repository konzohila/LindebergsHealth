# Azure Setup Anleitung

## Voraussetzungen
- Azure-Konto mit aktiver Subscription
- Azure CLI installiert
- LindebergsHealth Repository geklont

## 1. Azure AD App Registration

### App-Details
Die folgenden Informationen werden für die Azure AD App Registration benötigt:
- **App Name**: LindebergsHealth-App
- **Supported Account Types**: Single tenant
- **Redirect URIs**: Siehe Konfigurationstabelle unten

### Konfiguration
| Environment | Redirect URI |
|-------------|--------------|
| Development | `https://localhost:7001/signin-oidc` |
| Staging | `https://lindebergs-api-staging.azurewebsites.net/signin-oidc` |
| Production | `https://lindebergs-api-win.azurewebsites.net/signin-oidc` |

## 2. Azure Web Apps

### Staging-Umgebung
- **API**: `lindebergs-api-staging.azurewebsites.net`
- **Web**: `lindebergs-web-staging.azurewebsites.net`
- **Runtime**: .NET 8.0 (LTS)

### Produktions-Umgebung  
- **API**: `lindebergs-api-win.azurewebsites.net`
- **Runtime**: .NET 8.0 (LTS)

## 3. GitHub Secrets

Die folgenden Secrets müssen im GitHub Repository konfiguriert werden:

### Erforderliche Secrets
```
AZURE_CLIENT_SECRET
AZURE_WEBAPP_PUBLISH_PROFILE_API_STAGING
AZURE_WEBAPP_PUBLISH_PROFILE_WEB_STAGING
```

### Beschaffung der Werte
1. **Client Secret**: Azure Portal → App Registrations → LindebergsHealth-App → Certificates & secrets
2. **Publish Profiles**: Azure Portal → Web Apps → Download publish profile

## 4. Deployment

### Automatische Deployments
- **Staging**: Triggered bei Push auf `development` branch
- **Production**: Triggered bei Push auf `main` branch

### Manuelles Deployment
```bash
# Staging
az webapp deploy --resource-group LindebergsRG --name lindebergs-api-staging --src-path ./publish

# Production  
az webapp deploy --resource-group LindebergsRG --name lindebergs-api-win --src-path ./publish
```

## 5. Testen

### Health Check
```bash
# Staging
curl https://lindebergs-api-staging.azurewebsites.net/health

# Production
curl https://lindebergs-api-win.azurewebsites.net/health
```

### Authentication
1. Navigiere zu `/swagger` Endpoint
2. Klicke "Authorize" Button
3. Login mit Azure AD Credentials
4. Teste protected Endpoints 