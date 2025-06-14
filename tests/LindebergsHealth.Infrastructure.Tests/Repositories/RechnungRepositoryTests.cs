using Microsoft.EntityFrameworkCore;
using LindebergsHealth.Infrastructure.Data;
using LindebergsHealth.Infrastructure.Repositories;
using LindebergsHealth.Domain.Entities;
using FluentAssertions;

namespace LindebergsHealth.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests für das RechnungRepository
/// </summary>
[TestFixture]
public class RechnungRepositoryTests
{
    private LindebergsHealthDbContext _context;
    private RechnungRepository _repository;
    private DbContextOptions<LindebergsHealthDbContext> _options;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<LindebergsHealthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new LindebergsHealthDbContext(_options);
        _context.Database.EnsureCreated();
        _repository = new RechnungRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task AddAsync_ShouldAddRechnungToDatabase()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var rechnungsstatus = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddAsync(rechnungsstatus);
        await _context.SaveChangesAsync();

        var rechnung = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today,
            Fälligkeitsdatum = DateTime.Today.AddDays(14),
            Gesamtbetrag = 85.50m,
            Steuerbetrag = 16.25m,
            Nettobetrag = 69.25m,
            Steuersatz = 19.0m
        };

        // Act
        await _repository.AddAsync(rechnung);
        await _context.SaveChangesAsync();

        // Assert
        var savedRechnung = await _context.Rechnungen.FindAsync(rechnung.Id);
        savedRechnung.Should().NotBeNull();
        savedRechnung.PatientId.Should().Be(patient.Id);
        savedRechnung.Rechnungsnummer.Should().Be("R-2024-001");
        savedRechnung.Gesamtbetrag.Should().Be(85.50m);
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnRechnungWithRelatedData()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var rechnungsstatus = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var rechnung = new Rechnung
        {
            PatientId = patient.Id,
            Patient = patient,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsstatus = rechnungsstatus,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today,
            Fälligkeitsdatum = DateTime.Today.AddDays(14),
            Gesamtbetrag = 85.50m
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddAsync(rechnungsstatus);
        await _context.Rechnungen.AddAsync(rechnung);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(rechnung.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(rechnung.Id);
        result.Patient.Should().NotBeNull();
        result.Patient.Vorname.Should().Be("Max");
        result.Rechnungsstatus.Should().NotBeNull();
        result.Rechnungsstatus.Bezeichnung.Should().Be("Offen");
        result.Rechnungsnummer.Should().Be("R-2024-001");
    }

    [Test]
    public async Task GetRechnungenByPatientIdAsync_ShouldReturnPatientRechnungen()
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

        var rechnungsstatus = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var rechnung1 = new Rechnung
        {
            PatientId = patient1.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today,
            Gesamtbetrag = 85.50m
        };

        var rechnung2 = new Rechnung
        {
            PatientId = patient1.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-002",
            Rechnungsdatum = DateTime.Today.AddDays(-7),
            Gesamtbetrag = 120.00m
        };

        var rechnung3 = new Rechnung
        {
            PatientId = patient2.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-003",
            Rechnungsdatum = DateTime.Today,
            Gesamtbetrag = 95.00m
        };

        await _context.Patienten.AddRangeAsync(patient1, patient2);
        await _context.Rechnungsstatus.AddAsync(rechnungsstatus);
        await _context.Rechnungen.AddRangeAsync(rechnung1, rechnung2, rechnung3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRechnungenByPatientIdAsync(patient1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Id == rechnung1.Id);
        result.Should().Contain(r => r.Id == rechnung2.Id);
        result.Should().NotContain(r => r.Id == rechnung3.Id);
    }

    [Test]
    public async Task GetOffeneRechnungenAsync_ShouldReturnUnpaidRechnungen()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var statusOffen = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var statusBezahlt = new Rechnungsstatus
        {
            Bezeichnung = "Bezahlt",
            Code = "BEZAHLT"
        };

        var rechnungOffen1 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusOffen.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today,
            Gesamtbetrag = 85.50m
        };

        var rechnungOffen2 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusOffen.Id,
            Rechnungsnummer = "R-2024-002",
            Rechnungsdatum = DateTime.Today.AddDays(-7),
            Gesamtbetrag = 120.00m
        };

        var rechnungBezahlt = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusBezahlt.Id,
            Rechnungsnummer = "R-2024-003",
            Rechnungsdatum = DateTime.Today.AddDays(-14),
            Gesamtbetrag = 95.00m
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddRangeAsync(statusOffen, statusBezahlt);
        await _context.Rechnungen.AddRangeAsync(rechnungOffen1, rechnungOffen2, rechnungBezahlt);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetOffeneRechnungenAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Id == rechnungOffen1.Id);
        result.Should().Contain(r => r.Id == rechnungOffen2.Id);
        result.Should().NotContain(r => r.Id == rechnungBezahlt.Id);
    }

    [Test]
    public async Task GetÜberfälligeRechnungenAsync_ShouldReturnOverdueRechnungen()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var statusOffen = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var rechnungÜberfällig1 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusOffen.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today.AddDays(-30),
            Fälligkeitsdatum = DateTime.Today.AddDays(-16), // Überfällig
            Gesamtbetrag = 85.50m
        };

        var rechnungÜberfällig2 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusOffen.Id,
            Rechnungsnummer = "R-2024-002",
            Rechnungsdatum = DateTime.Today.AddDays(-21),
            Fälligkeitsdatum = DateTime.Today.AddDays(-7), // Überfällig
            Gesamtbetrag = 120.00m
        };

        var rechnungNichtÜberfällig = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusOffen.Id,
            Rechnungsnummer = "R-2024-003",
            Rechnungsdatum = DateTime.Today,
            Fälligkeitsdatum = DateTime.Today.AddDays(14), // Noch nicht fällig
            Gesamtbetrag = 95.00m
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddAsync(statusOffen);
        await _context.Rechnungen.AddRangeAsync(rechnungÜberfällig1, rechnungÜberfällig2, rechnungNichtÜberfällig);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetÜberfälligeRechnungenAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Id == rechnungÜberfällig1.Id);
        result.Should().Contain(r => r.Id == rechnungÜberfällig2.Id);
        result.Should().NotContain(r => r.Id == rechnungNichtÜberfällig.Id);
    }

    [Test]
    public async Task GetRechnungenByDateRangeAsync_ShouldReturnRechnungenInRange()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var rechnungsstatus = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var rechnung1 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today.AddDays(-5), // In Range
            Gesamtbetrag = 85.50m
        };

        var rechnung2 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-002",
            Rechnungsdatum = DateTime.Today.AddDays(-2), // In Range
            Gesamtbetrag = 120.00m
        };

        var rechnung3 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-003",
            Rechnungsdatum = DateTime.Today.AddDays(-15), // Out of Range
            Gesamtbetrag = 95.00m
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddAsync(rechnungsstatus);
        await _context.Rechnungen.AddRangeAsync(rechnung1, rechnung2, rechnung3);
        await _context.SaveChangesAsync();

        var startDate = DateTime.Today.AddDays(-7);
        var endDate = DateTime.Today;

        // Act
        var result = await _repository.GetRechnungenByDateRangeAsync(startDate, endDate);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Id == rechnung1.Id);
        result.Should().Contain(r => r.Id == rechnung2.Id);
        result.Should().NotContain(r => r.Id == rechnung3.Id);
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdateRechnung()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var rechnungsstatus = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var rechnung = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today,
            Gesamtbetrag = 85.50m,
            Bemerkungen = "Ursprüngliche Bemerkung"
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddAsync(rechnungsstatus);
        await _context.Rechnungen.AddAsync(rechnung);
        await _context.SaveChangesAsync();

        // Act
        rechnung.Bemerkungen = "Aktualisierte Bemerkung";
        rechnung.Gesamtbetrag = 95.00m;
        _repository.Update(rechnung);
        await _context.SaveChangesAsync();

        // Assert
        var updatedRechnung = await _context.Rechnungen.FindAsync(rechnung.Id);
        updatedRechnung.Should().NotBeNull();
        updatedRechnung.Bemerkungen.Should().Be("Aktualisierte Bemerkung");
        updatedRechnung.Gesamtbetrag.Should().Be(95.00m);
    }

    [Test]
    public async Task DeleteAsync_ShouldSoftDeleteRechnung()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var rechnungsstatus = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var rechnung = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today,
            Gesamtbetrag = 85.50m
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddAsync(rechnungsstatus);
        await _context.Rechnungen.AddAsync(rechnung);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(rechnung.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedRechnung = await _context.Rechnungen.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Id == rechnung.Id);
        deletedRechnung.Should().NotBeNull();
        deletedRechnung.IstGelöscht.Should().BeTrue();
        deletedRechnung.GelöschtAm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        // Verify it's not returned in normal queries
        var activeRechnungen = await _repository.GetAllAsync();
        activeRechnungen.Should().NotContain(r => r.Id == rechnung.Id);
    }

    [Test]
    public async Task GetRechnungByRechnungsnummerAsync_ShouldReturnCorrectRechnung()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var rechnungsstatus = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var rechnung1 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today,
            Gesamtbetrag = 85.50m
        };

        var rechnung2 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = rechnungsstatus.Id,
            Rechnungsnummer = "R-2024-002",
            Rechnungsdatum = DateTime.Today,
            Gesamtbetrag = 120.00m
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddAsync(rechnungsstatus);
        await _context.Rechnungen.AddRangeAsync(rechnung1, rechnung2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetRechnungByRechnungsnummerAsync("R-2024-001");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(rechnung1.Id);
        result.Rechnungsnummer.Should().Be("R-2024-001");
        result.Gesamtbetrag.Should().Be(85.50m);
    }

    [Test]
    public async Task CalculateGesamtumsatzAsync_ShouldReturnCorrectSum()
    {
        // Arrange
        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann"
        };

        var statusBezahlt = new Rechnungsstatus
        {
            Bezeichnung = "Bezahlt",
            Code = "BEZAHLT"
        };

        var statusOffen = new Rechnungsstatus
        {
            Bezeichnung = "Offen",
            Code = "OFFEN"
        };

        var rechnungBezahlt1 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusBezahlt.Id,
            Rechnungsnummer = "R-2024-001",
            Rechnungsdatum = DateTime.Today.AddDays(-5),
            Gesamtbetrag = 85.50m
        };

        var rechnungBezahlt2 = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusBezahlt.Id,
            Rechnungsnummer = "R-2024-002",
            Rechnungsdatum = DateTime.Today.AddDays(-3),
            Gesamtbetrag = 120.00m
        };

        var rechnungOffen = new Rechnung
        {
            PatientId = patient.Id,
            RechnungsstatusId = statusOffen.Id,
            Rechnungsnummer = "R-2024-003",
            Rechnungsdatum = DateTime.Today,
            Gesamtbetrag = 95.00m
        };

        await _context.Patienten.AddAsync(patient);
        await _context.Rechnungsstatus.AddRangeAsync(statusBezahlt, statusOffen);
        await _context.Rechnungen.AddRangeAsync(rechnungBezahlt1, rechnungBezahlt2, rechnungOffen);
        await _context.SaveChangesAsync();

        var startDate = DateTime.Today.AddDays(-7);
        var endDate = DateTime.Today;

        // Act
        var result = await _repository.CalculateGesamtumsatzAsync(startDate, endDate);

        // Assert
        result.Should().Be(205.50m); // 85.50 + 120.00 (nur bezahlte Rechnungen)
    }
} 