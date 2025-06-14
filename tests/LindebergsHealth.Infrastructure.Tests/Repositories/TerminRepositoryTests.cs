using Microsoft.EntityFrameworkCore;
using LindebergsHealth.Infrastructure.Data;
using LindebergsHealth.Infrastructure.Repositories;
using LindebergsHealth.Domain.Entities;
using FluentAssertions;

namespace LindebergsHealth.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests für das TerminRepository
/// </summary>
[TestFixture]
public class TerminRepositoryTests
{
    private LindebergsHealthDbContext _context;
    private TerminRepository _repository;
    private DbContextOptions<LindebergsHealthDbContext> _options;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<LindebergsHealthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new LindebergsHealthDbContext(_options);
        _context.Database.EnsureCreated();
        _repository = new TerminRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task AddAsync_ShouldAddTerminToDatabase()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.SaveChangesAsync();

        var termin = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1),
            Startzeit = new TimeSpan(10, 0, 0),
            Endzeit = new TimeSpan(11, 0, 0),
            DauerMinuten = 60,
            Notizen = "Erste Therapiesitzung"
        };

        // Act
        await _repository.AddAsync(termin);
        await _context.SaveChangesAsync();

        // Assert
        var savedTermin = await _context.Termine.FindAsync(termin.Id);
        savedTermin.Should().NotBeNull();
        savedTermin.PatientId.Should().Be(patient.Id);
        savedTermin.TherapeutId.Should().Be(therapeut.Id);
        savedTermin.DauerMinuten.Should().Be(60);
        savedTermin.Notizen.Should().Be("Erste Therapiesitzung");
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnTerminWithRelatedData()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var termin = new Termin
        {
            PatientId = patient.Id,
            Patient = patient,
            TherapeutId = therapeut.Id,
            Therapeut = therapeut,
            TermintypId = termintyp.Id,
            Termintyp = termintyp,
            TerminstatusId = terminstatus.Id,
            Terminstatus = terminstatus,
            Datum = DateTime.Today.AddDays(1),
            Startzeit = new TimeSpan(10, 0, 0),
            Endzeit = new TimeSpan(11, 0, 0),
            DauerMinuten = 60
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.Termine.AddAsync(termin);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(termin.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(termin.Id);
        result.Patient.Should().NotBeNull();
        result.Patient.Vorname.Should().Be("Max");
        result.Therapeut.Should().NotBeNull();
        result.Therapeut.Vorname.Should().Be("Dr. Anna");
        result.Termintyp.Should().NotBeNull();
        result.Termintyp.Bezeichnung.Should().Be("Therapietermin");
        result.Terminstatus.Should().NotBeNull();
        result.Terminstatus.Bezeichnung.Should().Be("Geplant");
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllTermine()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var termin1 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1),
            Startzeit = new TimeSpan(10, 0, 0),
            Endzeit = new TimeSpan(11, 0, 0)
        };

        var termin2 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(2),
            Startzeit = new TimeSpan(14, 0, 0),
            Endzeit = new TimeSpan(15, 0, 0)
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.Termine.AddRangeAsync(termin1, termin2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Id == termin1.Id);
        result.Should().Contain(t => t.Id == termin2.Id);
    }

    [Test]
    public async Task GetTermineByPatientIdAsync_ShouldReturnPatientTermine()
    {
        // Arrange
        var patient1 = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var patient2 = new Patient
        {
            Vorname = "Anna",
            Nachname = "Schmidt"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Peter",
            Nachname = "Müller"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var termin1 = new Termin
        {
            PatientId = patient1.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1)
        };

        var termin2 = new Termin
        {
            PatientId = patient1.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(2)
        };

        var termin3 = new Termin
        {
            PatientId = patient2.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(3)
        };

        await _context.Patienten.AddRangeAsync(patient1, patient2);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.Termine.AddRangeAsync(termin1, termin2, termin3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTermineByPatientIdAsync(patient1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Id == termin1.Id);
        result.Should().Contain(t => t.Id == termin2.Id);
        result.Should().NotContain(t => t.Id == termin3.Id);
    }

    [Test]
    public async Task GetTermineByTherapeutIdAsync_ShouldReturnTherapeutTermine()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut1 = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var therapeut2 = new Mitarbeiter
        {
            Vorname = "Dr. Peter",
            Nachname = "Müller"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var termin1 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut1.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1)
        };

        var termin2 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut1.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(2)
        };

        var termin3 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut2.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(3)
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddRangeAsync(therapeut1, therapeut2);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.Termine.AddRangeAsync(termin1, termin2, termin3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTermineByTherapeutIdAsync(therapeut1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Id == termin1.Id);
        result.Should().Contain(t => t.Id == termin2.Id);
        result.Should().NotContain(t => t.Id == termin3.Id);
    }

    [Test]
    public async Task GetTermineByDateRangeAsync_ShouldReturnTermineInRange()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var termin1 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1) // In Range
        };

        var termin2 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(5) // In Range
        };

        var termin3 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(10) // Out of Range
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.Termine.AddRangeAsync(termin1, termin2, termin3);
        await _context.SaveChangesAsync();

        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(7);

        // Act
        var result = await _repository.GetTermineByDateRangeAsync(startDate, endDate);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Id == termin1.Id);
        result.Should().Contain(t => t.Id == termin2.Id);
        result.Should().NotContain(t => t.Id == termin3.Id);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateTermin()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var termin = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1),
            Startzeit = new TimeSpan(10, 0, 0),
            Endzeit = new TimeSpan(11, 0, 0),
            DauerMinuten = 60,
            Notizen = "Ursprüngliche Notiz"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.Termine.AddAsync(termin);
        await _context.SaveChangesAsync();

        // Act
        termin.Notizen = "Aktualisierte Notiz";
        termin.DauerMinuten = 90;
        _repository.Update(termin);
        await _context.SaveChangesAsync();

        // Assert
        var updatedTermin = await _context.Termine.FindAsync(termin.Id);
        updatedTermin.Should().NotBeNull();
        updatedTermin.Notizen.Should().Be("Aktualisierte Notiz");
        updatedTermin.DauerMinuten.Should().Be(90);
    }

    [Test]
    public async Task DeleteAsync_ShouldSoftDeleteTermin()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var termin = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1)
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.Termine.AddAsync(termin);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(termin.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedTermin = await _context.Termine.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == termin.Id);
        deletedTermin.Should().NotBeNull();
        deletedTermin.IstGelöscht.Should().BeTrue();
        deletedTermin.GelöschtAm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        // Verify it's not returned in normal queries
        var activeTermine = await _repository.GetAllAsync();
        activeTermine.Should().NotContain(t => t.Id == termin.Id);
    }

    [Test]
    public async Task GetTermineByStatusAsync_ShouldReturnTermineWithSpecificStatus()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var statusGeplant = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var statusAbgeschlossen = new Terminstatus
        {
            Bezeichnung = "Abgeschlossen",
            Code = "ABGESCHLOSSEN"
        };

        var termin1 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = statusGeplant.Id,
            Datum = DateTime.Today.AddDays(1)
        };

        var termin2 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = statusGeplant.Id,
            Datum = DateTime.Today.AddDays(2)
        };

        var termin3 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = statusAbgeschlossen.Id,
            Datum = DateTime.Today.AddDays(-1)
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddRangeAsync(statusGeplant, statusAbgeschlossen);
        await _context.Termine.AddRangeAsync(termin1, termin2, termin3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTermineByStatusAsync(statusGeplant.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Id == termin1.Id);
        result.Should().Contain(t => t.Id == termin2.Id);
        result.Should().NotContain(t => t.Id == termin3.Id);
    }

    [Test]
    public async Task GetKonfliktTermineAsync_ShouldReturnOverlappingTermine()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt"
        };

        var termintyp = new Termintyp
        {
            Bezeichnung = "Therapietermin",
            Code = "THERAPIE"
        };

        var terminstatus = new Terminstatus
        {
            Bezeichnung = "Geplant",
            Code = "GEPLANT"
        };

        var datum = DateTime.Today.AddDays(1);

        var termin1 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = datum,
            Startzeit = new TimeSpan(10, 0, 0),
            Endzeit = new TimeSpan(11, 0, 0)
        };

        var termin2 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = datum,
            Startzeit = new TimeSpan(10, 30, 0), // Überschneidung
            Endzeit = new TimeSpan(11, 30, 0)
        };

        var termin3 = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = datum,
            Startzeit = new TimeSpan(14, 0, 0), // Keine Überschneidung
            Endzeit = new TimeSpan(15, 0, 0)
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Mitarbeiter.AddAsync(therapeut);
        await _context.Termintypen.AddAsync(termintyp);
        await _context.Terminstatus.AddAsync(terminstatus);
        await _context.Termine.AddRangeAsync(termin1, termin2, termin3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetKonfliktTermineAsync(
            therapeut.Id, 
            datum, 
            new TimeSpan(10, 15, 0), 
            new TimeSpan(10, 45, 0));

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Id == termin1.Id);
        result.Should().Contain(t => t.Id == termin2.Id);
        result.Should().NotContain(t => t.Id == termin3.Id);
    }
} 