# 🔐 Authentifizierung - LindebergsHealth.BlazorApp

## Übersicht

Das **LindebergsHealth.BlazorApp** verwendet **Azure Active Directory (Azure AD)** für die Authentifizierung mit **Microsoft Authentication Library (MSAL)** in einem **Blazor WebAssembly**-Projekt.

## 🏗️ Architektur

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Blazor WASM   │    │   Azure AD      │    │   Backend API   │
│   Client        │◄──►│   Tenant        │◄──►│   (Optional)    │
│                 │    │                 │    │                 │
│ • MSAL.js       │    │ • OAuth 2.0     │    │ • JWT Validation│
│ • Redirect Flow │    │ • OpenID Connect│    │ • Scopes        │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## ⚙️ Konfiguration

### Azure AD App Registration

**Tenant ID:** `bfda0a4e-8d57-4ec0-b5b6-c5642c54e015`  
**Client ID:** `edf1f825-f1b4-4801-a3c1-2812853b880d`  
**Authority:** `https://login.microsoftonline.com/bfda0a4e-8d57-4ec0-b5b6-c5642c54e015`

### appsettings.json

```json
{
  "AzureAd": {
    "Authority": "https://login.microsoftonline.com/bfda0a4e-8d57-4ec0-b5b6-c5642c54e015",
    "ClientId": "edf1f825-f1b4-4801-a3c1-2812853b880d",
    "ValidateAuthority": true,
    "PostLogoutRedirectUri": "/",
    "RedirectUri": "/authentication/login-callback",
    "ResponseType": "code"
  }
}
```

### Program.cs Konfiguration

```csharp
builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    
    // Cache-Konfiguration für besseres Logout-Verhalten
    options.ProviderOptions.Cache.CacheLocation = "localStorage";
    options.ProviderOptions.Cache.StoreAuthStateInCookie = false;
    
    // WICHTIG: Redirect-Modus statt Popup verwenden
    options.ProviderOptions.LoginMode = "redirect";
    options.ProviderOptions.Authentication.NavigateToLoginRequestUrl = false;
    
    // Standard-Scopes
    options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
    options.ProviderOptions.DefaultAccessTokenScopes.Add("profile");
    
    // Post-Logout Redirect URI
    options.ProviderOptions.Authentication.PostLogoutRedirectUri = builder.HostEnvironment.BaseAddress;
    
    // Popup-Probleme vermeiden
    options.ProviderOptions.Authentication.RedirectUri = builder.HostEnvironment.BaseAddress + "authentication/login-callback";
});
```

## 🔧 Implementierung

### 1. Authentication.razor (Benutzerdefinierte Auth-Seite)

```razor
@page "/authentication/{action}"
@using Microsoft.AspNetCore.Components.WebAssembly.Authentication

<RemoteAuthenticatorView Action="@Action">
    <LogInFailed>
        <div class="alert alert-danger">
            <h4>Anmeldung fehlgeschlagen</h4>
            <p>Mögliche Ursachen:</p>
            <ul>
                <li>Popup wurde blockiert</li>
                <li>Browser-Cache-Probleme</li>
                <li>Sitzung abgelaufen</li>
            </ul>
            <button class="btn btn-primary" @onclick="HandleLoginError">
                Cache leeren und erneut versuchen
            </button>
        </div>
    </LogInFailed>
</RemoteAuthenticatorView>
```

### 2. LoginDisplay.razor (Login/Logout UI)

```razor
@using Microsoft.AspNetCore.Components.Authorization
@using Microsoft.AspNetCore.Components.WebAssembly.Authentication

<AuthorizeView>
    <Authorized>
        <span class="nav-link">Hallo, @context.User.Identity?.Name!</span>
        <button class="nav-link btn btn-link" @onclick="BeginLogOut">Abmelden</button>
    </Authorized>
    <NotAuthorized>
        <a href="authentication/login" class="nav-link">Anmelden</a>
    </NotAuthorized>
</AuthorizeView>

@code {
    private async Task BeginLogOut()
    {
        // Cache leeren vor Logout
        await JSRuntime.InvokeAsync<bool>("authHelper.clearMsalCache");
        
        // Moderne MSAL Logout-Methode
        Navigation.NavigateToLogout("authentication/logout");
    }
}
```

