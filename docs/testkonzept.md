# Testkonzept LindebergsHealth

## 1. Testziele und Strategie
- Ziel: Sicherstellen, dass Kernfunktionen (Terminverwaltung, Finanzlogik, Mitarbeiterverwaltung) korrekt, stabil und regressionssicher laufen.
- Schwerpunkt: Business-Logik (Domain), Services, Schnittstellen (API), kritische Infrastruktur.
- Abdeckung: Hohe Code Coverage für Kernlogik, aber kein „Coverage um jeden Preis“ (Qualität vor Quantität).

## 2. Testarten
- **Unit Tests:** Sinnvolle, fachlich und technisch relevante Coverage (nicht 100% um jeden Preis). Für alle fachlich oder technisch relevanten Domain-Modelle, ValueObjects, Services, Mapper. Fokus: Validierung von Logik, Randfällen, Fehlerbehandlung. Keine Tests für reine Daten- oder Mapping-Entities ohne Logik.
- **Integration Tests:** Möglichst hohe Coverage für kritische Flows. Für Datenbankzugriffe (EF Core InMemory/SQLite), Service-Layer, API-Endpunkte. Fokus: Zusammenspiel mehrerer Komponenten, Datenpersistenz, CQRS-Flows.
- **End-to-End-Tests (E2E)**: Für die wichtigsten User-Flows (z.B. Terminbuchung, Rechnungserstellung) über die UI/API. Fokus: User Experience, End-to-End-Korrektheit.
- **Smoke-Tests**: Nach jedem Deployment: Sind die wichtigsten Endpunkte erreichbar und liefern sie sinnvolle Antworten?

## 3. Teststruktur
- Namenskonvention: Klar nach Layer/Modul getrennte Testprojekte (Domain.Tests, Application.Tests, Infrastructure.Tests, API.Tests, BlazorApp.Tests). Leere Testprojekte werden entfernt, nur sinnvolle Layer werden getestet.
- Testdaten: Möglichst wenig „Magie“: Testdaten explizit, nachvollziehbar, keine versteckten Abhängigkeiten.
- Mocking: Nur für externe Services, Datenbankzugriffe, Zeit/Datum, Authentifizierung.

## 4. Automatisierung
- **CI/CD**: Tests (NUnit) werden bei jedem Commit/PR automatisch ausgeführt (GitHub Actions). Die Coverage wird ermittelt und als Report bereitgestellt. Ziel ist eine sinnvolle, fachlich und technisch begründete Testabdeckung, nicht 100% um jeden Preis.
- **Fehlerkultur**: Tests dürfen nicht „rot“ sein auf main/master. Flaky Tests werden sofort gefixt oder isoliert.

## 5. Beispiel für Testabdeckung
- **Domain**: Validierungslogik (z.B. Termin darf nicht in der Vergangenheit liegen), Berechnungen (z.B. Gehalt, Rechnungsbeträge inkl. Steuern)
- **Application**: Service-Methoden (z.B. Terminbuchung, Patientenanlage)
- **API**: HTTP-Statuscodes, Fehlerfälle, Security (z.B. unautorisierte Requests)
- **Infrastructure**: Repository-Methoden, Migrationslogik

## 6. Empfohlene Reihenfolge für den Start
1. Unit-Tests für Domain-Logik (z.B. Termin, Patient, Rechnung)
2. Integrationstests für Service-Layer und Datenbank
3. API-Tests für die wichtigsten Endpunkte
4. E2E-Tests für die wichtigsten User-Flows
