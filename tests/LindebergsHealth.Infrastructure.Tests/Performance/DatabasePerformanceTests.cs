using Microsoft.EntityFrameworkCore;
using LindebergsHealth.Infrastructure.Data;
using LindebergsHealth.Domain.Entities;
using FluentAssertions;
using System.Diagnostics;

namespace LindebergsHealth.Infrastructure.Tests.Performance;

/// <summary>
/// Performance-Tests für die Datenbankoperationen
/// </summary>
[TestFixture]
public class DatabasePerformanceTests
{
    private LindebergsHealthDbContext _context;
    private DbContextOptions<LindebergsHealthDbContext> _options;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<LindebergsHealthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new LindebergsHealthDbContext(_options);
        _context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task BulkInsert_1000Patients_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var patients = GenerateTestPatients(1000);
        var stopwatch = Stopwatch.StartNew();

        // Act
        _context.Patienten.AddRange(patients);
        await _context.SaveChangesAsync();
        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // Should complete in less than 5 seconds
        
        var savedCount = await _context.Patienten.CountAsync();
        savedCount.Should().Be(1000);
    }

    [Test]
    public async Task BulkUpdate_1000Patients_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var patients = GenerateTestPatients(1000);
        _context.Patienten.AddRange(patients);
        await _context.SaveChangesAsync();

        var stopwatch = Stopwatch.StartNew();

        // Act
        foreach (var patient in patients)
        {
            patient.Vorname = $"Updated_{patient.Vorname}";
            patient.MarkAsModified(Guid.NewGuid());
        }

        await _context.SaveChangesAsync();
        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // Should complete in less than 5 seconds
        
        var updatedPatients = await _context.Patienten.Where(p => p.Vorname.StartsWith("Updated_")).CountAsync();
        updatedPatients.Should().Be(1000);
    }

    [Test]
    public async Task ComplexQuery_WithIncludes_ShouldCompleteInReasonableTime()
    {
        // Arrange
        await SeedComplexTestData();
        var stopwatch = Stopwatch.StartNew();

        // Act
        var results = await _context.Patienten
            .Include(p => p.Termine)
                .ThenInclude(t => t.Therapeut)
            .Include(p => p.Therapien)
                .ThenInclude(th => th.Therapeut)
            .Include(p => p.Rechnungen)
                .ThenInclude(r => r.Zahlungen)
            .Where(p => p.IstAktiv)
            .Take(100)
            .ToListAsync();

        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(2000); // Should complete in less than 2 seconds
        results.Should().NotBeEmpty();
    }

    [Test]
    public async Task SearchQuery_AcrossLargeDataset_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var patients = GenerateTestPatients(5000);
        _context.Patienten.AddRange(patients);
        await _context.SaveChangesAsync();

        var stopwatch = Stopwatch.StartNew();

        // Act
        var searchResults = await _context.Patienten
            .Where(p => p.Vorname.Contains("Test") || p.Nachname.Contains("Test"))
            .OrderBy(p => p.Nachname)
            .ThenBy(p => p.Vorname)
            .Take(50)
            .ToListAsync();

        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Should complete in less than 1 second
        searchResults.Should().NotBeEmpty();
    }

    [Test]
    public async Task PaginatedQuery_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var patients = GenerateTestPatients(2000);
        _context.Patienten.AddRange(patients);
        await _context.SaveChangesAsync();

        var pageSize = 20;
        var pageNumber = 50; // Middle of the dataset
        var stopwatch = Stopwatch.StartNew();

        // Act
        var paginatedResults = await _context.Patienten
            .OrderBy(p => p.Nachname)
            .ThenBy(p => p.Vorname)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(500); // Should complete in less than 0.5 seconds
        paginatedResults.Should().HaveCount(pageSize);
    }

    [Test]
    public async Task AggregateQueries_ShouldCompleteInReasonableTime()
    {
        // Arrange
        await SeedComplexTestData();
        var stopwatch = Stopwatch.StartNew();

        // Act
        var stats = await Task.WhenAll(
            _context.Patienten.CountAsync(),
            _context.Termine.CountAsync(),
            _context.Therapien.CountAsync(),
            _context.Rechnungen.SumAsync(r => r.Gesamtbetrag),
            _context.Patienten.Where(p => p.IstAktiv).CountAsync()
        );

        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Should complete in less than 1 second
        stats.Should().NotBeNull();
        stats.Length.Should().Be(5);
    }

    [Test]
    public async Task ConcurrentReads_ShouldHandleMultipleConnections()
    {
        // Arrange
        var patients = GenerateTestPatients(1000);
        _context.Patienten.AddRange(patients);
        await _context.SaveChangesAsync();

        var tasks = new List<Task<List<Patient>>>();
        var stopwatch = Stopwatch.StartNew();

        // Act - Simulate 10 concurrent read operations
        for (int i = 0; i < 10; i++)
        {
            var task = Task.Run(async () =>
            {
                using var context = new LindebergsHealthDbContext(_options);
                return await context.Patienten
                    .Where(p => p.Vorname.Contains("Test"))
                    .Take(100)
                    .ToListAsync();
            });
            tasks.Add(task);
        }

        var results = await Task.WhenAll(tasks);
        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(3000); // Should complete in less than 3 seconds
        results.Should().HaveCount(10);
        results.All(r => r.Count > 0).Should().BeTrue();
    }

    [Test]
    public async Task QueryFilter_Performance_ShouldNotSignificantlyImpactQueries()
    {
        // Arrange
        var patients = GenerateTestPatients(1000);
        // Mark half as inactive and some as deleted
        for (int i = 0; i < patients.Count; i++)
        {
            if (i % 2 == 0) patients[i].IstAktiv = false;
            if (i % 10 == 0) patients[i].SoftDelete(Guid.NewGuid(), "Test deletion");
        }

        _context.Patienten.AddRange(patients);
        await _context.SaveChangesAsync();

        var stopwatch = Stopwatch.StartNew();

        // Act - Query with filters applied
        var filteredResults = await _context.Patienten.ToListAsync();
        var unfilteredResults = await _context.Patienten.IgnoreQueryFilters().ToListAsync();

        stopwatch.Stop();

        // Assert
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000); // Should complete in less than 1 second
        filteredResults.Count.Should().BeLessThan(unfilteredResults.Count);
        filteredResults.All(p => p.IstAktiv && !p.IstGelöscht).Should().BeTrue();
    }

    private List<Patient> GenerateTestPatients(int count)
    {
        var patients = new List<Patient>();
        var random = new Random(42); // Fixed seed for reproducible tests

        for (int i = 0; i < count; i++)
        {
            patients.Add(new Patient
            {
                Vorname = $"Test{i}",
                Nachname = $"Patient{i}",
                Geburtsdatum = DateTime.Today.AddYears(-random.Next(18, 80)),
                Geschlecht = random.Next(2) == 0 ? "M" : "W",
                Email = $"test{i}@example.com",
                Telefon = $"0{random.Next(100000000, 999999999)}",
                Straße = $"Teststraße {i}",
                PLZ = $"{random.Next(10000, 99999)}",
                Ort = $"Teststadt{i % 10}",
                Land = "Deutschland",
                IstAktiv = true
            });
        }

        return patients;
    }

    private async Task SeedComplexTestData()
    {
        var patients = GenerateTestPatients(100);
        var mitarbeiter = new List<Mitarbeiter>();
        var therapien = new List<Therapie>();
        var termine = new List<Termin>();
        var rechnungen = new List<Rechnung>();

        // Create staff
        for (int i = 0; i < 10; i++)
        {
            mitarbeiter.Add(new Mitarbeiter
            {
                Vorname = $"Dr. Test{i}",
                Nachname = $"Therapeut{i}",
                Email = $"therapeut{i}@test.com"
            });
        }

        _context.Patienten.AddRange(patients);
        _context.Mitarbeiter.AddRange(mitarbeiter);
        await _context.SaveChangesAsync();

        var random = new Random(42);

        // Create therapies
        foreach (var patient in patients.Take(50))
        {
            var therapeut = mitarbeiter[random.Next(mitarbeiter.Count)];
            var therapie = new Therapie
            {
                PatientId = patient.Id,
                TherapeutId = therapeut.Id,
                Bezeichnung = $"Therapie für {patient.Vorname}",
                StartDatum = DateTime.Today.AddDays(-random.Next(30, 365))
            };
            therapien.Add(therapie);
        }

        _context.Therapien.AddRange(therapien);
        await _context.SaveChangesAsync();

        // Create appointments
        foreach (var therapie in therapien)
        {
            for (int i = 0; i < random.Next(1, 10); i++)
            {
                termine.Add(new Termin
                {
                    PatientId = therapie.PatientId,
                    TherapeutId = therapie.TherapeutId,
                    TherapieId = therapie.Id,
                    Datum = DateTime.Today.AddDays(random.Next(-30, 30)),
                    Uhrzeit = TimeSpan.FromHours(random.Next(8, 18)),
                    Dauer = TimeSpan.FromMinutes(30),
                    Titel = $"Termin {i + 1}"
                });
            }
        }

        _context.Termine.AddRange(termine);

        // Create invoices
        foreach (var patient in patients.Take(30))
        {
            rechnungen.Add(new Rechnung
            {
                PatientId = patient.Id,
                RechnungsNummer = $"R{DateTime.Now.Ticks}{random.Next(1000)}",
                Rechnungsdatum = DateTime.Today.AddDays(-random.Next(1, 90)),
                Gesamtbetrag = random.Next(50, 500)
            });
        }

        _context.Rechnungen.AddRange(rechnungen);
        await _context.SaveChangesAsync();
    }
} 