### 3. JavaScript Cache-Management (auth-helper.js)

```javascript
window.authHelper = {
    clearMsalCache: function() {
        try {
            // MSAL-spezifische Cache-Keys löschen
            const keys = Object.keys(localStorage);
            keys.forEach(key => {
                if (key.startsWith('msal.') || 
                    key.includes('azure') ||
                    key.includes('login.microsoftonline.com')) {
                    localStorage.removeItem(key);
                }
            });
            
            // Session Storage auch leeren
            const sessionKeys = Object.keys(sessionStorage);
            sessionKeys.forEach(key => {
                if (key.startsWith('msal.') || 
                    key.includes('azure')) {
                    sessionStorage.removeItem(key);
                }
            });
            
            return true;
        } catch (error) {
            console.error('Fehler beim Cache leeren:', error);
            return false;
        }
    }
};
```

## 🚨 Bekannte Probleme & Lösungen

### Problem 1: "Request was blocked inside a popup"

**Ursache:** MSAL versucht Popup-Authentifizierung zu verwenden  
**Lösung:** Redirect-Modus konfiguriert in `Program.cs`

```csharp
options.ProviderOptions.LoginMode = "redirect";
options.ProviderOptions.Authentication.NavigateToLoginRequestUrl = false;
```

### Problem 2: Logout/Login-Zyklus hängt

**Ursache:** Browser-Cache behält alte MSAL-Tokens  
**Lösung:** JavaScript-Hilfsfunktionen für Cache-Management

### Problem 3: JSON Serialization Error

**Ursache:** JavaScript-Funktionen geben `null`/`undefined` statt Boolean zurück  
**Lösung:** Explizite `Boolean()` Konvertierung in JavaScript

## 🔒 Sicherheitsaspekte

### 1. Token-Speicherung
- **localStorage** für bessere Performance
- **Keine Cookies** für Token-Speicherung
- Automatische Token-Erneuerung durch MSAL

### 2. Redirect URIs
- **Login:** `/authentication/login-callback`
- **Logout:** `/` (Startseite)
- Konfiguriert in Azure AD App Registration

### 3. Scopes
- **openid:** Basis-Authentifizierung
- **profile:** Benutzerprofil-Informationen
- Erweiterbar für API-Zugriff

## 🧪 Testing

### Lokale Entwicklung

```bash
# BlazorApp starten
cd src/Presentation/LindebergsHealth.BlazorApp
dotnet run

# URLs:
# https://localhost:7186
# http://localhost:5104
```

### Test-Szenarien

1. **Erfolgreiche Anmeldung**
   - Weiterleitung zu Azure AD
   - Rückkehr zur Anwendung
   - Benutzerinformationen anzeigen

2. **Logout-Test**
   - Cache wird geleert
   - Weiterleitung zu Azure AD Logout
   - Rückkehr zur Startseite

3. **Cache-Problem-Test**
   - Nach Logout erneut anmelden
   - Sollte nicht "hängen bleiben"

## 📚 Weiterführende Dokumentation

- [Microsoft MSAL.js Dokumentation](https://docs.microsoft.com/en-us/azure/active-directory/develop/msal-js-initializing-client-applications)
- [Blazor WebAssembly Authentication](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/)
- [Azure AD App Registration](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)

## 🔄 Nächste Schritte

1. **API-Integration:** Backend-API mit JWT-Validation
2. **Rollen-Management:** Azure AD Gruppen/Rollen
3. **Multi-Tenant:** Unterstützung mehrerer Tenants
4. **Conditional Access:** Erweiterte Sicherheitsrichtlinien 