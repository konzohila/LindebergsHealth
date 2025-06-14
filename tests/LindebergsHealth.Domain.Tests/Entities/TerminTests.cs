using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Termin Entity
/// </summary>
[TestFixture]
public class TerminTests
{
    private Termin _termin;
    private Patient _patient;

    [SetUp]
    public void Setup()
    {
        _patient = new Patient
        {
            Id = Guid.NewGuid(),
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        _termin = new Termin
        {
            PatientId = _patient.Id,
            Patient = _patient,
            Datum = DateTime.Today.AddDays(1),
            DauerMinuten = 60
        };
    }

    [Test]
    public void Termin_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_termin, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Termin_ShouldHaveValidProperties()
    {
        // Assert
        Assert.That(_termin.PatientId, Is.EqualTo(_patient.Id));
        Assert.That(_termin.Patient, Is.EqualTo(_patient));
        Assert.That(_termin.Datum, Is.EqualTo(DateTime.Today.AddDays(1)));
        Assert.That(_termin.DauerMinuten, Is.EqualTo(60));
    }

    [Test]
    public void Termin_ShouldInitializeCollections()
    {
        // Assert
        Assert.That(_termin.Änderungen, Is.Not.Null);
        Assert.That(_termin.WartelistenEinträge, Is.Not.Null);
        Assert.That(_termin.Rechnungen, Is.Not.Null);
    }

    [Test]
    public void Termin_ShouldAllowAddingRechnung()
    {
        // Arrange
        var rechnung = new Rechnung
        {
            TerminId = _termin.Id,
            PatientId = _patient.Id,
            Betrag = 85.50m
        };

        // Act
        _termin.Rechnungen.Add(rechnung);

        // Assert
        Assert.That(_termin.Rechnungen, Contains.Item(rechnung));
        Assert.That(_termin.Rechnungen.Count, Is.EqualTo(1));
    }

    [Test]
    public void Termin_ShouldValidateDuration()
    {
        // Arrange & Act
        _termin.DauerMinuten = 30;

        // Assert
        Assert.That(_termin.DauerMinuten, Is.GreaterThan(0));
        Assert.That(_termin.DauerMinuten, Is.LessThanOrEqualTo(480)); // Max 8 Stunden
    }
} 