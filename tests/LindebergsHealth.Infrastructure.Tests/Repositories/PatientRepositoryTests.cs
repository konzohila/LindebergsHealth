using Microsoft.EntityFrameworkCore;
using LindebergsHealth.Infrastructure.Data;
using LindebergsHealth.Infrastructure.Repositories;
using LindebergsHealth.Domain.Entities;
using FluentAssertions;

namespace LindebergsHealth.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests für das PatientRepository
/// </summary>
[TestFixture]
public class PatientRepositoryTests
{
    private LindebergsHealthDbContext _context;
    private PatientRepository _repository;
    private DbContextOptions<LindebergsHealthDbContext> _options;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<LindebergsHealthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new LindebergsHealthDbContext(_options);
        _context.Database.EnsureCreated();
        _repository = new PatientRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task GetByIdAsync_WithValidId_ShouldReturnPatient()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            Geburtsdatum = new DateTime(1990, 5, 15),
            Geschlecht = "M"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(patient.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Vorname.Should().Be("Max");
        result.Nachname.Should().Be("Mustermann");
    }

    [Test]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllActivePatients()
    {
        // Arrange
        var activePatient = new Patient
        {
            Vorname = "Active",
            Nachname = "Patient",
            IstAktiv = true
        };

        var inactivePatient = new Patient
        {
            Vorname = "Inactive",
            Nachname = "Patient",
            IstAktiv = false
        };

        await _context.Patienten.AddRangeAsync(activePatient, inactivePatient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Vorname.Should().Be("Active");
    }

    [Test]
    public async Task AddAsync_ShouldAddPatientToDatabase()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "New",
            Nachname = "Patient",
            Geburtsdatum = new DateTime(1985, 3, 20),
            Geschlecht = "W"
        };

        // Act
        await _repository.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Assert
        var savedPatient = await _context.Patienten.FirstOrDefaultAsync(p => p.Id == patient.Id);
        savedPatient.Should().NotBeNull();
        savedPatient!.Vorname.Should().Be("New");
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdatePatientInDatabase()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Original",
            Nachname = "Patient"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        patient.Vorname = "Updated";
        _repository.Update(patient);
        await _context.SaveChangesAsync();

        // Assert
        var updatedPatient = await _context.Patienten.FirstOrDefaultAsync(p => p.Id == patient.Id);
        updatedPatient.Should().NotBeNull();
        updatedPatient!.Vorname.Should().Be("Updated");
    }

    [Test]
    public async Task DeleteAsync_ShouldSoftDeletePatient()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "ToDelete",
            Nachname = "Patient"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(patient.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedPatient = await _context.Patienten.FirstOrDefaultAsync(p => p.Id == patient.Id);
        deletedPatient.Should().BeNull(); // Should be filtered out by query filter

        // Verify it's soft deleted, not hard deleted
        var allPatients = await _context.Patienten.IgnoreQueryFilters().ToListAsync();
        var softDeletedPatient = allPatients.FirstOrDefault(p => p.Id == patient.Id);
        softDeletedPatient.Should().NotBeNull();
        softDeletedPatient!.IstGelöscht.Should().BeTrue();
    }

    [Test]
    public async Task GetByPatientenNummerAsync_WithValidNumber_ShouldReturnPatient()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Test",
            Nachname = "Patient"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByPatientenNummerAsync(patient.PatientenNummer);

        // Assert
        result.Should().NotBeNull();
        result!.PatientenNummer.Should().Be(patient.PatientenNummer);
    }

    [Test]
    public async Task GetByPatientenNummerAsync_WithInvalidNumber_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByPatientenNummerAsync("INVALID123");

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task SearchAsync_ByName_ShouldReturnMatchingPatients()
    {
        // Arrange
        var patients = new[]
        {
            new Patient { Vorname = "Max", Nachname = "Mustermann" },
            new Patient { Vorname = "Anna", Nachname = "Schmidt" },
            new Patient { Vorname = "Peter", Nachname = "Müller" }
        };

        await _context.Patienten.AddRangeAsync(patients);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Max");

        // Assert
        result.Should().HaveCount(1);
        result.First().Vorname.Should().Be("Max");
    }

    [Test]
    public async Task SearchAsync_ByLastName_ShouldReturnMatchingPatients()
    {
        // Arrange
        var patients = new[]
        {
            new Patient { Vorname = "Max", Nachname = "Mustermann" },
            new Patient { Vorname = "Anna", Nachname = "Mustermann" },
            new Patient { Vorname = "Peter", Nachname = "Müller" }
        };

        await _context.Patienten.AddRangeAsync(patients);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("Mustermann");

        // Assert
        result.Should().HaveCount(2);
        result.All(p => p.Nachname == "Mustermann").Should().BeTrue();
    }

    [Test]
    public async Task GetWithTermineAsync_ShouldIncludeTermine()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Test",
            Nachname = "Patient"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Test",
            Nachname = "Therapeut",
            Email = "test@test.com"
        };

        var termin = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            Datum = DateTime.Today.AddDays(1),
            Uhrzeit = TimeSpan.FromHours(10),
            Dauer = TimeSpan.FromMinutes(30),
            Titel = "Test Termin"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termine.AddAsync(termin);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetWithTermineAsync(patient.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Termine.Should().HaveCount(1);
        result.Termine.First().Titel.Should().Be("Test Termin");
    }

    [Test]
    public async Task GetWithTherapienAsync_ShouldIncludeTherapien()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Test",
            Nachname = "Patient"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Test",
            Nachname = "Therapeut",
            Email = "test@test.com"
        };

        var therapie = new Therapie
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            Bezeichnung = "Test Therapie",
            StartDatum = DateTime.Today
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Therapien.AddAsync(therapie);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetWithTherapienAsync(patient.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Therapien.Should().HaveCount(1);
        result.Therapien.First().Bezeichnung.Should().Be("Test Therapie");
    }

    [Test]
    public async Task GetByGeburtsdatumAsync_ShouldReturnPatientsWithMatchingBirthdate()
    {
        // Arrange
        var birthdate = new DateTime(1990, 5, 15);
        var patients = new[]
        {
            new Patient { Vorname = "Max", Nachname = "Mustermann", Geburtsdatum = birthdate },
            new Patient { Vorname = "Anna", Nachname = "Schmidt", Geburtsdatum = birthdate },
            new Patient { Vorname = "Peter", Nachname = "Müller", Geburtsdatum = new DateTime(1985, 3, 20) }
        };

        await _context.Patienten.AddRangeAsync(patients);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByGeburtsdatumAsync(birthdate);

        // Assert
        result.Should().HaveCount(2);
        result.All(p => p.Geburtsdatum == birthdate).Should().BeTrue();
    }

    [Test]
    public async Task GetActiveCountAsync_ShouldReturnCountOfActivePatients()
    {
        // Arrange
        var patients = new[]
        {
            new Patient { Vorname = "Active1", Nachname = "Patient", IstAktiv = true },
            new Patient { Vorname = "Active2", Nachname = "Patient", IstAktiv = true },
            new Patient { Vorname = "Inactive", Nachname = "Patient", IstAktiv = false }
        };

        await _context.Patienten.AddRangeAsync(patients);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetActiveCountAsync();

        // Assert
        result.Should().Be(2);
    }

    [Test]
    public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Test",
            Nachname = "Patient"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ExistsAsync(patient.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.ExistsAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }
} 