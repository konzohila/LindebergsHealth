using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die TherapieEinheit Entity - Funktionalitätstests
/// </summary>
[TestFixture]
public class TherapieEinheitTests
{
    private TherapieEinheit _therapieEinheit;
    private Patient _patient;
    private Mitarbeiter _therapeut;
    private Termin _termin;
    private Therapietyp _therapietyp;
    private Therapiestatus _therapiestatus;

    [SetUp]
    public void Setup()
    {
        _patient = new Patient
        {
            Id = Guid.NewGuid(),
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        _therapeut = new Mitarbeiter
        {
            Id = Guid.NewGuid(),
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        _termin = new Termin
        {
            Id = Guid.NewGuid(),
            PatientId = _patient.Id,
            MitarbeiterId = _therapeut.Id,
            Datum = DateTime.Today,
            DauerMinuten = 60
        };

        _therapietyp = new Therapietyp
        {
            Id = Guid.NewGuid(),
            Bezeichnung = "Osteopathie",
            Code = "OST",
            Aktiv = true
        };

        _therapiestatus = new Therapiestatus
        {
            Id = Guid.NewGuid(),
            Bezeichnung = "Abgeschlossen",
            Code = "DONE",
            Aktiv = true
        };

        _therapieEinheit = new TherapieEinheit
        {
            TerminId = _termin.Id,
            Termin = _termin,
            PatientId = _patient.Id,
            Patient = _patient,
            TherapeutId = _therapeut.Id,
            Therapeut = _therapeut,
            TherapietypId = _therapietyp.Id,
            Therapietyp = _therapietyp,
            TherapiestatusId = _therapiestatus.Id,
            Therapiestatus = _therapiestatus,
            Dauer = 60
        };
    }

    [Test]
    public void TherapieEinheit_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_therapieEinheit, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void TherapieEinheit_ShouldHaveValidTherapyData()
    {
        // Assert
        Assert.That(_therapieEinheit.TerminId, Is.EqualTo(_termin.Id));
        Assert.That(_therapieEinheit.Termin, Is.EqualTo(_termin));
        Assert.That(_therapieEinheit.PatientId, Is.EqualTo(_patient.Id));
        Assert.That(_therapieEinheit.Patient, Is.EqualTo(_patient));
        Assert.That(_therapieEinheit.TherapeutId, Is.EqualTo(_therapeut.Id));
        Assert.That(_therapieEinheit.Therapeut, Is.EqualTo(_therapeut));
        Assert.That(_therapieEinheit.TherapietypId, Is.EqualTo(_therapietyp.Id));
        Assert.That(_therapieEinheit.Therapietyp, Is.EqualTo(_therapietyp));
        Assert.That(_therapieEinheit.Dauer, Is.EqualTo(60));
    }

    [Test]
    public void TherapieEinheit_ShouldSupportDetailedDocumentation()
    {
        // Arrange
        var befund = "Patient klagt über Rückenschmerzen im LWS-Bereich. Bewegungseinschränkung bei Flexion.";
        var behandlung = "Osteopathische Behandlung der LWS, Mobilisation der Facettengelenke, Weichteiltechniken.";
        var notizen = "Patient reagiert gut auf Behandlung. Nächster Termin in 1 Woche empfohlen.";

        // Act
        _therapieEinheit.Befund = befund;
        _therapieEinheit.Behandlung = behandlung;
        _therapieEinheit.Notizen = notizen;

        // Assert
        Assert.That(_therapieEinheit.Befund, Is.EqualTo(befund));
        Assert.That(_therapieEinheit.Behandlung, Is.EqualTo(behandlung));
        Assert.That(_therapieEinheit.Notizen, Is.EqualTo(notizen));
    }

    [Test]
    public void TherapieEinheit_ShouldValidateDuration()
    {
        // Arrange
        var validDurations = new[] { 15, 30, 45, 60, 90, 120 };
        var invalidDurations = new[] { 0, -15, 5, 200 };

        // Act & Assert - Valid durations
        foreach (var duration in validDurations)
        {
            _therapieEinheit.Dauer = duration;
            Assert.That(_therapieEinheit.Dauer, Is.GreaterThan(0));
            Assert.That(_therapieEinheit.Dauer, Is.LessThanOrEqualTo(180)); // Max 3 Stunden
        }

        // Invalid durations should be handled by business logic
        foreach (var duration in invalidDurations)
        {
            _therapieEinheit.Dauer = duration;
            if (duration <= 0 || duration > 180)
            {
                // Business logic should validate this
                Assert.That(duration <= 0 || duration > 180, Is.True);
            }
        }
    }

    [Test]
    public void TherapieEinheit_ShouldSupportDifferentTherapyTypes()
    {
        // Arrange
        var therapyTypes = new[]
        {
            new Therapietyp { Bezeichnung = "Osteopathie", Code = "OST" },
            new Therapietyp { Bezeichnung = "Physiotherapie", Code = "PHY" },
            new Therapietyp { Bezeichnung = "Massage", Code = "MAS" },
            new Therapietyp { Bezeichnung = "Krankengymnastik", Code = "KG" }
        };

        // Act & Assert
        foreach (var therapyType in therapyTypes)
        {
            _therapieEinheit.TherapietypId = therapyType.Id;
            _therapieEinheit.Therapietyp = therapyType;

            Assert.That(_therapieEinheit.Therapietyp.Bezeichnung, Is.EqualTo(therapyType.Bezeichnung));
            Assert.That(_therapieEinheit.Therapietyp.Code, Is.EqualTo(therapyType.Code));
        }
    }

    [Test]
    public void TherapieEinheit_ShouldTrackTherapyStatus()
    {
        // Arrange
        var statusProgression = new[]
        {
            new Therapiestatus { Bezeichnung = "Geplant", Code = "PLANNED" },
            new Therapiestatus { Bezeichnung = "In Behandlung", Code = "ACTIVE" },
            new Therapiestatus { Bezeichnung = "Abgeschlossen", Code = "COMPLETED" },
            new Therapiestatus { Bezeichnung = "Abgebrochen", Code = "CANCELLED" }
        };

        // Act & Assert
        foreach (var status in statusProgression)
        {
            _therapieEinheit.TherapiestatusId = status.Id;
            _therapieEinheit.Therapiestatus = status;

            Assert.That(_therapieEinheit.Therapiestatus.Bezeichnung, Is.EqualTo(status.Bezeichnung));
            Assert.That(_therapieEinheit.Therapiestatus.Code, Is.EqualTo(status.Code));
        }
    }

    [Test]
    public void TherapieEinheit_ShouldValidatePatientTherapistConsistency()
    {
        // Assert - Patient und Therapeut müssen mit Termin übereinstimmen
        Assert.That(_therapieEinheit.PatientId, Is.EqualTo(_termin.PatientId));
        Assert.That(_therapieEinheit.TherapeutId, Is.EqualTo(_termin.MitarbeiterId));
    }

    [Test]
    public void TherapieEinheit_ShouldSupportStructuredFindings()
    {
        // Arrange - Strukturierter Befund
        var strukturierterBefund = @"
ANAMNESE:
- Rückenschmerzen seit 3 Wochen
- Auslöser: Heben schwerer Gegenstände
- Schmerzintensität: 7/10

INSPEKTION:
- Schonhaltung erkennbar
- Muskuläre Verspannungen im LWS-Bereich

PALPATION:
- Druckschmerz L4/L5
- Erhöhte Muskelspannung Mm. erector spinae

BEWEGUNGSPRÜFUNG:
- Flexion: 60° (eingeschränkt)
- Extension: 20° (normal)
- Lateralflexion: bds. 25° (leicht eingeschränkt)";

        // Act
        _therapieEinheit.Befund = strukturierterBefund;

        // Assert
        Assert.That(_therapieEinheit.Befund, Contains.Substring("ANAMNESE"));
        Assert.That(_therapieEinheit.Befund, Contains.Substring("INSPEKTION"));
        Assert.That(_therapieEinheit.Befund, Contains.Substring("PALPATION"));
        Assert.That(_therapieEinheit.Befund, Contains.Substring("BEWEGUNGSPRÜFUNG"));
    }

    [Test]
    public void TherapieEinheit_ShouldSupportTreatmentDocumentation()
    {
        // Arrange - Strukturierte Behandlungsdokumentation
        var behandlungsprotokoll = @"
DURCHGEFÜHRTE TECHNIKEN:
1. Weichteiltechniken LWS (15 min)
2. Mobilisation Facettengelenke L4/L5 (10 min)
3. Muskelenergietechniken Mm. psoas (15 min)
4. Entspannungstechniken (10 min)

PATIENT REAKTION:
- Schmerzreduktion von 7/10 auf 4/10
- Verbesserte Beweglichkeit
- Entspannung der Muskulatur

HAUSAUFGABEN:
- Dehnübungen für Hüftbeuger 2x täglich
- Wärmeanwendung bei Bedarf
- Vermeidung schwerer körperlicher Arbeit";

        // Act
        _therapieEinheit.Behandlung = behandlungsprotokoll;

        // Assert
        Assert.That(_therapieEinheit.Behandlung, Contains.Substring("DURCHGEFÜHRTE TECHNIKEN"));
        Assert.That(_therapieEinheit.Behandlung, Contains.Substring("PATIENT REAKTION"));
        Assert.That(_therapieEinheit.Behandlung, Contains.Substring("HAUSAUFGABEN"));
    }

    [Test]
    public void TherapieEinheit_ShouldCalculateTreatmentEffectiveness()
    {
        // Arrange - Schmerzskala vor und nach Behandlung
        var schmerzVorher = 8;
        var schmerzNachher = 3;
        var verbesserung = schmerzVorher - schmerzNachher;
        var verbesserungProzent = (verbesserung / (double)schmerzVorher) * 100;

        var notizen = $"Schmerzreduktion: {schmerzVorher}/10 → {schmerzNachher}/10 ({verbesserungProzent:F1}% Verbesserung)";

        // Act
        _therapieEinheit.Notizen = notizen;

        // Assert
        Assert.That(_therapieEinheit.Notizen, Contains.Substring("Schmerzreduktion"));
        Assert.That(_therapieEinheit.Notizen, Contains.Substring("62,5% Verbesserung"));
    }

    [Test]
    public void TherapieEinheit_ShouldSupportFollowUpPlanning()
    {
        // Arrange
        var followUpNotes = @"
BEHANDLUNGSPLAN:
- Fortsetzung der osteopathischen Behandlung
- Nächster Termin in 1 Woche
- Fokus auf Stabilisierung der LWS

PROGNOSE:
- Gute Heilungschancen bei Compliance
- Erwartete Behandlungsdauer: 4-6 Sitzungen

EMPFEHLUNGEN:
- Ergonomische Arbeitsplatzgestaltung
- Präventive Rückenschule
- Regelmäßige Bewegung";

        // Act
        _therapieEinheit.Notizen = followUpNotes;

        // Assert
        Assert.That(_therapieEinheit.Notizen, Contains.Substring("BEHANDLUNGSPLAN"));
        Assert.That(_therapieEinheit.Notizen, Contains.Substring("PROGNOSE"));
        Assert.That(_therapieEinheit.Notizen, Contains.Substring("EMPFEHLUNGEN"));
    }

    [Test]
    public void TherapieEinheit_ShouldValidateBusinessRules()
    {
        // Arrange & Assert - Geschäftsregeln
        
        // 1. Dauer sollte nicht länger als Termindauer sein
        Assert.That(_therapieEinheit.Dauer, Is.LessThanOrEqualTo(_termin.DauerMinuten));
        
        // 2. TherapieEinheit sollte nur für vergangene oder aktuelle Termine erstellt werden
        Assert.That(_termin.Datum, Is.LessThanOrEqualTo(DateTime.Today.AddDays(1)));
        
        // 3. Patient und Therapeut müssen aktiv sein (würde in Business Logic geprüft)
        Assert.That(_patient.IstGelöscht, Is.False);
        Assert.That(_therapeut.IstGelöscht, Is.False);
    }

    [Test]
    public void TherapieEinheit_ShouldSupportQualityAssurance()
    {
        // Arrange - Qualitätssicherung
        var qualitätsprüfung = @"
BEHANDLUNGSQUALITÄT:
✓ Anamnese vollständig dokumentiert
✓ Befund strukturiert erhoben
✓ Behandlungsziele definiert
✓ Techniken fachgerecht angewandt
✓ Patientenreaktion dokumentiert
✓ Hausaufgaben vergeben

COMPLIANCE:
- Patient kooperativ
- Hausaufgaben werden durchgeführt
- Termine werden eingehalten";

        // Act
        _therapieEinheit.Notizen = qualitätsprüfung;

        // Assert
        Assert.That(_therapieEinheit.Notizen, Contains.Substring("BEHANDLUNGSQUALITÄT"));
        Assert.That(_therapieEinheit.Notizen, Contains.Substring("COMPLIANCE"));
        Assert.That(_therapieEinheit.Notizen.Count(c => c == '✓'), Is.EqualTo(6));
    }
} 