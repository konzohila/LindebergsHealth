using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LindebergsHealth.Infrastructure.Data;
using LindebergsHealth.Infrastructure.Repositories;
using LindebergsHealth.Domain.Entities;
using FluentAssertions;

namespace LindebergsHealth.Infrastructure.Tests.Integration;

/// <summary>
/// Integrationstests für die gesamte Datenbankintegration
/// </summary>
[TestFixture]
public class DatabaseIntegrationTests
{
    private ServiceProvider _serviceProvider;
    private LindebergsHealthDbContext _context;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<LindebergsHealthDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));
        
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<ITerminRepository, TerminRepository>();
        services.AddScoped<ITherapieRepository, TherapieRepository>();
        services.AddScoped<IMitarbeiterRepository, MitarbeiterRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        _serviceProvider = services.BuildServiceProvider();
        _context = _serviceProvider.GetRequiredService<LindebergsHealthDbContext>();
        _context.Database.EnsureCreated();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
        _serviceProvider.Dispose();
    }

    [Test]
    public async Task CompletePatientWorkflow_ShouldWorkEndToEnd()
    {
        // Arrange
        var patientRepo = _serviceProvider.GetRequiredService<IPatientRepository>();
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

        var patient = new Patient
        {
            Vorname = "Integration",
            Nachname = "Test",
            Geburtsdatum = new DateTime(1990, 5, 15),
            Geschlecht = "M",
            Email = "integration@test.com",
            Telefon = "0123456789"
        };

        // Act & Assert - Create
        await patientRepo.AddAsync(patient);
        await unitOfWork.SaveChangesAsync();

        var savedPatient = await patientRepo.GetByIdAsync(patient.Id);
        savedPatient.Should().NotBeNull();
        savedPatient!.Vorname.Should().Be("Integration");

        // Act & Assert - Update
        savedPatient.Vorname = "Updated Integration";
        savedPatient.MarkAsModified(Guid.NewGuid());
        patientRepo.Update(savedPatient);
        await unitOfWork.SaveChangesAsync();

        var updatedPatient = await patientRepo.GetByIdAsync(patient.Id);
        updatedPatient!.Vorname.Should().Be("Updated Integration");
        updatedPatient.GeändertAm.Should().NotBeNull();

        // Act & Assert - Search
        var searchResults = await patientRepo.SearchAsync("Updated");
        searchResults.Should().HaveCount(1);
        searchResults.First().Id.Should().Be(patient.Id);

        // Act & Assert - Soft Delete
        await patientRepo.DeleteAsync(patient.Id);
        await unitOfWork.SaveChangesAsync();

        var deletedPatient = await patientRepo.GetByIdAsync(patient.Id);
        deletedPatient.Should().BeNull(); // Filtered out by query filter

        // Verify soft delete
        var allPatients = await _context.Patienten.IgnoreQueryFilters().ToListAsync();
        var softDeletedPatient = allPatients.FirstOrDefault(p => p.Id == patient.Id);
        softDeletedPatient.Should().NotBeNull();
        softDeletedPatient!.IstGelöscht.Should().BeTrue();
    }

    [Test]
    public async Task CompleteTerminWorkflow_WithRelationships_ShouldWorkEndToEnd()
    {
        // Arrange
        var patientRepo = _serviceProvider.GetRequiredService<IPatientRepository>();
        var mitarbeiterRepo = _serviceProvider.GetRequiredService<IMitarbeiterRepository>();
        var terminRepo = _serviceProvider.GetRequiredService<ITerminRepository>();
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

        var patient = new Patient
        {
            Vorname = "Termin",
            Nachname = "Patient"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Termin",
            Nachname = "Therapeut",
            Email = "therapeut@test.com"
        };

        var termin = new Termin
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            Datum = DateTime.Today.AddDays(1),
            Uhrzeit = TimeSpan.FromHours(10),
            Dauer = TimeSpan.FromMinutes(30),
            Titel = "Integration Test Termin",
            Beschreibung = "Test Beschreibung"
        };

        // Act - Create all entities
        await patientRepo.AddAsync(patient);
        await mitarbeiterRepo.AddAsync(therapeut);
        await terminRepo.AddAsync(termin);
        await unitOfWork.SaveChangesAsync();

        // Assert - Verify relationships
        var savedTermin = await terminRepo.GetWithDetailsAsync(termin.Id);
        savedTermin.Should().NotBeNull();
        savedTermin!.Patient.Should().NotBeNull();
        savedTermin.Patient!.Vorname.Should().Be("Termin");
        savedTermin.Therapeut.Should().NotBeNull();
        savedTermin.Therapeut!.Vorname.Should().Be("Dr. Termin");

        // Act - Confirm appointment
        var userId = Guid.NewGuid();
        savedTermin.Bestätigen(userId);
        terminRepo.Update(savedTermin);
        await unitOfWork.SaveChangesAsync();

        // Assert - Verify confirmation
        var confirmedTermin = await terminRepo.GetByIdAsync(termin.Id);
        confirmedTermin!.IstBestätigt.Should().BeTrue();
        confirmedTermin.BestätigtVon.Should().Be(userId);

        // Act - Reschedule appointment
        var newDate = DateTime.Today.AddDays(2);
        var newTime = TimeSpan.FromHours(14);
        confirmedTermin.Verschieben(userId, newDate, newTime, "Terminkonflikt");
        terminRepo.Update(confirmedTermin);
        await unitOfWork.SaveChangesAsync();

        // Assert - Verify rescheduling
        var rescheduledTermin = await terminRepo.GetByIdAsync(termin.Id);
        rescheduledTermin!.Datum.Should().Be(newDate);
        rescheduledTermin.Uhrzeit.Should().Be(newTime);
        rescheduledTermin.VerschobenGrund.Should().Be("Terminkonflikt");
    }

    [Test]
    public async Task CompleteTherapieWorkflow_ShouldWorkEndToEnd()
    {
        // Arrange
        var patientRepo = _serviceProvider.GetRequiredService<IPatientRepository>();
        var mitarbeiterRepo = _serviceProvider.GetRequiredService<IMitarbeiterRepository>();
        var therapieRepo = _serviceProvider.GetRequiredService<ITherapieRepository>();
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

        var patient = new Patient
        {
            Vorname = "Therapie",
            Nachname = "Patient"
        };

        var therapeut = new Mitarbeiter
        {
            Vorname = "Dr. Therapie",
            Nachname = "Therapeut",
            Email = "therapeut@test.com"
        };

        var therapie = new Therapie
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            Bezeichnung = "Integration Test Therapie",
            Beschreibung = "Test Therapie Beschreibung",
            StartDatum = DateTime.Today,
            GeplantesEndDatum = DateTime.Today.AddDays(30)
        };

        // Act - Create entities
        await patientRepo.AddAsync(patient);
        await mitarbeiterRepo.AddAsync(therapeut);
        await therapieRepo.AddAsync(therapie);
        await unitOfWork.SaveChangesAsync();

        // Assert - Verify creation
        var savedTherapie = await therapieRepo.GetWithDetailsAsync(therapie.Id);
        savedTherapie.Should().NotBeNull();
        savedTherapie!.Patient.Should().NotBeNull();
        savedTherapie.Therapeut.Should().NotBeNull();

        // Act - Start therapy
        var userId = Guid.NewGuid();
        savedTherapie.Starten(userId);
        therapieRepo.Update(savedTherapie);
        await unitOfWork.SaveChangesAsync();

        // Assert - Verify start
        var startedTherapie = await therapieRepo.GetByIdAsync(therapie.Id);
        startedTherapie!.Status.Should().Be("Aktiv");
        startedTherapie.GestartetAm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        // Act - Complete therapy
        startedTherapie.Abschließen(userId, "Therapie erfolgreich abgeschlossen");
        therapieRepo.Update(startedTherapie);
        await unitOfWork.SaveChangesAsync();

        // Assert - Verify completion
        var completedTherapie = await therapieRepo.GetByIdAsync(therapie.Id);
        completedTherapie!.Status.Should().Be("Abgeschlossen");
        completedTherapie.AbgeschlossenAm.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        completedTherapie.AbschlussNotizen.Should().Be("Therapie erfolgreich abgeschlossen");
    }

    [Test]
    public async Task ConcurrentAccess_ShouldHandleConcurrencyCorrectly()
    {
        // Arrange
        var patientRepo = _serviceProvider.GetRequiredService<IPatientRepository>();
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

        var patient = new Patient
        {
            Vorname = "Concurrent",
            Nachname = "Test"
        };

        await patientRepo.AddAsync(patient);
        await unitOfWork.SaveChangesAsync();

        // Create two separate contexts to simulate concurrent access
        using var context1 = new LindebergsHealthDbContext(
            new DbContextOptionsBuilder<LindebergsHealthDbContext>()
                .UseInMemoryDatabase(_context.Database.GetDbConnection().Database)
                .Options);

        using var context2 = new LindebergsHealthDbContext(
            new DbContextOptionsBuilder<LindebergsHealthDbContext>()
                .UseInMemoryDatabase(_context.Database.GetDbConnection().Database)
                .Options);

        var patient1 = await context1.Patienten.FirstAsync(p => p.Id == patient.Id);
        var patient2 = await context2.Patienten.FirstAsync(p => p.Id == patient.Id);

        // Act
        patient1.Vorname = "Updated1";
        patient2.Vorname = "Updated2";

        await context1.SaveChangesAsync();

        // Assert
        var act = async () => await context2.SaveChangesAsync();
        await act.Should().ThrowAsync<DbUpdateConcurrencyException>();
    }

    [Test]
    public async Task QueryFilters_ShouldWorkAcrossAllEntities()
    {
        // Arrange
        var patientRepo = _serviceProvider.GetRequiredService<IPatientRepository>();
        var terminRepo = _serviceProvider.GetRequiredService<ITerminRepository>();
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

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

        var deletedPatient = new Patient
        {
            Vorname = "Deleted",
            Nachname = "Patient"
        };

        await patientRepo.AddAsync(activePatient);
        await patientRepo.AddAsync(inactivePatient);
        await patientRepo.AddAsync(deletedPatient);
        await unitOfWork.SaveChangesAsync();

        // Soft delete one patient
        deletedPatient.SoftDelete(Guid.NewGuid(), "Test deletion");
        patientRepo.Update(deletedPatient);
        await unitOfWork.SaveChangesAsync();

        // Act
        var allPatients = await patientRepo.GetAllAsync();
        var allPatientsIgnoreFilters = await _context.Patienten.IgnoreQueryFilters().ToListAsync();

        // Assert
        allPatients.Should().HaveCount(1); // Only active patient
        allPatients.First().Vorname.Should().Be("Active");

        allPatientsIgnoreFilters.Should().HaveCount(3); // All patients including inactive and deleted
    }

    [Test]
    public async Task LookupData_ShouldBeAccessible()
    {
        // Act
        var terminStatus = await _context.TerminStatus.ToListAsync();
        var geschlechter = await _context.Geschlechter.ToListAsync();
        var therapieTypen = await _context.TherapieTypen.ToListAsync();

        // Assert
        terminStatus.Should().NotBeEmpty();
        geschlechter.Should().NotBeEmpty();
        therapieTypen.Should().NotBeEmpty();

        // Verify specific lookup values
        terminStatus.Should().Contain(ts => ts.Name == "Geplant");
        terminStatus.Should().Contain(ts => ts.Name == "Bestätigt");
        terminStatus.Should().Contain(ts => ts.Name == "Abgeschlossen");
        terminStatus.Should().Contain(ts => ts.Name == "Abgesagt");

        geschlechter.Should().Contain(g => g.Kürzel == "M" && g.Bezeichnung == "Männlich");
        geschlechter.Should().Contain(g => g.Kürzel == "W" && g.Bezeichnung == "Weiblich");
        geschlechter.Should().Contain(g => g.Kürzel == "D" && g.Bezeichnung == "Divers");
    }

    [Test]
    public async Task ComplexQueries_WithMultipleIncludes_ShouldPerformWell()
    {
        // Arrange
        var patientRepo = _serviceProvider.GetRequiredService<IPatientRepository>();
        var mitarbeiterRepo = _serviceProvider.GetRequiredService<IMitarbeiterRepository>();
        var terminRepo = _serviceProvider.GetRequiredService<ITerminRepository>();
        var therapieRepo = _serviceProvider.GetRequiredService<ITherapieRepository>();
        var unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();

        // Create test data
        var patient = new Patient { Vorname = "Complex", Nachname = "Query" };
        var therapeut = new Mitarbeiter { Vorname = "Dr. Complex", Nachname = "Therapeut", Email = "complex@test.com" };
        
        var therapie = new Therapie
        {
            PatientId = patient.Id,
            TherapeutId = therapeut.Id,
            Bezeichnung = "Complex Therapie",
            StartDatum = DateTime.Today
        };

        var termine = new List<Termin>
        {
            new()
            {
                PatientId = patient.Id,
                TherapeutId = therapeut.Id,
                TherapieId = therapie.Id,
                Datum = DateTime.Today.AddDays(1),
                Uhrzeit = TimeSpan.FromHours(10),
                Dauer = TimeSpan.FromMinutes(30),
                Titel = "Termin 1"
            },
            new()
            {
                PatientId = patient.Id,
                TherapeutId = therapeut.Id,
                TherapieId = therapie.Id,
                Datum = DateTime.Today.AddDays(2),
                Uhrzeit = TimeSpan.FromHours(11),
                Dauer = TimeSpan.FromMinutes(45),
                Titel = "Termin 2"
            }
        };

        await patientRepo.AddAsync(patient);
        await mitarbeiterRepo.AddAsync(therapeut);
        await therapieRepo.AddAsync(therapie);
        
        foreach (var termin in termine)
        {
            await terminRepo.AddAsync(termin);
        }
        
        await unitOfWork.SaveChangesAsync();

        // Act - Complex query with multiple includes
        var result = await _context.Patienten
            .Include(p => p.Termine)
                .ThenInclude(t => t.Therapeut)
            .Include(p => p.Therapien)
                .ThenInclude(th => th.Therapeut)
            .Where(p => p.Vorname == "Complex")
            .FirstOrDefaultAsync();

        // Assert
        result.Should().NotBeNull();
        result!.Termine.Should().HaveCount(2);
        result.Therapien.Should().HaveCount(1);
        result.Termine.All(t => t.Therapeut != null).Should().BeTrue();
        result.Therapien.All(th => th.Therapeut != null).Should().BeTrue();
    }
} 