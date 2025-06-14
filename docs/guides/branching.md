# 🌿 Git Branching-Strategie

## Übersicht

LindebergsHealth verwendet eine **hybride Git-Flow-Strategie** mit automatischen Azure-Deployments für optimale Entwicklung und Stabilität.

## Branch-Struktur

```
main (Production)           🔥 Azure Production WebApp
  ↑ (Pull Request)
development (Integration)   🧪 Azure Staging WebApp  
  ↑ (Pull Request)
feature/patient-mgmt        👨‍💻 Lokale Entwicklung
feature/appointments        👨‍💻 Lokale Entwicklung  
feature/authentication      👨‍💻 Lokale Entwicklung
```

## Branch-Zwecke

### `main` - Production Branch 🔥
- **Zweck:** Produktive, stabile Version
- **Deployment:** Azure Production WebApp (automatisch)
- **Schutz:** Nur über Pull Request von `development`
- **Stabilität:** Höchste Priorität - nur getestete Features

### `development` - Integration Branch 🧪  
- **Zweck:** Integration und Staging
- **Deployment:** Azure Staging WebApp (automatisch)
- **Merge von:** Feature-Branches via Pull Request
- **Testing:** Integration Tests, UAT, Stakeholder-Review

### `feature/*` - Feature Branches 👨‍💻
- **Zweck:** Isolierte Feature-Entwicklung  
- **Naming:** `feature/feature-name` (z.B. `feature/patient-management`)
- **Basis:** Abzweig von `development`
- **Lebensdauer:** Bis Feature fertig und gemergt

## Workflow

### 1. Feature entwickeln
```bash
# Von development abzweigen
git checkout development
git pull origin development
git checkout -b feature/patient-management

# Entwickeln und commiten
git add .
git commit -m "feat: implement patient management"
git push origin feature/patient-management
```

### 2. Feature integrieren  
```bash
# Pull Request: feature/patient-management → development
# Nach Code Review und Tests: Merge
# → Automatisches Deployment zu Azure Staging
```

### 3. Release  
```bash
# Pull Request: development → main
# Nach finalen Tests: Merge  
# → Automatisches Deployment zu Azure Production
```

## Deployment-Pipeline

### Development Branch → Azure Staging
- **Trigger:** Push/Merge zu `develop`
- **Pipeline:** `.github/workflows/staging.yml`
- **Environment:** Staging/Test
- **URL:** `https://lindebergshealth-staging.azurewebsites.net`

### Main Branch → Azure Production  
- **Trigger:** Push/Merge zu `main`
- **Pipeline:** `.github/workflows/production.yml`  
- **Environment:** Production
- **URL:** `https://lindebergshealth.azurewebsites.net`

---

## Automatisierte Qualitätsprüfungen (CI/CD)

Für alle Pull Requests und Deployments auf `develop` (Staging) und `main` (Production) sind umfangreiche Quality Gates in den GitHub Actions Workflows aktiviert:

- **Code-Formatierung:** Nur korrekt formatierter Code (gemäß Style Guide) wird akzeptiert.
- **Static Code Analysis:** Der Code muss alle statischen Analysen und Roslyn Analyzer bestehen (alle Warnungen als Fehler).
- **Dependency Vulnerability Scan:** Alle verwendeten NuGet-Pakete werden auf bekannte Sicherheitslücken geprüft.
- **Secret Scanning:** GitHub prüft automatisch auf versehentlich eingecheckte Secrets/API-Keys.
- **Test Coverage Gate:** Die Testabdeckung (Statement Coverage) muss mindestens 80 % betragen, sonst schlägt der Build fehl.

**Merges in `develop` und `main` sind nur möglich, wenn alle Checks erfolgreich sind.**

Details und Anpassungen siehe:
- `.github/workflows/staging.yml`
- `.github/workflows/production.yml`

---

## Konventionen

### Branch-Naming
- `feature/feature-name` - Neue Features
- `bugfix/bug-description` - Bugfixes  
- `hotfix/critical-fix` - Kritische Production-Fixes
- `chore/task-description` - Wartungsarbeiten

### Commit-Messages
```
feat: add patient management system
fix: resolve appointment booking bug  
chore: update dependencies
docs: update API documentation
```

### Pull Request Template
1. **Beschreibung:** Was wurde geändert?
2. **Testing:** Wie wurde getestet?
3. **Screenshots:** UI-Änderungen zeigen
4. **Breaking Changes:** Dokumentieren falls vorhanden

## Best Practices

### ✅ Do's  
- Kleine, fokussierte Feature-Branches
- Regelmäßige Commits mit klaren Messages
- Code Review vor jedem Merge
- Tests vor Pull Request
- Development regelmäßig in Feature-Branch mergen

### ❌ Don'ts
- Direkter Push zu `main` 
- Lange lebende Feature-Branches
- Merge ohne Tests
- Unvollständige Features in `development`
- Breaking Changes ohne Dokumentation

## Notfall-Verfahren

### Hotfix für Production
```bash
# Direkt von main abzweigen  
git checkout main
git checkout -b hotfix/critical-security-fix

# Fix implementieren und testen
git commit -m "hotfix: patch security vulnerability"

# Zu main UND development mergen
# → Direktes Production Deployment bei kritischen Fixes
``` 