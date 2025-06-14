using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die Mitarbeiter Entity
/// </summary>
[TestFixture]
public class MitarbeiterTests
{
    private Mitarbeiter _mitarbeiter;

    [SetUp]
    public void Setup()
    {
        _mitarbeiter = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };
    }

    [Test]
    public void Mitarbeiter_ShouldInheritFromBaseEntity()
    {
        // Assert
        Assert.That(_mitarbeiter, Is.InstanceOf<BaseEntity>());
    }

    [Test]
    public void Mitarbeiter_ShouldHaveValidProperties()
    {
        // Assert
        Assert.That(_mitarbeiter.Vorname, Is.EqualTo("Dr. Anna"));
        Assert.That(_mitarbeiter.Nachname, Is.EqualTo("Schmidt"));
    }

    [Test]
    public void Mitarbeiter_ShouldInitializeCollections()
    {
        // Assert
        Assert.That(_mitarbeiter.Termine, Is.Not.Null);
        Assert.That(_mitarbeiter.Fortbildungen, Is.Not.Null);
        Assert.That(_mitarbeiter.Adressen, Is.Not.Null);
        Assert.That(_mitarbeiter.Kontakte, Is.Not.Null);
        Assert.That(_mitarbeiter.Gehaelter, Is.Not.Null);
    }

    [Test]
    public void Mitarbeiter_ShouldAllowAddingTermin()
    {
        // Arrange
        var termin = new Termin
        {
            MitarbeiterId = _mitarbeiter.Id,
            Datum = DateTime.Today.AddDays(1),
            DauerMinuten = 60
        };

        // Act
        _mitarbeiter.Termine.Add(termin);

        // Assert
        Assert.That(_mitarbeiter.Termine, Contains.Item(termin));
        Assert.That(_mitarbeiter.Termine.Count, Is.EqualTo(1));
    }

    [Test]
    public void Mitarbeiter_ShouldAllowSoftDelete()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        _mitarbeiter.SoftDelete(userId, "Test-Löschung");

        // Assert
        Assert.That(_mitarbeiter.IstGelöscht, Is.True);
        Assert.That(_mitarbeiter.GelöschtVon, Is.EqualTo(userId));
        Assert.That(_mitarbeiter.LöschGrund, Is.EqualTo("Test-Löschung"));
    }
} 