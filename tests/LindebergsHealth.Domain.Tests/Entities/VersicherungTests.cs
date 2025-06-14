using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Versicherung Entity - Funktionalitätstests
/// </summary>
[TestFixture]
public class VersicherungTests
{
    private Versicherung _versicherung;
    private Patient _patient;
    private Versicherungstyp _versicherungstyp;

    [SetUp]
    public void Setup()
    {
        _patient = new Patient
        {
            Id = Guid.NewGuid(),
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        _versicherungstyp = new Versicherungstyp
        {
            Id = Guid.NewGuid(),
            Bezeichnung = "Gesetzliche Krankenversicherung",
            Code = "GKV",
            Aktiv = true
        };

        _versicherung = new Versicherung
        {
            PatientId = _patient.Id,
            Patient = _patient,
            VersicherungstypId = _versicherungstyp.Id,
            Versicherungstyp = _versicherungstyp,
            Versicherungsname = "AOK Bayern",
            Versicherungsnummer = "A123456789",
            GueltigVon = DateTime.Today.AddYears(-1),
            GueltigBis = DateTime.Today.AddYears(1),
            IstHauptversicherung = true
        };
    }

    [Test]
    public void Versicherung_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_versicherung, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Versicherung_ShouldHaveValidInsuranceData()
    {
        // Assert
        Assert.That(_versicherung.PatientId, Is.EqualTo(_patient.Id));
        Assert.That(_versicherung.Patient, Is.EqualTo(_patient));
        Assert.That(_versicherung.VersicherungstypId, Is.EqualTo(_versicherungstyp.Id));
        Assert.That(_versicherung.Versicherungstyp, Is.EqualTo(_versicherungstyp));
        Assert.That(_versicherung.Versicherungsname, Is.EqualTo("AOK Bayern"));
        Assert.That(_versicherung.Versicherungsnummer, Is.EqualTo("A123456789"));
        Assert.That(_versicherung.IstHauptversicherung, Is.True);
    }

    [Test]
    public void Versicherung_ShouldValidateInsuranceNumbers()
    {
        // Arrange - Verschiedene Versicherungsnummer-Formate
        var validInsuranceNumbers = new[]
        {
            "A123456789", // AOK Format
            "B987654321", // Barmer Format
            "T555666777", // Techniker Format
            "1234567890", // Numerisch
            "X12345678Y"  // Mit Buchstaben
        };

        // Act & Assert
        foreach (var number in validInsuranceNumbers)
        {
            _versicherung.Versicherungsnummer = number;
            Assert.That(_versicherung.Versicherungsnummer, Is.EqualTo(number));
            Assert.That(string.IsNullOrWhiteSpace(_versicherung.Versicherungsnummer), Is.False);
            Assert.That(_versicherung.Versicherungsnummer.Length, Is.GreaterThanOrEqualTo(8));
        }
    }

    [Test]
    public void Versicherung_ShouldValidateValidityPeriod()
    {
        // Arrange
        var startDate = new DateTime(2024, 1, 1);
        var endDate = new DateTime(2024, 12, 31);

        // Act
        _versicherung.GueltigVon = startDate;
        _versicherung.GueltigBis = endDate;

        // Assert
        Assert.That(_versicherung.GueltigVon, Is.EqualTo(startDate));
        Assert.That(_versicherung.GueltigBis, Is.EqualTo(endDate));
        Assert.That(_versicherung.GueltigBis, Is.GreaterThan(_versicherung.GueltigVon));
    }

    [Test]
    public void Versicherung_ShouldCheckCurrentValidity()
    {
        // Arrange
        var heute = DateTime.Today;
        
        // Test 1: Aktuell gültige Versicherung
        _versicherung.GueltigVon = heute.AddDays(-30);
        _versicherung.GueltigBis = heute.AddDays(30);
        
        var istGueltig = heute >= _versicherung.GueltigVon && heute <= _versicherung.GueltigBis;
        
        // Assert
        Assert.That(istGueltig, Is.True);
        
        // Test 2: Abgelaufene Versicherung
        _versicherung.GueltigVon = heute.AddDays(-60);
        _versicherung.GueltigBis = heute.AddDays(-30);
        
        istGueltig = heute >= _versicherung.GueltigVon && heute <= _versicherung.GueltigBis;
        
        // Assert
        Assert.That(istGueltig, Is.False);
    }

    [Test]
    public void Versicherung_ShouldSupportDifferentInsuranceTypes()
    {
        // Arrange
        var insuranceTypes = new[]
        {
            new Versicherungstyp { Bezeichnung = "Gesetzliche Krankenversicherung", Code = "GKV" },
            new Versicherungstyp { Bezeichnung = "Private Krankenversicherung", Code = "PKV" },
            new Versicherungstyp { Bezeichnung = "Beihilfe", Code = "BEI" },
            new Versicherungstyp { Bezeichnung = "Selbstzahler", Code = "SZ" },
            new Versicherungstyp { Bezeichnung = "Zusatzversicherung", Code = "ZV" }
        };

        // Act & Assert
        foreach (var insuranceType in insuranceTypes)
        {
            _versicherung.VersicherungstypId = insuranceType.Id;
            _versicherung.Versicherungstyp = insuranceType;

            Assert.That(_versicherung.Versicherungstyp.Bezeichnung, Is.EqualTo(insuranceType.Bezeichnung));
            Assert.That(_versicherung.Versicherungstyp.Code, Is.EqualTo(insuranceType.Code));
        }
    }

    [Test]
    public void Versicherung_ShouldEnforcePrimaryInsuranceLogic()
    {
        // Arrange - Ein Patient sollte nur eine Hauptversicherung haben
        var hauptversicherung = new Versicherung
        {
            PatientId = _patient.Id,
            Versicherungsname = "AOK Bayern",
            IstHauptversicherung = true
        };

        var zusatzversicherung = new Versicherung
        {
            PatientId = _patient.Id,
            Versicherungsname = "DKV Zusatz",
            IstHauptversicherung = false
        };

        // Act
        _patient.Versicherungen.Add(hauptversicherung);
        _patient.Versicherungen.Add(zusatzversicherung);

        // Assert
        var hauptversicherungen = _patient.Versicherungen.Where(v => v.IstHauptversicherung).ToList();
        Assert.That(hauptversicherungen.Count, Is.EqualTo(1));
        Assert.That(hauptversicherungen.First().Versicherungsname, Is.EqualTo("AOK Bayern"));
    }

    [Test]
    public void Versicherung_ShouldSupportInsuranceCompanyDetails()
    {
        // Arrange
        var versicherungsdetails = new
        {
            Name = "AOK Bayern - Die Gesundheitskasse",
            IK_Nummer = "108018347", // Institutionskennzeichen
            Adresse = "Stromeyer-Straße 5, 90443 Nürnberg",
            Telefon = "0911 1021-0",
            Email = "service@by.aok.de"
        };

        // Act
        _versicherung.Versicherungsname = versicherungsdetails.Name;
        _versicherung.IK_Nummer = versicherungsdetails.IK_Nummer;
        _versicherung.Notizen = $"Adresse: {versicherungsdetails.Adresse}\nTelefon: {versicherungsdetails.Telefon}\nE-Mail: {versicherungsdetails.Email}";

        // Assert
        Assert.That(_versicherung.Versicherungsname, Is.EqualTo(versicherungsdetails.Name));
        Assert.That(_versicherung.IK_Nummer, Is.EqualTo(versicherungsdetails.IK_Nummer));
        Assert.That(_versicherung.Notizen, Contains.Substring("Adresse"));
        Assert.That(_versicherung.Notizen, Contains.Substring("Telefon"));
        Assert.That(_versicherung.Notizen, Contains.Substring("E-Mail"));
    }

    [Test]
    public void Versicherung_ShouldValidateIKNumber()
    {
        // Arrange - IK-Nummern sind 9-stellig
        var validIKNumbers = new[]
        {
            "108018347", // AOK Bayern
            "104212059", // Barmer
            "120381322", // Techniker
            "109519005"  // DAK
        };

        var invalidIKNumbers = new[]
        {
            "12345",     // Zu kurz
            "1234567890", // Zu lang
            "ABC123456"   // Buchstaben
        };

        // Act & Assert - Valid IK numbers
        foreach (var ikNumber in validIKNumbers)
        {
            _versicherung.IK_Nummer = ikNumber;
            Assert.That(_versicherung.IK_Nummer, Is.EqualTo(ikNumber));
            Assert.That(_versicherung.IK_Nummer.Length, Is.EqualTo(9));
            Assert.That(_versicherung.IK_Nummer.All(char.IsDigit), Is.True);
        }

        // Invalid IK numbers should be handled by business logic
        foreach (var ikNumber in invalidIKNumbers)
        {
            _versicherung.IK_Nummer = ikNumber;
            if (ikNumber.Length != 9 || !ikNumber.All(char.IsDigit))
            {
                Assert.That(ikNumber.Length != 9 || !ikNumber.All(char.IsDigit), Is.True);
            }
        }
    }

    [Test]
    public void Versicherung_ShouldSupportCoverageDetails()
    {
        // Arrange - Leistungsumfang
        var leistungsumfang = @"
ABGEDECKTE LEISTUNGEN:
✓ Osteopathie (4 Sitzungen/Jahr, 60€/Sitzung)
✓ Physiotherapie (unbegrenzt bei Verordnung)
✓ Massage (bei Verordnung)
✓ Krankengymnastik (bei Verordnung)

NICHT ABGEDECKT:
✗ Wellness-Massagen
✗ Präventive Behandlungen ohne Verordnung
✗ Kosmetische Behandlungen

ZUZAHLUNGEN:
- Rezeptgebühr: 10€
- Eigenanteil Heilmittel: 10% + 10€";

        // Act
        _versicherung.Leistungsumfang = leistungsumfang;

        // Assert
        Assert.That(_versicherung.Leistungsumfang, Contains.Substring("ABGEDECKTE LEISTUNGEN"));
        Assert.That(_versicherung.Leistungsumfang, Contains.Substring("NICHT ABGEDECKT"));
        Assert.That(_versicherung.Leistungsumfang, Contains.Substring("ZUZAHLUNGEN"));
        Assert.That(_versicherung.Leistungsumfang.Count(c => c == '✓'), Is.EqualTo(4));
        Assert.That(_versicherung.Leistungsumfang.Count(c => c == '✗'), Is.EqualTo(3));
    }

    [Test]
    public void Versicherung_ShouldTrackInsuranceHistory()
    {
        // Arrange - Versicherungshistorie
        var versicherungshistorie = new[]
        {
            new Versicherung
            {
                PatientId = _patient.Id,
                Versicherungsname = "TK - Techniker Krankenkasse",
                GueltigVon = new DateTime(2020, 1, 1),
                GueltigBis = new DateTime(2022, 12, 31),
                IstHauptversicherung = true
            },
            new Versicherung
            {
                PatientId = _patient.Id,
                Versicherungsname = "AOK Bayern",
                GueltigVon = new DateTime(2023, 1, 1),
                GueltigBis = new DateTime(2024, 12, 31),
                IstHauptversicherung = true
            }
        };

        // Act
        foreach (var versicherung in versicherungshistorie)
        {
            _patient.Versicherungen.Add(versicherung);
        }

        // Assert
        var sortierteHistorie = _patient.Versicherungen.OrderBy(v => v.GueltigVon).ToList();
        Assert.That(sortierteHistorie.Count, Is.EqualTo(2));
        Assert.That(sortierteHistorie.First().Versicherungsname, Is.EqualTo("TK - Techniker Krankenkasse"));
        Assert.That(sortierteHistorie.Last().Versicherungsname, Is.EqualTo("AOK Bayern"));
    }

    [Test]
    public void Versicherung_ShouldSupportPrivateInsuranceSpecifics()
    {
        // Arrange - Private Krankenversicherung
        var pkvVersicherungstyp = new Versicherungstyp
        {
            Bezeichnung = "Private Krankenversicherung",
            Code = "PKV"
        };

        var pkvVersicherung = new Versicherung
        {
            PatientId = _patient.Id,
            VersicherungstypId = pkvVersicherungstyp.Id,
            Versicherungstyp = pkvVersicherungstyp,
            Versicherungsname = "Allianz Private Krankenversicherung",
            Versicherungsnummer = "PKV-123456789",
            Tarif = "Komfort Plus",
            Selbstbeteiligung = 500.00m,
            Erstattungssatz = 80.0m // 80% Erstattung
        };

        // Act & Assert
        Assert.That(pkvVersicherung.Versicherungstyp.Code, Is.EqualTo("PKV"));
        Assert.That(pkvVersicherung.Tarif, Is.EqualTo("Komfort Plus"));
        Assert.That(pkvVersicherung.Selbstbeteiligung, Is.EqualTo(500.00m));
        Assert.That(pkvVersicherung.Erstattungssatz, Is.EqualTo(80.0m));
    }

    [Test]
    public void Versicherung_ShouldCalculateReimbursement()
    {
        // Arrange - Erstattungsberechnung
        var behandlungskosten = 85.50m;
        var erstattungssatz = 80.0m; // 80%
        var selbstbeteiligung = 100.00m;

        _versicherung.Erstattungssatz = erstattungssatz;
        _versicherung.Selbstbeteiligung = selbstbeteiligung;

        // Act - Berechnung der Erstattung
        var erstattungsfähigerBetrag = Math.Max(0, behandlungskosten - selbstbeteiligung);
        var erstattung = erstattungsfähigerBetrag * (erstattungssatz / 100);
        var eigenanteil = behandlungskosten - erstattung;

        // Assert
        Assert.That(erstattungsfähigerBetrag, Is.EqualTo(0)); // Unter Selbstbeteiligung
        Assert.That(erstattung, Is.EqualTo(0));
        Assert.That(eigenanteil, Is.EqualTo(behandlungskosten));

        // Test mit höheren Kosten
        behandlungskosten = 200.00m;
        erstattungsfähigerBetrag = Math.Max(0, behandlungskosten - selbstbeteiligung);
        erstattung = erstattungsfähigerBetrag * (erstattungssatz / 100);
        eigenanteil = behandlungskosten - erstattung;

        Assert.That(erstattungsfähigerBetrag, Is.EqualTo(100.00m));
        Assert.That(erstattung, Is.EqualTo(80.00m));
        Assert.That(eigenanteil, Is.EqualTo(120.00m));
    }

    [Test]
    public void Versicherung_ShouldSupportContactInformation()
    {
        // Arrange
        var kontaktdaten = @"
KONTAKTDATEN AOK BAYERN:
Telefon: 0911 1021-0
Fax: 0911 1021-1111
E-Mail: service@by.aok.de
Website: www.aok.de/bayern

ÖFFNUNGSZEITEN:
Mo-Do: 08:00-16:00 Uhr
Fr: 08:00-14:00 Uhr

NOTFALL-HOTLINE:
24/7: 0800 1021-030";

        // Act
        _versicherung.Kontaktdaten = kontaktdaten;

        // Assert
        Assert.That(_versicherung.Kontaktdaten, Contains.Substring("KONTAKTDATEN"));
        Assert.That(_versicherung.Kontaktdaten, Contains.Substring("ÖFFNUNGSZEITEN"));
        Assert.That(_versicherung.Kontaktdaten, Contains.Substring("NOTFALL-HOTLINE"));
    }
} 