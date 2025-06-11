# 🔧 Development Guidelines - LindebergsHealth

## Verbindliche Entwicklungsstandards

Diese Regeln sind **IMMER** zu befolgen - keine Ausnahmen!

---

## 📖 1. Dokumentation IMMER aktuell halten

### ✅ Bei jeder Änderung:
- **README.md** updaten wenn Struktur/Setup sich ändert
- **API-Dokumentation** bei neuen Endpoints
- **Architecture Decision Records (ADRs)** bei Design-Entscheidungen
- **Code-Kommentare** für komplexe Business Logic
- **Changelogs** für Breaking Changes

### 📝 Dokumentation-Checklist:
- [ ] README.md aktualisiert?
- [ ] API-Docs generiert/aktualisiert?
- [ ] Neue Features dokumentiert?
- [ ] Breaking Changes beschrieben?

---

## 🌍 2. Commit Messages IMMER in Englisch

### Format (Conventional Commits):
```
<type>(<scope>): <description>

[optional body]

[optional footer(s)]
```

### ✅ Erlaubte Types:
- `feat:` - Neue Features
- `fix:` - Bugfixes  
- `docs:` - Dokumentation
- `style:` - Code-Formatierung
- `refactor:` - Code-Refactoring
- `test:` - Tests hinzufügen/ändern
- `chore:` - Build, Dependencies, etc.

### ✅ Beispiele:
```bash
feat(patient): add patient registration endpoint
fix(auth): resolve token expiration issue
docs(api): update swagger documentation
test(appointment): add unit tests for booking logic
refactor(domain): extract patient validation logic
```

---

## 🏗️ 3. Architektur-Struktur IMMER einhalten

### 🚫 VERBOTEN:
- Domain-Logik in DataAccess
- UI-Logik in Business Layer
- Direkte DB-Zugriffe von Presentation
- Zirkuläre Abhängigkeiten

### ✅ ERLAUBT:
```
Presentation → Business → DataAccess
     ↓           ↓
  nur DTOs   nur Interfaces
```

### Dependency Rules:
```csharp
// ✅ RICHTIG
public class PatientController : ControllerBase
{
    private readonly IPatientService _service; // Interface aus Business
}

// ❌ FALSCH  
public class PatientController : ControllerBase
{
    private readonly PatientRepository _repo; // Direkt DataAccess
}
```

---

## 🧹 4. Clean Code & SOLID Principles

### Single Responsibility Principle (SRP):
```csharp
// ✅ RICHTIG - Eine Verantwortlichkeit
public class PatientValidator
{
    public ValidationResult ValidatePatient(Patient patient) { }
}

// ❌ FALSCH - Mehrere Verantwortlichkeiten
public class PatientService
{
    public void ValidatePatient() { }
    public void SaveToDatabase() { }
    public void SendEmail() { }
}
```

### Open/Closed Principle (OCP):
```csharp
// ✅ RICHTIG - Erweiterbar ohne Änderung
public interface INotificationService
{
    Task SendAsync(string message);
}

public class EmailNotificationService : INotificationService { }
public class SmsNotificationService : INotificationService { }
```

### Liskov Substitution Principle (LSP):
```csharp
// ✅ RICHTIG - Substitutable
public abstract class Patient
{
    public virtual bool CanBookAppointment() => IsActive;
}

public class VipPatient : Patient
{
    public override bool CanBookAppointment() => true; // Erweitert nur
}
```

### Interface Segregation Principle (ISP):
```csharp
// ✅ RICHTIG - Kleine, spezifische Interfaces
public interface IPatientReader
{
    Task<Patient> GetByIdAsync(Guid id);
}

public interface IPatientWriter
{
    Task SaveAsync(Patient patient);
}

// ❌ FALSCH - Fettes Interface
public interface IPatientRepository
{
    Task<Patient> GetByIdAsync(Guid id);
    Task SaveAsync(Patient patient);
    Task DeleteAsync(Guid id);
    Task<Patient[]> SearchAsync(string criteria);
    Task<Patient[]> GetByDoctorAsync(Guid doctorId);
    // ... 20 weitere Methoden
}
```

### Dependency Inversion Principle (DIP):
```csharp
// ✅ RICHTIG - Abhängigkeit von Abstraktionen
public class PatientService
{
    private readonly IPatientRepository _repository;
    private readonly INotificationService _notifications;
}

// ❌ FALSCH - Abhängigkeit von Konkretionen
public class PatientService
{
    private readonly SqlPatientRepository _repository;
    private readonly EmailService _emailService;
}
```

---

## 🧪 5. Test-Driven Development (TDD) - 100% Coverage

### TDD Red-Green-Refactor Cycle:
1. **🔴 RED:** Failing test schreiben
2. **🟢 GREEN:** Minimaler Code um Test zu bestehen
3. **🔄 REFACTOR:** Code verbessern, Tests grün halten

### Test-Pyramide:
```
        🔺 E2E Tests (10%)
      🔺🔺 Integration Tests (20%)  
    🔺🔺🔺🔺 Unit Tests (70%)
```

### Verbindliche Test-Coverage:
- **Unit Tests: 100%** - Keine Ausnahme!
- **Integration Tests: 90%**
- **E2E Tests: 80%**

### Test-Struktur:
```
tests/
├── Business/
│   ├── LindebergsHealth.Domain.Tests/
│   └── LindebergsHealth.Application.Tests/
├── DataAccess/
│   └── LindebergsHealth.DataAccess.Tests/
└── Presentation/
    ├── LindebergsHealth.Api.Tests/
    └── LindebergsHealth.Web.Tests/
```

### Test-Naming Convention:
```csharp
[Test]
public void CreatePatient_WithValidData_ShouldReturnPatientId()
{
    // Arrange
    var patient = new Patient("John", "Doe", DateTime.Now.AddYears(-30));
    
    // Act
    var result = _patientService.CreatePatient(patient);
    
    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
}
```

---

## ✅ Pre-Commit Checklist

Vor **JEDEM** Commit:

- [ ] **Tests:** Alle Tests grün (100% Coverage)
- [ ] **Build:** Solution kompiliert ohne Warnings
- [ ] **Architecture:** 3-Schichten-Struktur eingehalten
- [ ] **SOLID:** Principles befolgt
- [ ] **Documentation:** Aktualisiert
- [ ] **Commit Message:** Englisch, Conventional Format
- [ ] **Code Review:** Self-Review gemacht

---

## 🚫 Definition of "Done"

Ein Feature ist erst **DONE** wenn:

1. ✅ **Code geschrieben** (Clean Code, SOLID)
2. ✅ **Tests geschrieben** (100% Coverage, TDD)
3. ✅ **Dokumentation aktualisiert**
4. ✅ **Code Review** bestanden
5. ✅ **CI/CD Pipeline** grün
6. ✅ **Integration Tests** bestanden
7. ✅ **Deployment** erfolgreich

**Kein Code wird ohne diese 7 Punkte gemergt!** 