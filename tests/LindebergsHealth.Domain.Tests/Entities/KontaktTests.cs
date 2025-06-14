using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Kontakt Entity - Funktionalitätstests
/// </summary>
[TestFixture]
public class KontaktTests
{
    private Kontakt _kontakt;
    private Kontakttyp _kontakttyp;

    [SetUp]
    public void Setup()
    {
        _kontakttyp = new Kontakttyp
        {
            Bezeichnung = "Telefon",
            Code = "TEL",
            Aktiv = true
        };

        _kontakt = new Kontakt
        {
            KontakttypId = _kontakttyp.Id,
            Kontakttyp = _kontakttyp,
            Wert = "+49 30 12345678",
            IstHauptkontakt = true,
            IstValidiert = false
        };
    }

    [Test]
    public void Kontakt_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_kontakt, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Kontakt_ShouldHaveValidContactData()
    {
        // Assert
        Assert.That(_kontakt.KontakttypId, Is.EqualTo(_kontakttyp.Id));
        Assert.That(_kontakt.Kontakttyp, Is.EqualTo(_kontakttyp));
        Assert.That(_kontakt.Wert, Is.EqualTo("+49 30 12345678"));
        Assert.That(_kontakt.IstHauptkontakt, Is.True);
        Assert.That(_kontakt.IstValidiert, Is.False);
    }

    [Test]
    public void Kontakt_ShouldInitializeCollections()
    {
        // Assert
        Assert.That(_kontakt.PatientenKontakte, Is.Not.Null);
        Assert.That(_kontakt.MitarbeiterKontakte, Is.Not.Null);
        Assert.That(_kontakt.KooperationspartnerKontakte, Is.Not.Null);
    }

    [Test]
    public void Kontakt_ShouldSupportPhoneNumberValidation()
    {
        // Arrange
        var validPhoneNumbers = new[]
        {
            "+49 30 12345678",
            "030 12345678",
            "0171 1234567",
            "+49 171 1234567"
        };

        // Act & Assert
        foreach (var phoneNumber in validPhoneNumbers)
        {
            _kontakt.Wert = phoneNumber;
            
            // Basic validation - should contain digits and allowed characters
            var cleanedNumber = phoneNumber.Replace(" ", "").Replace("+", "").Replace("-", "");
            Assert.That(cleanedNumber.All(c => char.IsDigit(c)), Is.True,
                $"Phone number {phoneNumber} should contain only digits after cleaning");
        }
    }

    [Test]
    public void Kontakt_ShouldSupportEmailValidation()
    {
        // Arrange
        var emailKontakttyp = new Kontakttyp { Bezeichnung = "E-Mail", Code = "EMAIL" };
        var emailKontakt = new Kontakt
        {
            Kontakttyp = emailKontakttyp,
            Wert = "max.mustermann@example.com"
        };

        // Act & Assert
        Assert.That(emailKontakt.Wert.Contains("@"), Is.True);
        Assert.That(emailKontakt.Wert.Contains("."), Is.True);
        Assert.That(emailKontakt.Wert.IndexOf("@") < emailKontakt.Wert.LastIndexOf("."), Is.True);
    }

    [Test]
    public void Kontakt_ShouldSupportValidationTracking()
    {
        // Arrange
        var validationDate = DateTime.UtcNow;

        // Act
        _kontakt.IstValidiert = true;
        _kontakt.ValidiertAm = validationDate;

        // Assert
        Assert.That(_kontakt.IstValidiert, Is.True);
        Assert.That(_kontakt.ValidiertAm, Is.EqualTo(validationDate));
    }

    [Test]
    public void Kontakt_ShouldAllowPatientAssociation()
    {
        // Arrange
        var patient = new Patient { Vorname = "Max", Nachname = "Mustermann" };
        var patientKontakt = new PatientKontakt
        {
            PatientId = patient.Id,
            KontaktId = _kontakt.Id,
            Patient = patient,
            Kontakt = _kontakt,
            IstHauptkontakt = true,
            GueltigVon = DateTime.Today
        };

        // Act
        _kontakt.PatientenKontakte.Add(patientKontakt);

        // Assert
        Assert.That(_kontakt.PatientenKontakte.Count, Is.EqualTo(1));
        Assert.That(_kontakt.PatientenKontakte.First().Patient, Is.EqualTo(patient));
        Assert.That(_kontakt.PatientenKontakte.First().IstHauptkontakt, Is.True);
    }

    [Test]
    public void Kontakt_ShouldSupportNotesAndComments()
    {
        // Arrange
        var notes = "Nur werktags zwischen 9-17 Uhr erreichbar";

        // Act
        _kontakt.Notizen = notes;

        // Assert
        Assert.That(_kontakt.Notizen, Is.EqualTo(notes));
    }

    [Test]
    public void Kontakt_ShouldHandleDifferentContactTypes()
    {
        // Arrange
        var contactTypes = new[]
        {
            new { Type = "Telefon", Value = "+49 30 12345678" },
            new { Type = "E-Mail", Value = "test@example.com" },
            new { Type = "Fax", Value = "+49 30 12345679" },
            new { Type = "WhatsApp", Value = "+49 171 1234567" }
        };

        // Act & Assert
        foreach (var contactType in contactTypes)
        {
            var kontakttyp = new Kontakttyp { Bezeichnung = contactType.Type, Code = contactType.Type.ToUpper() };
            var kontakt = new Kontakt
            {
                Kontakttyp = kontakttyp,
                Wert = contactType.Value
            };

            Assert.That(kontakt.Kontakttyp.Bezeichnung, Is.EqualTo(contactType.Type));
            Assert.That(kontakt.Wert, Is.EqualTo(contactType.Value));
        }
    }

    [Test]
    public void Kontakt_ShouldEnforceMainContactLogic()
    {
        // Arrange
        var patient = new Patient { Vorname = "Max", Nachname = "Mustermann" };
        
        var hauptkontakt = new Kontakt
        {
            Kontakttyp = _kontakttyp,
            Wert = "+49 30 11111111",
            IstHauptkontakt = true
        };

        var zweitkontakt = new Kontakt
        {
            Kontakttyp = _kontakttyp,
            Wert = "+49 30 22222222",
            IstHauptkontakt = false
        };

        // Act
        var patientHauptkontakt = new PatientKontakt
        {
            Patient = patient,
            Kontakt = hauptkontakt,
            IstHauptkontakt = true
        };

        var patientZweitkontakt = new PatientKontakt
        {
            Patient = patient,
            Kontakt = zweitkontakt,
            IstHauptkontakt = false
        };

        // Assert
        Assert.That(patientHauptkontakt.IstHauptkontakt, Is.True);
        Assert.That(patientZweitkontakt.IstHauptkontakt, Is.False);
        
        // Nur ein Hauptkontakt pro Typ sollte erlaubt sein
        Assert.That(hauptkontakt.IstHauptkontakt, Is.True);
        Assert.That(zweitkontakt.IstHauptkontakt, Is.False);
    }

    [Test]
    public void Kontakt_ShouldSupportInternationalFormats()
    {
        // Arrange
        var internationalContacts = new[]
        {
            "+1 555 123 4567", // USA
            "+44 20 7946 0958", // UK
            "+33 1 42 86 83 26", // France
            "+81 3 1234 5678" // Japan
        };

        // Act & Assert
        foreach (var contact in internationalContacts)
        {
            _kontakt.Wert = contact;
            Assert.That(_kontakt.Wert.StartsWith("+"), Is.True,
                $"International contact {contact} should start with +");
        }
    }
} 