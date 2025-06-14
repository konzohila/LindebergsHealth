using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Patient Entity
/// </summary>
[TestFixture]
public class PatientTests
{
    private Patient _patient;

    [SetUp]
    public void Setup()
    {
        _patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            Geburtsdatum = new DateTime(1990, 5, 15),
            GeschlechtId = Guid.NewGuid()
        };
    }

    [Test]
    public void Patient_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_patient, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Patient_ShouldHaveValidProperties()
    {
        // Assert
        Assert.That(_patient.Vorname, Is.EqualTo("Max"));
        Assert.That(_patient.Nachname, Is.EqualTo("Mustermann"));
        Assert.That(_patient.Geburtsdatum, Is.EqualTo(new DateTime(1990, 5, 15)));
        Assert.That(_patient.GeschlechtId, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void Patient_ShouldInitializeCollections()
    {
        // Assert
        Assert.That(_patient.Termine, Is.Not.Null);
        Assert.That(_patient.Adressen, Is.Not.Null);
        Assert.That(_patient.Kontakte, Is.Not.Null);
        Assert.That(_patient.Rechnungen, Is.Not.Null);
        Assert.That(_patient.Versicherungen, Is.Not.Null);
        Assert.That(_patient.Beziehungspersonen, Is.Not.Null);
        Assert.That(_patient.CRMNetzwerke, Is.Not.Null);
        Assert.That(_patient.Koerperstatuseintraege, Is.Not.Null);
        Assert.That(_patient.Therapieserien, Is.Not.Null);
        Assert.That(_patient.Dokumente, Is.Not.Null);
        Assert.That(_patient.Einwilligungen, Is.Not.Null);
        Assert.That(_patient.Kommunikationsverlaeufe, Is.Not.Null);
    }

    [Test]
    public void Patient_ShouldAllowAddingTermin()
    {
        // Arrange
        var termin = new Termin
        {
            PatientId = _patient.Id,
            Datum = DateTime.Today.AddDays(1),
            DauerMinuten = 60
        };

        // Act
        _patient.Termine.Add(termin);

        // Assert
        Assert.That(_patient.Termine, Contains.Item(termin));
        Assert.That(_patient.Termine.Count, Is.EqualTo(1));
    }

    [Test]
    public void Patient_ShouldAllowSoftDelete()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        _patient.SoftDelete(userId, "Test-Löschung");

        // Assert
        Assert.That(_patient.IstGelöscht, Is.True);
        Assert.That(_patient.GelöschtVon, Is.EqualTo(userId));
        Assert.That(_patient.LöschGrund, Is.EqualTo("Test-Löschung"));
    }
} 