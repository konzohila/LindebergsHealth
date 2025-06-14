using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Domain.Tests.Entities;

/// <summary>
/// Tests für die BaseEntity Klasse
/// </summary>
[TestFixture]
public class BaseEntityTests
{
    private TestEntity _entity;
    private readonly Guid _testUserId = Guid.NewGuid();

    [SetUp]
    public void Setup()
    {
        _entity = new TestEntity();
    }

    [Test]
    public void Constructor_ShouldInitializeWithDefaults()
    {
        // Arrange & Act
        var entity = new TestEntity();

        // Assert
        Assert.That(entity.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(entity.ErstelltAm, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromSeconds(1)));
        Assert.That(entity.IstGelöscht, Is.False);
        Assert.That(entity.RowVersion, Is.Not.Null);
        Assert.That(entity.RowVersion.Length, Is.EqualTo(0));
    }

    [Test]
    public void SoftDelete_ShouldMarkAsDeleted()
    {
        // Arrange
        var grund = "Test-Löschung";

        // Act
        _entity.SoftDelete(_testUserId, grund);

        // Assert
        Assert.That(_entity.IstGelöscht, Is.True);
        Assert.That(_entity.GelöschtVon, Is.EqualTo(_testUserId));
        Assert.That(_entity.GelöschtAm, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromSeconds(1)));
        Assert.That(_entity.LöschGrund, Is.EqualTo(grund));
    }

    [Test]
    public void SoftDelete_WithoutReason_ShouldUseEmptyReason()
    {
        // Act
        _entity.SoftDelete(_testUserId);

        // Assert
        Assert.That(_entity.IstGelöscht, Is.True);
        Assert.That(_entity.LöschGrund, Is.EqualTo(""));
    }

    [Test]
    public void SoftDelete_WhenAlreadyDeleted_ShouldAllowMultipleDeletions()
    {
        // Arrange
        _entity.SoftDelete(_testUserId, "Erste Löschung");

        // Act & Assert - Should not throw, just update the deletion info
        Assert.DoesNotThrow(() => _entity.SoftDelete(_testUserId, "Zweite Löschung"));
        Assert.That(_entity.LöschGrund, Is.EqualTo("Zweite Löschung"));
    }

    [Test]
    public void MarkAsModified_ShouldUpdateModificationFields()
    {
        // Act
        _entity.MarkAsModified(_testUserId);

        // Assert
        Assert.That(_entity.GeändertVon, Is.EqualTo(_testUserId));
        Assert.That(_entity.GeändertAm, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromSeconds(1)));
    }

    [Test]
    public void Id_ShouldBeUniqueForEachInstance()
    {
        // Arrange & Act
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        // Assert
        Assert.That(entity1.Id, Is.Not.EqualTo(entity2.Id));
    }

    [Test]
    public void ErstelltAm_ShouldBeSetOnCreation()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var entity = new TestEntity();
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.That(entity.ErstelltAm, Is.GreaterThanOrEqualTo(beforeCreation));
        Assert.That(entity.ErstelltAm, Is.LessThanOrEqualTo(afterCreation));
    }
}

/// <summary>
/// Test-Entity für BaseEntity Tests
/// </summary>
public class TestEntity : BaseEntity
{
    public string TestProperty { get; set; } = string.Empty;
} 