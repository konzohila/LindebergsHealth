using Moq;
using FluentAssertions;
using LindebergsHealth.Application.Services;
using LindebergsHealth.Application.Interfaces;
using LindebergsHealth.Domain.Entities;
using LindebergsHealth.Domain.Interfaces;

namespace LindebergsHealth.Application.Tests.Services;

/// <summary>
/// Tests für den PatientService
/// </summary>
[TestFixture]
public class PatientServiceTests
{
    private Mock<IPatientRepository> _mockPatientRepository;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private PatientService _patientService;

    [SetUp]
    public void Setup()
    {
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _patientService = new PatientService(_mockPatientRepository.Object, _mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetPatientByIdAsync_WithValidId_ShouldReturnPatient()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var expectedPatient = new Patient
        {
            Id = patientId,
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        _mockPatientRepository
            .Setup(r => r.GetByIdAsync(patientId))
            .ReturnsAsync(expectedPatient);

        // Act
        var result = await _patientService.GetPatientByIdAsync(patientId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(patientId);
        result.Vorname.Should().Be("Max");
        result.Nachname.Should().Be("Mustermann");

        _mockPatientRepository.Verify(r => r.GetByIdAsync(patientId), Times.Once);
    }

    [Test]
    public async Task GetPatientByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        _mockPatientRepository
            .Setup(r => r.GetByIdAsync(patientId))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _patientService.GetPatientByIdAsync(patientId);

        // Assert
        result.Should().BeNull();

        _mockPatientRepository.Verify(r => r.GetByIdAsync(patientId), Times.Once);
    }

    [Test]
    public async Task GetAllPatientsAsync_ShouldReturnAllPatients()
    {
        // Arrange
        var expectedPatients = new List<Patient>
        {
            new() { Vorname = "Max", Nachname = "Mustermann" },
            new() { Vorname = "Anna", Nachname = "Schmidt" }
        };

        _mockPatientRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(expectedPatients);

        // Act
        var result = await _patientService.GetAllPatientsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedPatients);

        _mockPatientRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task CreatePatientAsync_WithValidPatient_ShouldCreateAndReturnPatient()
    {
        // Arrange
        var newPatient = new Patient
        {
            Vorname = "New",
            Nachname = "Patient",
            Geburtsdatum = new DateTime(1990, 5, 15),
            Geschlecht = "M"
        };

        _mockPatientRepository
            .Setup(r => r.AddAsync(It.IsAny<Patient>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _patientService.CreatePatientAsync(newPatient);

        // Assert
        result.Should().NotBeNull();
        result.Vorname.Should().Be("New");
        result.Nachname.Should().Be("Patient");
        result.Id.Should().NotBe(Guid.Empty);

        _mockPatientRepository.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdatePatientAsync_WithValidPatient_ShouldUpdatePatient()
    {
        // Arrange
        var existingPatient = new Patient
        {
            Id = Guid.NewGuid(),
            Vorname = "Original",
            Nachname = "Patient"
        };

        var updatedPatient = new Patient
        {
            Id = existingPatient.Id,
            Vorname = "Updated",
            Nachname = "Patient"
        };

        _mockPatientRepository
            .Setup(r => r.GetByIdAsync(existingPatient.Id))
            .ReturnsAsync(existingPatient);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _patientService.UpdatePatientAsync(updatedPatient);

        // Assert
        result.Should().NotBeNull();
        result!.Vorname.Should().Be("Updated");

        _mockPatientRepository.Verify(r => r.GetByIdAsync(existingPatient.Id), Times.Once);
        _mockPatientRepository.Verify(r => r.Update(It.IsAny<Patient>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task UpdatePatientAsync_WithNonExistingPatient_ShouldReturnNull()
    {
        // Arrange
        var nonExistingPatient = new Patient
        {
            Id = Guid.NewGuid(),
            Vorname = "NonExisting",
            Nachname = "Patient"
        };

        _mockPatientRepository
            .Setup(r => r.GetByIdAsync(nonExistingPatient.Id))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _patientService.UpdatePatientAsync(nonExistingPatient);

        // Assert
        result.Should().BeNull();

        _mockPatientRepository.Verify(r => r.GetByIdAsync(nonExistingPatient.Id), Times.Once);
        _mockPatientRepository.Verify(r => r.Update(It.IsAny<Patient>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public async Task DeletePatientAsync_WithValidId_ShouldDeletePatient()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var existingPatient = new Patient
        {
            Id = patientId,
            Vorname = "ToDelete",
            Nachname = "Patient"
        };

        _mockPatientRepository
            .Setup(r => r.GetByIdAsync(patientId))
            .ReturnsAsync(existingPatient);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _patientService.DeletePatientAsync(patientId);

        // Assert
        result.Should().BeTrue();

        _mockPatientRepository.Verify(r => r.GetByIdAsync(patientId), Times.Once);
        _mockPatientRepository.Verify(r => r.DeleteAsync(patientId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task DeletePatientAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        _mockPatientRepository
            .Setup(r => r.GetByIdAsync(patientId))
            .ReturnsAsync((Patient?)null);

        // Act
        var result = await _patientService.DeletePatientAsync(patientId);

        // Assert
        result.Should().BeFalse();

        _mockPatientRepository.Verify(r => r.GetByIdAsync(patientId), Times.Once);
        _mockPatientRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public async Task SearchPatientsAsync_WithValidSearchTerm_ShouldReturnMatchingPatients()
    {
        // Arrange
        var searchTerm = "Max";
        var expectedPatients = new List<Patient>
        {
            new() { Vorname = "Max", Nachname = "Mustermann" },
            new() { Vorname = "Maximilian", Nachname = "Schmidt" }
        };

        _mockPatientRepository
            .Setup(r => r.SearchAsync(searchTerm))
            .ReturnsAsync(expectedPatients);

        // Act
        var result = await _patientService.SearchPatientsAsync(searchTerm);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedPatients);

        _mockPatientRepository.Verify(r => r.SearchAsync(searchTerm), Times.Once);
    }

    [Test]
    public async Task GetPatientByPatientenNummerAsync_WithValidNumber_ShouldReturnPatient()
    {
        // Arrange
        var patientenNummer = "P123456";
        var expectedPatient = new Patient
        {
            PatientenNummer = patientenNummer,
            Vorname = "Test",
            Nachname = "Patient"
        };

        _mockPatientRepository
            .Setup(r => r.GetByPatientenNummerAsync(patientenNummer))
            .ReturnsAsync(expectedPatient);

        // Act
        var result = await _patientService.GetPatientByPatientenNummerAsync(patientenNummer);

        // Assert
        result.Should().NotBeNull();
        result!.PatientenNummer.Should().Be(patientenNummer);

        _mockPatientRepository.Verify(r => r.GetByPatientenNummerAsync(patientenNummer), Times.Once);
    }

    [Test]
    public async Task DeactivatePatientAsync_WithValidId_ShouldDeactivatePatient()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reason = "Patient verzogen";
        
        var existingPatient = new Patient
        {
            Id = patientId,
            Vorname = "Test",
            Nachname = "Patient",
            IstAktiv = true
        };

        _mockPatientRepository
            .Setup(r => r.GetByIdAsync(patientId))
            .ReturnsAsync(existingPatient);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _patientService.DeactivatePatientAsync(patientId, userId, reason);

        // Assert
        result.Should().BeTrue();
        existingPatient.IstAktiv.Should().BeFalse();
        existingPatient.DeaktivierungsGrund.Should().Be(reason);

        _mockPatientRepository.Verify(r => r.GetByIdAsync(patientId), Times.Once);
        _mockPatientRepository.Verify(r => r.Update(existingPatient), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task ActivatePatientAsync_WithValidId_ShouldActivatePatient()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        var existingPatient = new Patient
        {
            Id = patientId,
            Vorname = "Test",
            Nachname = "Patient",
            IstAktiv = false
        };

        _mockPatientRepository
            .Setup(r => r.GetByIdAsync(patientId))
            .ReturnsAsync(existingPatient);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _patientService.ActivatePatientAsync(patientId, userId);

        // Assert
        result.Should().BeTrue();
        existingPatient.IstAktiv.Should().BeTrue();

        _mockPatientRepository.Verify(r => r.GetByIdAsync(patientId), Times.Once);
        _mockPatientRepository.Verify(r => r.Update(existingPatient), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task GetActivePatientCountAsync_ShouldReturnCount()
    {
        // Arrange
        var expectedCount = 42;

        _mockPatientRepository
            .Setup(r => r.GetActiveCountAsync())
            .ReturnsAsync(expectedCount);

        // Act
        var result = await _patientService.GetActivePatientCountAsync();

        // Assert
        result.Should().Be(expectedCount);

        _mockPatientRepository.Verify(r => r.GetActiveCountAsync(), Times.Once);
    }

    [Test]
    public async Task PatientExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        _mockPatientRepository
            .Setup(r => r.ExistsAsync(patientId))
            .ReturnsAsync(true);

        // Act
        var result = await _patientService.PatientExistsAsync(patientId);

        // Assert
        result.Should().BeTrue();

        _mockPatientRepository.Verify(r => r.ExistsAsync(patientId), Times.Once);
    }

    [Test]
    public async Task GetPatientWithTermineAsync_WithValidId_ShouldReturnPatientWithTermine()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var expectedPatient = new Patient
        {
            Id = patientId,
            Vorname = "Test",
            Nachname = "Patient",
            Termine = new List<Termin>
            {
                new() { Titel = "Termin 1" },
                new() { Titel = "Termin 2" }
            }
        };

        _mockPatientRepository
            .Setup(r => r.GetWithTermineAsync(patientId))
            .ReturnsAsync(expectedPatient);

        // Act
        var result = await _patientService.GetPatientWithTermineAsync(patientId);

        // Assert
        result.Should().NotBeNull();
        result!.Termine.Should().HaveCount(2);

        _mockPatientRepository.Verify(r => r.GetWithTermineAsync(patientId), Times.Once);
    }
} 