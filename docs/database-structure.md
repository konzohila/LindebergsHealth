# 🏥 Datenbankstruktur - Lindebergs Arztpraxis-Software

## 📋 Inhaltsverzeichnis
1. [Überblick](#überblick)
2. [Grundprinzipien](#grundprinzipien)
3. [Hauptbereiche](#hauptbereiche)
4. [Detaillierte Tabellenbeschreibung](#detaillierte-tabellenbeschreibung)
5. [Beziehungen zwischen Tabellen](#beziehungen-zwischen-tabellen)
6. [DSGVO-Compliance](#dsgvo-compliance)
7. [Performance-Optimierungen](#performance-optimierungen)

---

## 🎯 Überblick

Die Datenbank für Lindebergs Arztpraxis-Software ist speziell für deutsche Heilpraktiker- und Arztpraxen entwickelt worden. Sie verwaltet alle wichtigen Aspekte einer modernen Praxis:

- **Patientenverwaltung** mit vollständigen Stammdaten
- **Terminplanung** mit flexibler Kategorisierung
- **Therapieverwaltung** für verschiedene Behandlungsarten
- **Mitarbeiterverwaltung** mit Verträgen und Qualifikationen
- **Finanzverwaltung** mit Rechnungen und Budgetplanung
- **CRM-System** für Patientenbeziehungen
- **Dokumentenverwaltung** mit DSGVO-Compliance

---

## 🔧 Grundprinzipien

### 1. **Clean Architecture**
Die Datenbank folgt den Prinzipien der Clean Architecture:
- Klare Trennung von Geschäftslogik und Datenhaltung
- Unabhängigkeit von externen Frameworks
- Testbarkeit und Wartbarkeit

### 2. **DSGVO-Compliance**
Jede Tabelle enthält:
- **Soft Delete**: Daten werden nicht physisch gelöscht, sondern nur markiert
- **Audit Trail**: Wer hat wann was geändert?
- **Löschgrund**: Warum wurden Daten gelöscht?
- **Versionierung**: Alle Änderungen werden nachverfolgbar gespeichert

### 3. **Deutsche Lokalisierung**
- Alle Feldnamen sind auf Deutsch
- Lookup-Tabellen enthalten deutsche Standardwerte
- Berücksichtigung deutscher Gesetze und Normen

---

## 🏗️ Hauptbereiche

Die Datenbank ist in folgende Hauptbereiche unterteilt:

### 👤 **Patientenverwaltung**
- Grunddaten, Erweiterungen, Versicherungen
- Adressen, Kontakte, Beziehungspersonen
- Notfallkontakte, Kommunikationseinstellungen

### 📅 **Terminplanung**
- Termine mit flexibler Kategorisierung
- Terminvorlagen und -serien
- Wartelisten und Änderungsprotokolle

### 🩺 **Therapieverwaltung**
- Therapieserien und Einzelsitzungen
- Befunde und Körperstatus
- Behandlungspläne und Checklisten

### 👨‍⚕️ **Mitarbeiterverwaltung**
- Stammdaten und Verträge
- Qualifikationen und Fortbildungen
- Arbeitszeiten und Urlaubsplanung

### 💰 **Finanzverwaltung**
- Rechnungen und Positionen
- Zahlungen und Mahnungen
- Budgetplanung und Kostenstellenrechnung

### 🏢 **Praxisverwaltung**
- Praxisdaten und Räume
- Ausstattung und Inventar
- Kooperationspartner

### 🤝 **CRM-System**
- Kundenstatus und -kategorien
- Netzwerk und Empfehlungen
- Kommunikationsverlauf

---

## 📊 Detaillierte Tabellenbeschreibung

### 👤 **1. Patientenverwaltung**

#### **Patient** (Haupttabelle)
**Zweck**: Speichert die Grunddaten aller Patienten

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `Vorname` | String | Vorname des Patienten | "Max" |
| `Nachname` | String | Nachname des Patienten | "Mustermann" |
| `Geburtsdatum` | DateTime | Geburtsdatum | 1985-03-15 |
| `GeschlechtId` | GUID | Verweis auf Geschlecht | → Männlich |

**Verbundene Tabellen**:
- `PatientErweiterung`: Zusätzliche Daten (Familienstand, Staatsangehörigkeit, etc.)
- `PatientVersicherung`: Krankenversicherungsdaten
- `PatientAdresse`: Wohn- und Postadresse
- `PatientKontakt`: Telefon, E-Mail, etc.
- `PatientBeziehungsperson`: Angehörige und Kontaktpersonen

#### **PatientErweiterung**
**Zweck**: Erweiterte Patientendaten für spezielle Anforderungen

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `PatientId` | GUID | Verweis auf Patient | → Max Mustermann |
| `Beruf` | String | Beruf des Patienten | "Softwareentwickler" |
| `FamilienstandId` | GUID | Familienstand | → Verheiratet |
| `StaatsangehoerigkeitId` | GUID | Staatsangehörigkeit | → Deutsch |
| `Muttersprache` | String | Muttersprache | "Deutsch" |
| `Besonderheiten` | Text | Medizinische Besonderheiten | "Allergien: Nüsse" |

#### **PatientVersicherung**
**Zweck**: Verwaltung der Krankenversicherungsdaten

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `PatientId` | GUID | Verweis auf Patient | → Max Mustermann |
| `VersicherungstypId` | GUID | Art der Versicherung | → PKV |
| `VersicherungsstatusId` | GUID | Status | → Vollversicherung |
| `Versicherungsnummer` | String | Versichertennummer | "A123456789" |
| `Versicherungsname` | String | Name der Krankenkasse | "AOK Bayern" |
| `GueltigVon` | DateTime | Gültig ab | 2024-01-01 |
| `GueltigBis` | DateTime | Gültig bis | 2024-12-31 |

---

### 📅 **2. Terminplanung**

#### **Termin** (Haupttabelle)
**Zweck**: Verwaltung aller Termine in der Praxis

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `Datum` | DateTime | Datum und Uhrzeit | 2024-03-15 10:00 |
| `DauerMinuten` | Integer | Dauer in Minuten | 60 |
| `PatientId` | GUID | Welcher Patient? | → Max Mustermann |
| `MitarbeiterId` | GUID | Welcher Therapeut? | → Dr. Schmidt |
| `RaumId` | GUID | Welcher Raum? | → Behandlungsraum 1 |
| `TermintypId` | GUID | Art des Termins | → Osteopathie |
| `TerminstatusId` | GUID | Status | → Bestätigt |

#### **TerminKategorie**
**Zweck**: Kategorisierung und Farbkodierung von Terminen

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Bereich` | String | Fachbereich | "OST" (Osteopathie) |
| `Bereichsbezeichnung` | String | Anzeigename | "Osteopathie" |
| `Farbe` | String | Farbname | "Blau" |
| `Farbcode` | String | HEX-Farbcode | "#0066CC" |
| `Code` | String | Eindeutiger Code | "NEU-OST-60" |
| `DauerMinuten` | Integer | Standarddauer | 60 |
| `Kundentyp` | String | Kundentyp | "NEU" (Neukunde) |
| `Versicherung` | String | Versicherungsart | "PKV" |

---

### 🩺 **3. Therapieverwaltung**

#### **TherapieSerie**
**Zweck**: Verwaltung von Behandlungsserien (z.B. 10x Physiotherapie)

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `PatientId` | GUID | Welcher Patient? | → Max Mustermann |
| `TherapietypId` | GUID | Art der Therapie | → Osteopathie |
| `AnzahlGeplant` | Integer | Geplante Sitzungen | 6 |
| `AnzahlAbgeschlossen` | Integer | Bereits durchgeführt | 3 |
| `StartDatum` | DateTime | Beginn der Serie | 2024-03-01 |
| `EndDatum` | DateTime | Geplantes Ende | 2024-04-15 |
| `TherapieserieStatusId` | GUID | Status der Serie | → Laufend |

#### **TherapieEinheit**
**Zweck**: Einzelne Therapiesitzungen mit Befunden

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `TerminId` | GUID | Zugehöriger Termin | → Termin vom 15.03.2024 |
| `PatientId` | GUID | Patient | → Max Mustermann |
| `TherapeutId` | GUID | Behandelnder Therapeut | → Dr. Schmidt |
| `TherapietypId` | GUID | Art der Therapie | → Osteopathie |
| `TherapiestatusId` | GUID | Status | → Abgeschlossen |
| `Befund` | Text | Medizinischer Befund | "Verspannung im LWS-Bereich" |
| `Behandlung` | Text | Durchgeführte Behandlung | "Mobilisation L4/L5" |
| `Notizen` | Text | Zusätzliche Notizen | "Patient reagiert gut" |
| `Dauer` | Integer | Behandlungsdauer | 45 |

#### **Koerperstatus**
**Zweck**: Dokumentation von Körperbefunden und Schmerzskala

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `PatientId` | GUID | Patient | → Max Mustermann |
| `TherapeutId` | GUID | Untersuchender Therapeut | → Dr. Schmidt |
| `KoerperregionId` | GUID | Körperregion | → Lendenwirbelsäule |
| `Befund` | Text | Befundbeschreibung | "Bewegungseinschränkung" |
| `SchmerzSkala` | Integer | Schmerz 0-10 | 6 |

---

### 👨‍⚕️ **4. Mitarbeiterverwaltung**

#### **Mitarbeiter** (Haupttabelle)
**Zweck**: Grunddaten aller Mitarbeiter

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `Vorname` | String | Vorname | "Dr. Anna" |
| `Nachname` | String | Nachname | "Schmidt" |
| `Email` | String | E-Mail-Adresse | "a.schmidt@praxis.de" |
| `Telefon` | String | Telefonnummer | "+49 89 12345678" |

#### **MitarbeiterDetails**
**Zweck**: Erweiterte Mitarbeiterdaten

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `MitarbeiterId` | GUID | Verweis auf Mitarbeiter | → Dr. Schmidt |
| `Geburtsdatum` | DateTime | Geburtsdatum | 1980-05-20 |
| `GeschlechtId` | GUID | Geschlecht | → Weiblich |
| `Staatsangehoerigkeit` | String | Staatsangehörigkeit | "Deutsch" |
| `Sozialversicherungsnummer` | String | SV-Nummer | "12 345678 A 123" |

#### **MitarbeiterVertrag**
**Zweck**: Arbeitsverträge und Konditionen

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `MitarbeiterId` | GUID | Verweis auf Mitarbeiter | → Dr. Schmidt |
| `FunktionId` | GUID | Berufsbezeichnung | → Osteopath |
| `AbteilungId` | GUID | Abteilung | → Osteopathie |
| `VertragsformId` | GUID | Vertragsart | → Vollzeit |
| `GehaltBrutto` | Decimal | Bruttogehalt | 4500.00 |
| `UrlaubstageJahr` | Integer | Urlaubstage | 30 |
| `VertragsBeginn` | DateTime | Vertragsbeginn | 2024-01-01 |
| `VertragsEnde` | DateTime | Vertragsende | null (unbefristet) |

---

### 💰 **5. Finanzverwaltung**

#### **Rechnung**
**Zweck**: Verwaltung aller Rechnungen

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `Rechnungsnummer` | String | Eindeutige Nummer | "2024-0001" |
| `PatientId` | GUID | Rechnungsempfänger | → Max Mustermann |
| `RechnungsstatusId` | GUID | Status | → Offen |
| `Rechnungsdatum` | DateTime | Rechnungsdatum | 2024-03-15 |
| `Faelligkeitsdatum` | DateTime | Fälligkeitsdatum | 2024-04-14 |
| `BetragNetto` | Decimal | Nettobetrag | 95.00 |
| `BetragBrutto` | Decimal | Bruttobetrag | 113.05 |
| `Steuersatz` | Decimal | Mehrwertsteuersatz | 19.0 |

#### **RechnungsPosition**
**Zweck**: Einzelne Positionen einer Rechnung

| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `RechnungId` | GUID | Zugehörige Rechnung | → Rechnung 2024-0001 |
| `TerminId` | GUID | Zugehöriger Termin | → Termin vom 15.03.2024 |
| `Bezeichnung` | String | Leistungsbeschreibung | "Osteopathische Behandlung" |
| `Menge` | Decimal | Anzahl | 1 |
| `Einzelpreis` | Decimal | Preis pro Einheit | 95.00 |
| `Gesamtpreis` | Decimal | Gesamtpreis | 95.00 |

---

### 📋 **6. Lookup-Tabellen (Stammdaten)**

Lookup-Tabellen enthalten vordefinierte Werte, die in anderen Tabellen verwendet werden. Alle basieren auf der `BaseLookupEntity`:

#### **BaseLookupEntity** (Basisklasse)
| Feld | Typ | Beschreibung | Beispiel |
|------|-----|-------------|----------|
| `Id` | GUID | Eindeutige ID | a1b2c3d4-... |
| `Bezeichnung` | String | Anzeigename | "Männlich" |
| `Code` | String | Eindeutiger Code | "M" |
| `Sortierung` | Integer | Sortierreihenfolge | 1 |
| `Aktiv` | Boolean | Ist aktiv? | true |
| `Beschreibung` | String | Zusätzliche Beschreibung | "Männliches Geschlecht" |

#### **Wichtige Lookup-Tabellen**:

**Geschlecht**:
- Männlich (Code: M)
- Weiblich (Code: W)
- Divers (Code: D)

**Familienstand**:
- Ledig
- Verheiratet
- Geschieden
- Verwitwet
- Lebenspartnerschaft

**Versicherungstyp**:
- Gesetzliche Krankenversicherung (GKV)
- Private Krankenversicherung (PKV)
- Beihilfe
- Selbstzahler

**Termintyp**:
- Erstbehandlung
- Folgebehandlung
- Beratungsgespräch
- Nachkontrolle

**Therapietyp**:
- Osteopathie
- Physiotherapie
- Massage
- Cranio-Sacral-Therapie

**Terminstatus**:
- Geplant
- Bestätigt
- Abgeschlossen
- Abgesagt
- Nicht erschienen

---

## 🔗 Beziehungen zwischen Tabellen

### **1:1 Beziehungen** (Ein-zu-Eins)
- `Patient` ↔ `PatientErweiterung`
- `Mitarbeiter` ↔ `MitarbeiterDetails`

### **1:n Beziehungen** (Ein-zu-Viele)
- `Patient` → `Termin` (Ein Patient hat viele Termine)
- `Mitarbeiter` → `Termin` (Ein Mitarbeiter hat viele Termine)
- `Patient` → `Rechnung` (Ein Patient hat viele Rechnungen)
- `Rechnung` → `RechnungsPosition` (Eine Rechnung hat viele Positionen)
- `TherapieSerie` → `TherapieEinheit` (Eine Serie hat viele Einzelsitzungen)

### **n:m Beziehungen** (Viele-zu-Viele)
- `Patient` ↔ `Versicherung` (über `PatientVersicherung`)
- `Patient` ↔ `Adresse` (über `PatientAdresse`)
- `Mitarbeiter` ↔ `Qualifikation` (über `MitarbeiterQualifikation`)

### **Lookup-Beziehungen**
Jede Haupttabelle ist mit entsprechenden Lookup-Tabellen verknüpft:
- `Patient.GeschlechtId` → `Geschlecht.Id`
- `Termin.TermintypId` → `Termintyp.Id`
- `Rechnung.RechnungsstatusId` → `Rechnungsstatus.Id`

---

## 🔒 DSGVO-Compliance

### **Soft Delete Prinzip**
Anstatt Daten physisch zu löschen, werden sie nur als gelöscht markiert:

**Jede Tabelle enthält**:
- `IstGelöscht` (Boolean) - Ist der Datensatz gelöscht?
- `GelöschtAm` (DateTime) - Wann wurde gelöscht?
- `GelöschtVon` (GUID) - Wer hat gelöscht?
- `LöschGrund` (String) - Warum wurde gelöscht?

### **Audit Trail**
Jede Tabelle protokolliert:
- `ErstelltAm` (DateTime) - Wann wurde der Datensatz erstellt?
- `ErstelltVon` (GUID) - Wer hat den Datensatz erstellt?
- `GeändertAm` (DateTime) - Wann wurde zuletzt geändert?
- `GeändertVon` (GUID) - Wer hat zuletzt geändert?

### **Historisierung**
Für wichtige Tabellen gibt es History-Tabellen:
- `PatientHistory` - Alle Änderungen an Patientendaten
- `TerminHistory` - Alle Terminänderungen
- `RechnungHistory` - Alle Rechnungsänderungen

**History-Tabellen enthalten zusätzlich**:
- `OriginalId` (GUID) - Verweis auf ursprünglichen Datensatz
- `GültigVon` (DateTime) - Ab wann war diese Version gültig?
- `GültigBis` (DateTime) - Bis wann war diese Version gültig?
- `ÄnderungsTyp` (String) - Art der Änderung (INSERT, UPDATE, DELETE)

### **Versionierung**
- `RowVersion` (Byte[]) - Optimistic Concurrency Control
- Verhindert gleichzeitige Änderungen durch mehrere Benutzer

---

## ⚡ Performance-Optimierungen

### **Automatische Indizes**
Das System erstellt automatisch Indizes für häufige Abfragen:

```sql
-- Soft Delete Performance
CREATE INDEX IX_Patient_IstGelöscht ON Patient (IstGelöscht)
CREATE INDEX IX_Termin_IstGelöscht ON Termin (IstGelöscht)

-- Terminsuche
CREATE INDEX IX_Termin_Datum_Patient ON Termin (Datum, PatientId)
CREATE INDEX IX_Termin_Mitarbeiter_Datum ON Termin (MitarbeiterId, Datum)

-- Rechnungssuche  
CREATE INDEX IX_Rechnung_Patient_Status ON Rechnung (PatientId, RechnungsstatusId)
CREATE INDEX IX_Rechnung_Datum ON Rechnung (Rechnungsdatum)

-- History-Performance
CREATE INDEX IX_PatientHistory_OriginalGueltigVon ON PatientHistory (OriginalId, GültigVon)
```

### **Query Filter**
Automatische Filterung gelöschter Datensätze:
```sql
-- Wird automatisch zu jeder Abfrage hinzugefügt
WHERE IstGelöscht = 0
```

### **Composite Indizes**
Für komplexe Abfragen:
```sql
-- Terminkalender-Abfragen
CREATE INDEX IX_Termin_Datum_Mitarbeiter_Status 
ON Termin (Datum, MitarbeiterId, TerminstatusId)

-- Patientensuche
CREATE INDEX IX_Patient_Name_Geburt 
ON Patient (Nachname, Vorname, Geburtsdatum)
```

---

## 📈 Erweiterbarkeit

### **Modularer Aufbau**
Die Datenbank ist modular aufgebaut und kann einfach erweitert werden:

- **Neue Therapieformen**: Einfach neue Einträge in `Therapietyp`
- **Neue Raumtypen**: Erweiterung der `Raumtyp`-Lookup
- **Zusätzliche Patientendaten**: Erweiterung von `PatientErweiterung`
- **Neue Berichte**: Alle Daten sind über Views und Stored Procedures abrufbar

### **API-Ready**
Die Struktur ist optimal für REST-APIs:
- Klare Entity-Abgrenzung
- Normalisierte Datenstruktur
- Konsistente Namenskonventionen
- JSON-serialisierbare Objekte

### **Multi-Mandanten-Fähig**
Kann einfach für mehrere Praxen erweitert werden:
- Hinzufügung einer `PraxisId` zu allen relevanten Tabellen
- Row-Level Security für Datentrennung
- Mandanten-spezifische Konfigurationen

---

## 🎯 Fazit

Die Datenbankstruktur von Lindebergs Arztpraxis-Software ist:

✅ **DSGVO-konform** - Vollständige Compliance mit deutschen Datenschutzgesetzen  
✅ **Skalierbar** - Wächst mit den Anforderungen der Praxis  
✅ **Performant** - Optimiert für schnelle Abfragen  
✅ **Wartbar** - Klare Struktur und Dokumentation  
✅ **Erweiterbar** - Modularer Aufbau für zukünftige Anforderungen  
✅ **Deutsch** - Vollständig lokalisiert für deutsche Praxen  
✅ **Professionell** - Enterprise-Grade Architektur

Die Datenbank bildet das solide Fundament für eine moderne, digitale Arztpraxis und unterstützt alle wichtigen Geschäftsprozesse von der Patientenaufnahme bis zur Abrechnung.

---

## 📞 Support

Bei Fragen zur Datenbankstruktur oder Erweiterungswünschen wenden Sie sich an das Entwicklungsteam.

**Letzte Aktualisierung**: März 2024  
**Version**: 1.0  
**Status**: Produktionsreif 