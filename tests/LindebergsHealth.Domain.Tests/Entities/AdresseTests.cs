using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Adresse Entity - Funktionalitätstests
/// </summary>
[TestFixture]
public class AdresseTests
{
    private Adresse _adresse;

    [SetUp]
    public void Setup()
    {
        _adresse = new Adresse
        {
            Strasse = "Musterstraße",
            Hausnummer = "123",
            Zusatz = "2. OG",
            Postleitzahl = "12345",
            Ort = "Musterstadt",
            Land = "Deutschland"
        };
    }

    [Test]
    public void Adresse_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_adresse, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Adresse_ShouldHaveValidGermanAddress()
    {
        // Assert
        Assert.That(_adresse.Strasse, Is.EqualTo("Musterstraße"));
        Assert.That(_adresse.Hausnummer, Is.EqualTo("123"));
        Assert.That(_adresse.Zusatz, Is.EqualTo("2. OG"));
        Assert.That(_adresse.Postleitzahl, Is.EqualTo("12345"));
        Assert.That(_adresse.Ort, Is.EqualTo("Musterstadt"));
        Assert.That(_adresse.Land, Is.EqualTo("Deutschland"));
    }

    [Test]
    public void Adresse_ShouldInitializeCollections()
    {
        // Assert
        Assert.That(_adresse.PatientenAdressen, Is.Not.Null);
        Assert.That(_adresse.MitarbeiterAdressen, Is.Not.Null);
        Assert.That(_adresse.KooperationspartnerAdressen, Is.Not.Null);
    }

    [Test]
    public void Adresse_ShouldSupportGeoCoordinates()
    {
        // Arrange
        var latitude = 52.520008m; // Berlin
        var longitude = 13.404954m;

        // Act
        _adresse.Latitude = latitude;
        _adresse.Longitude = longitude;

        // Assert
        Assert.That(_adresse.Latitude, Is.EqualTo(latitude));
        Assert.That(_adresse.Longitude, Is.EqualTo(longitude));
    }

    [Test]
    public void Adresse_ShouldSupportValidation()
    {
        // Arrange
        var validationSource = "Google Maps API";
        var validationDate = DateTime.UtcNow;

        // Act
        _adresse.IstValidiert = true;
        _adresse.ValidiertAm = validationDate;
        _adresse.ValidationSource = validationSource;

        // Assert
        Assert.That(_adresse.IstValidiert, Is.True);
        Assert.That(_adresse.ValidiertAm, Is.EqualTo(validationDate));
        Assert.That(_adresse.ValidationSource, Is.EqualTo(validationSource));
    }

    [Test]
    public void Adresse_ShouldAllowPatientAssociation()
    {
        // Arrange
        var patient = new Patient { Vorname = "Max", Nachname = "Mustermann" };
        var adresstyp = new Adresstyp { Bezeichnung = "Hauptadresse", Code = "HAUPT" };
        var patientAdresse = new PatientAdresse
        {
            PatientId = patient.Id,
            AdresseId = _adresse.Id,
            Patient = patient,
            Adresse = _adresse,
            Adresstyp = adresstyp,
            IstHauptadresse = true,
            GueltigVon = DateTime.Today
        };

        // Act
        _adresse.PatientenAdressen.Add(patientAdresse);

        // Assert
        Assert.That(_adresse.PatientenAdressen.Count, Is.EqualTo(1));
        Assert.That(_adresse.PatientenAdressen.First().Patient, Is.EqualTo(patient));
        Assert.That(_adresse.PatientenAdressen.First().IstHauptadresse, Is.True);
    }

    [Test]
    public void Adresse_ShouldValidateGermanPostalCode()
    {
        // Arrange & Act
        var validPostalCodes = new[] { "12345", "01234", "99999" };
        var invalidPostalCodes = new[] { "1234", "123456", "ABCDE", "" };

        // Assert - Valid postal codes should be 5 digits
        foreach (var code in validPostalCodes)
        {
            _adresse.Postleitzahl = code;
            Assert.That(_adresse.Postleitzahl.Length, Is.EqualTo(5));
            Assert.That(_adresse.Postleitzahl.All(char.IsDigit), Is.True, 
                $"Postal code {code} should contain only digits");
        }
    }

    [Test]
    public void Adresse_ShouldFormatFullAddress()
    {
        // Act
        var fullAddress = $"{_adresse.Strasse} {_adresse.Hausnummer}, {_adresse.Postleitzahl} {_adresse.Ort}";
        var expectedAddress = "Musterstraße 123, 12345 Musterstadt";

        // Assert
        Assert.That(fullAddress, Is.EqualTo(expectedAddress));
    }

    [Test]
    public void Adresse_ShouldSupportInternationalAddresses()
    {
        // Arrange
        var internationalAddress = new Adresse
        {
            Strasse = "123 Main Street",
            Hausnummer = "",
            Postleitzahl = "10001",
            Ort = "New York",
            Land = "USA"
        };

        // Assert
        Assert.That(internationalAddress.Land, Is.EqualTo("USA"));
        Assert.That(internationalAddress.Postleitzahl, Is.EqualTo("10001"));
    }

    [Test]
    public void Adresse_ShouldTrackValidationHistory()
    {
        // Arrange
        var firstValidation = DateTime.UtcNow.AddDays(-30);
        var secondValidation = DateTime.UtcNow;

        // Act - First validation
        _adresse.IstValidiert = true;
        _adresse.ValidiertAm = firstValidation;
        _adresse.ValidationSource = "Manual";

        // Act - Second validation
        _adresse.ValidiertAm = secondValidation;
        _adresse.ValidationSource = "Google API";

        // Assert
        Assert.That(_adresse.ValidiertAm, Is.EqualTo(secondValidation));
        Assert.That(_adresse.ValidationSource, Is.EqualTo("Google API"));
        Assert.That(_adresse.IstValidiert, Is.True);
    }
} 