using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Fortbildung Entity - Funktionalitätstests
/// </summary>
[TestFixture]
public class FortbildungTests
{
    private Fortbildung _fortbildung;
    private Mitarbeiter _mitarbeiter;
    private Fortbildungstyp _fortbildungstyp;
    private Fortbildungsstatus _fortbildungsstatus;

    [SetUp]
    public void Setup()
    {
        _mitarbeiter = new Mitarbeiter
        {
            Id = Guid.NewGuid(),
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        _fortbildungstyp = new Fortbildungstyp
        {
            Id = Guid.NewGuid(),
            Bezeichnung = "Osteopathie Weiterbildung",
            Code = "OST_WB",
            Aktiv = true
        };

        _fortbildungsstatus = new Fortbildungsstatus
        {
            Id = Guid.NewGuid(),
            Bezeichnung = "Abgeschlossen",
            Code = "COMPLETED",
            Aktiv = true
        };

        _fortbildung = new Fortbildung
        {
            MitarbeiterId = _mitarbeiter.Id,
            Mitarbeiter = _mitarbeiter,
            FortbildungstypId = _fortbildungstyp.Id,
            Fortbildungstyp = _fortbildungstyp,
            FortbildungsstatusId = _fortbildungsstatus.Id,
            Fortbildungsstatus = _fortbildungsstatus,
            Titel = "Craniosacrale Osteopathie - Grundkurs",
            Anbieter = "Deutsche Gesellschaft für Osteopathie",
            Startdatum = DateTime.Today.AddDays(-30),
            Enddatum = DateTime.Today.AddDays(-25),
            Stunden = 40,
            Kosten = 850.00m,
            IstPflichtfortbildung = true
        };
    }

    [Test]
    public void Fortbildung_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_fortbildung, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Fortbildung_ShouldHaveValidTrainingData()
    {
        // Assert
        Assert.That(_fortbildung.MitarbeiterId, Is.EqualTo(_mitarbeiter.Id));
        Assert.That(_fortbildung.Mitarbeiter, Is.EqualTo(_mitarbeiter));
        Assert.That(_fortbildung.FortbildungstypId, Is.EqualTo(_fortbildungstyp.Id));
        Assert.That(_fortbildung.Fortbildungstyp, Is.EqualTo(_fortbildungstyp));
        Assert.That(_fortbildung.Titel, Is.EqualTo("Craniosacrale Osteopathie - Grundkurs"));
        Assert.That(_fortbildung.Anbieter, Is.EqualTo("Deutsche Gesellschaft für Osteopathie"));
        Assert.That(_fortbildung.Stunden, Is.EqualTo(40));
        Assert.That(_fortbildung.Kosten, Is.EqualTo(850.00m));
        Assert.That(_fortbildung.IstPflichtfortbildung, Is.True);
    }

    [Test]
    public void Fortbildung_ShouldValidateDateRange()
    {
        // Arrange
        var startDate = new DateTime(2024, 3, 1);
        var endDate = new DateTime(2024, 3, 5);

        // Act
        _fortbildung.Startdatum = startDate;
        _fortbildung.Enddatum = endDate;

        // Assert
        Assert.That(_fortbildung.Startdatum, Is.EqualTo(startDate));
        Assert.That(_fortbildung.Enddatum, Is.EqualTo(endDate));
        Assert.That(_fortbildung.Enddatum, Is.GreaterThanOrEqualTo(_fortbildung.Startdatum));
        
        // Berechne Dauer
        var dauer = (_fortbildung.Enddatum - _fortbildung.Startdatum).Days + 1;
        Assert.That(dauer, Is.EqualTo(5));
    }

    [Test]
    public void Fortbildung_ShouldSupportDifferentTrainingTypes()
    {
        // Arrange
        var trainingTypes = new[]
        {
            new Fortbildungstyp { Bezeichnung = "Osteopathie", Code = "OST" },
            new Fortbildungstyp { Bezeichnung = "Physiotherapie", Code = "PHY" },
            new Fortbildungstyp { Bezeichnung = "Massage", Code = "MAS" },
            new Fortbildungstyp { Bezeichnung = "Erste Hilfe", Code = "EH" },
            new Fortbildungstyp { Bezeichnung = "Datenschutz", Code = "DS" },
            new Fortbildungstyp { Bezeichnung = "Qualitätsmanagement", Code = "QM" }
        };

        // Act & Assert
        foreach (var trainingType in trainingTypes)
        {
            _fortbildung.FortbildungstypId = trainingType.Id;
            _fortbildung.Fortbildungstyp = trainingType;

            Assert.That(_fortbildung.Fortbildungstyp.Bezeichnung, Is.EqualTo(trainingType.Bezeichnung));
            Assert.That(_fortbildung.Fortbildungstyp.Code, Is.EqualTo(trainingType.Code));
        }
    }

    [Test]
    public void Fortbildung_ShouldTrackTrainingStatus()
    {
        // Arrange
        var statusProgression = new[]
        {
            new Fortbildungsstatus { Bezeichnung = "Geplant", Code = "PLANNED" },
            new Fortbildungsstatus { Bezeichnung = "Angemeldet", Code = "REGISTERED" },
            new Fortbildungsstatus { Bezeichnung = "Laufend", Code = "ONGOING" },
            new Fortbildungsstatus { Bezeichnung = "Abgeschlossen", Code = "COMPLETED" },
            new Fortbildungsstatus { Bezeichnung = "Abgebrochen", Code = "CANCELLED" }
        };

        // Act & Assert
        foreach (var status in statusProgression)
        {
            _fortbildung.FortbildungsstatusId = status.Id;
            _fortbildung.Fortbildungsstatus = status;

            Assert.That(_fortbildung.Fortbildungsstatus.Bezeichnung, Is.EqualTo(status.Bezeichnung));
            Assert.That(_fortbildung.Fortbildungsstatus.Code, Is.EqualTo(status.Code));
        }
    }

    [Test]
    public void Fortbildung_ShouldCalculateHourlyRate()
    {
        // Arrange
        var kosten = 850.00m;
        var stunden = 40;

        // Act
        _fortbildung.Kosten = kosten;
        _fortbildung.Stunden = stunden;

        var stundenpreis = _fortbildung.Kosten / _fortbildung.Stunden;

        // Assert
        Assert.That(stundenpreis, Is.EqualTo(21.25m));
    }

    [Test]
    public void Fortbildung_ShouldSupportCertificationTracking()
    {
        // Arrange
        var zertifikatsdaten = @"
ZERTIFIKAT:
Titel: Craniosacrale Osteopathie - Grundkurs
Anbieter: Deutsche Gesellschaft für Osteopathie
Zertifikatsnummer: DGO-2024-CSO-001
Ausstellungsdatum: 15.03.2024
Gültigkeitsdauer: 5 Jahre
Nächste Rezertifizierung: 15.03.2029

INHALTE:
- Anatomie des craniosacralen Systems
- Palpationstechniken
- Behandlungsprotokoll
- Praktische Übungen (32 Stunden)
- Theoretische Prüfung (bestanden)";

        // Act
        _fortbildung.Zertifikat = zertifikatsdaten;
        _fortbildung.Zertifikatsnummer = "DGO-2024-CSO-001";
        _fortbildung.ZertifikatGueltigBis = DateTime.Today.AddYears(5);

        // Assert
        Assert.That(_fortbildung.Zertifikat, Contains.Substring("ZERTIFIKAT"));
        Assert.That(_fortbildung.Zertifikat, Contains.Substring("INHALTE"));
        Assert.That(_fortbildung.Zertifikatsnummer, Is.EqualTo("DGO-2024-CSO-001"));
        Assert.That(_fortbildung.ZertifikatGueltigBis, Is.EqualTo(DateTime.Today.AddYears(5)));
    }

    [Test]
    public void Fortbildung_ShouldValidateMandatoryTraining()
    {
        // Arrange - Pflichtfortbildungen
        var pflichtfortbildungen = new[]
        {
            new Fortbildung
            {
                Titel = "Erste Hilfe Kurs",
                IstPflichtfortbildung = true,
                ZertifikatGueltigBis = DateTime.Today.AddYears(2),
                Stunden = 8
            },
            new Fortbildung
            {
                Titel = "Datenschutz Schulung",
                IstPflichtfortbildung = true,
                ZertifikatGueltigBis = DateTime.Today.AddYears(1),
                Stunden = 4
            },
            new Fortbildung
            {
                Titel = "Hygiene Schulung",
                IstPflichtfortbildung = true,
                ZertifikatGueltigBis = DateTime.Today.AddYears(1),
                Stunden = 2
            }
        };

        // Act
        foreach (var fortbildung in pflichtfortbildungen)
        {
            _mitarbeiter.Fortbildungen.Add(fortbildung);
        }

        // Assert
        var pflichtFortbildungen = _mitarbeiter.Fortbildungen.Where(f => f.IstPflichtfortbildung).ToList();
        Assert.That(pflichtFortbildungen.Count, Is.EqualTo(3));
        
        var gesamtStundenPflicht = pflichtFortbildungen.Sum(f => f.Stunden);
        Assert.That(gesamtStundenPflicht, Is.EqualTo(14));
    }

    [Test]
    public void Fortbildung_ShouldCheckCertificationExpiry()
    {
        // Arrange
        var heute = DateTime.Today;
        
        // Test 1: Gültiges Zertifikat
        _fortbildung.ZertifikatGueltigBis = heute.AddMonths(6);
        var istGueltig = _fortbildung.ZertifikatGueltigBis >= heute;
        Assert.That(istGueltig, Is.True);
        
        // Test 2: Bald ablaufendes Zertifikat (Warnung)
        _fortbildung.ZertifikatGueltigBis = heute.AddDays(30);
        var baldAbgelaufen = (_fortbildung.ZertifikatGueltigBis - heute).Days <= 60;
        Assert.That(baldAbgelaufen, Is.True);
        
        // Test 3: Abgelaufenes Zertifikat
        _fortbildung.ZertifikatGueltigBis = heute.AddDays(-1);
        istGueltig = _fortbildung.ZertifikatGueltigBis >= heute;
        Assert.That(istGueltig, Is.False);
    }

    [Test]
    public void Fortbildung_ShouldSupportCostTracking()
    {
        // Arrange - Kostenaufstellung
        var kostenaufstellung = @"
KOSTENAUFSTELLUNG:
Kursgebühr: 750,00 €
Unterkunft: 80,00 € (2 Nächte à 40€)
Verpflegung: 20,00 €
Anreise: 0,00 € (vor Ort)
GESAMT: 850,00 €

FINANZIERUNG:
Arbeitgeber: 850,00 € (100%)
Eigenanteil: 0,00 €

STEUERLICHE BEHANDLUNG:
Betriebsausgabe: Ja
Umsatzsteuer: 19% (135,29 €)
Nettobetrag: 714,71 €";

        // Act
        _fortbildung.Kosten = 850.00m;
        _fortbildung.Kostenaufstellung = kostenaufstellung;
        _fortbildung.ArbeitgeberAnteil = 850.00m;
        _fortbildung.Eigenanteil = 0.00m;

        // Assert
        Assert.That(_fortbildung.Kosten, Is.EqualTo(850.00m));
        Assert.That(_fortbildung.ArbeitgeberAnteil, Is.EqualTo(850.00m));
        Assert.That(_fortbildung.Eigenanteil, Is.EqualTo(0.00m));
        Assert.That(_fortbildung.ArbeitgeberAnteil + _fortbildung.Eigenanteil, Is.EqualTo(_fortbildung.Kosten));
        Assert.That(_fortbildung.Kostenaufstellung, Contains.Substring("KOSTENAUFSTELLUNG"));
        Assert.That(_fortbildung.Kostenaufstellung, Contains.Substring("FINANZIERUNG"));
    }

    [Test]
    public void Fortbildung_ShouldTrackLearningObjectives()
    {
        // Arrange - Lernziele und Kompetenzen
        var lernziele = @"
LERNZIELE:
1. Anatomie des craniosacralen Systems verstehen
2. Palpationstechniken erlernen und anwenden
3. Behandlungsprotokoll entwickeln
4. Kontraindikationen erkennen
5. Dokumentation der Behandlung

ERWORBENE KOMPETENZEN:
✓ Craniosacrale Rhythmus ertasten
✓ Spannungsmuster identifizieren
✓ Sanfte Korrekturtechniken anwenden
✓ Behandlungsplan erstellen
✓ Patientenaufklärung durchführen

PRAXISRELEVANZ:
- Erweiterung des Behandlungsspektrums
- Verbesserung der Behandlungsqualität
- Neue Patientengruppen ansprechen
- Alleinstellungsmerkmal entwickeln";

        // Act
        _fortbildung.Lernziele = lernziele;

        // Assert
        Assert.That(_fortbildung.Lernziele, Contains.Substring("LERNZIELE"));
        Assert.That(_fortbildung.Lernziele, Contains.Substring("ERWORBENE KOMPETENZEN"));
        Assert.That(_fortbildung.Lernziele, Contains.Substring("PRAXISRELEVANZ"));
        Assert.That(_fortbildung.Lernziele.Count(c => c == '✓'), Is.EqualTo(5));
    }

    [Test]
    public void Fortbildung_ShouldSupportQualityAssessment()
    {
        // Arrange - Bewertung der Fortbildung
        var bewertung = @"
BEWERTUNG DER FORTBILDUNG:

INHALT: ⭐⭐⭐⭐⭐ (5/5)
- Sehr praxisnah und gut strukturiert
- Aktuelle wissenschaftliche Erkenntnisse
- Gute Balance zwischen Theorie und Praxis

DOZENT: ⭐⭐⭐⭐⭐ (5/5)
- Sehr kompetent und erfahren
- Gute didaktische Fähigkeiten
- Individuelle Betreuung

ORGANISATION: ⭐⭐⭐⭐ (4/5)
- Pünktlicher Start und Ende
- Gute Räumlichkeiten
- Kleine Verbesserungen bei der Verpflegung möglich

GESAMTBEWERTUNG: ⭐⭐⭐⭐⭐ (5/5)
WEITEREMPFEHLUNG: Ja, unbedingt!";

        // Act
        _fortbildung.Bewertung = bewertung;
        _fortbildung.GesamtBewertung = 5;
        _fortbildung.Weiterempfehlung = true;

        // Assert
        Assert.That(_fortbildung.Bewertung, Contains.Substring("BEWERTUNG DER FORTBILDUNG"));
        Assert.That(_fortbildung.Bewertung, Contains.Substring("GESAMTBEWERTUNG"));
        Assert.That(_fortbildung.GesamtBewertung, Is.EqualTo(5));
        Assert.That(_fortbildung.Weiterempfehlung, Is.True);
        Assert.That(_fortbildung.Bewertung.Count(c => c == '⭐'), Is.GreaterThan(0));
    }

    [Test]
    public void Fortbildung_ShouldCalculateROI()
    {
        // Arrange - Return on Investment Berechnung
        var kosten = 850.00m;
        var zusaetzlicherStundensatz = 5.00m; // Durch Spezialisierung
        var mehrStundenProWoche = 2; // Zusätzliche Behandlungen
        var wochenProJahr = 50;

        // Act
        var jaehrlicheMehreinnahmen = zusaetzlicherStundensatz * mehrStundenProWoche * wochenProJahr;
        var amortisationInMonaten = (kosten / (jaehrlicheMehreinnahmen / 12));

        // Assert
        Assert.That(jaehrlicheMehreinnahmen, Is.EqualTo(500.00m));
        Assert.That(amortisationInMonaten, Is.EqualTo(20.4m).Within(0.1m)); // Ca. 20 Monate

        // Dokumentation
        var roiAnalyse = $@"
ROI-ANALYSE:
Investition: {kosten:C}
Jährliche Mehreinnahmen: {jaehrlicheMehreinnahmen:C}
Amortisation: {amortisationInMonaten:F1} Monate
ROI nach 2 Jahren: {((jaehrlicheMehreinnahmen * 2 - kosten) / kosten * 100):F1}%";

        _fortbildung.ROI_Analyse = roiAnalyse;
        Assert.That(_fortbildung.ROI_Analyse, Contains.Substring("ROI-ANALYSE"));
    }

    [Test]
    public void Fortbildung_ShouldSupportFollowUpPlanning()
    {
        // Arrange - Nachfolgemaßnahmen
        var nachfolgeMassnahmen = @"
NACHFOLGEMASSNAHMEN:

KURZFRISTIG (1-3 Monate):
- Erste Patienten mit neuer Technik behandeln
- Behandlungsergebnisse dokumentieren
- Feedback von Patienten einholen

MITTELFRISTIG (3-6 Monate):
- Aufbaukurs ""Craniosacrale Osteopathie - Fortgeschrittene""
- Supervision bei erfahrenem Kollegen
- Fallbesprechungen im Team

LANGFRISTIG (6-12 Monate):
- Spezialisierung auf bestimmte Patientengruppen
- Eigene Fortbildungen für Kollegen anbieten
- Wissenschaftliche Publikation anstreben

NÄCHSTE FORTBILDUNGEN:
- Viszerale Osteopathie (geplant für Herbst 2024)
- Kinderosteopathie (geplant für 2025)";

        // Act
        _fortbildung.NachfolgeMassnahmen = nachfolgeMassnahmen;

        // Assert
        Assert.That(_fortbildung.NachfolgeMassnahmen, Contains.Substring("NACHFOLGEMASSNAHMEN"));
        Assert.That(_fortbildung.NachfolgeMassnahmen, Contains.Substring("KURZFRISTIG"));
        Assert.That(_fortbildung.NachfolgeMassnahmen, Contains.Substring("MITTELFRISTIG"));
        Assert.That(_fortbildung.NachfolgeMassnahmen, Contains.Substring("LANGFRISTIG"));
        Assert.That(_fortbildung.NachfolgeMassnahmen, Contains.Substring("NÄCHSTE FORTBILDUNGEN"));
    }
} 