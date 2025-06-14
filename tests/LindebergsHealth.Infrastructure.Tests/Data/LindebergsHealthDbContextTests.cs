using Microsoft.EntityFrameworkCore;
using LindebergsHealth.Infrastructure.Data;
using LindebergsHealth.Domain.Entities;
using FluentAssertions;

namespace LindebergsHealth.Infrastructure.Tests.Data;

/// <summary>
/// Tests für den LindebergsHealthDbContext
/// </summary>
[TestFixture]
public class LindebergsHealthDbContextTests
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
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public void DbContext_ShouldHaveAllDbSets()
    {
        // Assert - Hauptentitäten
        Assert.That(_context.Patienten, Is.Not.Null);
        Assert.That(_context.Mitarbeiter, Is.Not.Null);
        Assert.That(_context.Termine, Is.Not.Null);
        Assert.That(_context.Rechnungen, Is.Not.Null);
        Assert.That(_context.Therapieserien, Is.Not.Null);
        Assert.That(_context.Koerperstatuseintraege, Is.Not.Null);
        Assert.That(_context.Dokumente, Is.Not.Null);
        Assert.That(_context.Einwilligungen, Is.Not.Null);
        Assert.That(_context.Kommunikationsverlaeufe, Is.Not.Null);
        Assert.That(_context.Wartelisten, Is.Not.Null);
        Assert.That(_context.Praxen, Is.Not.Null);
        Assert.That(_context.Raeume, Is.Not.Null);
        Assert.That(_context.Ausstattungen, Is.Not.Null);
        Assert.That(_context.Adressen, Is.Not.Null);
        Assert.That(_context.Kontakte, Is.Not.Null);

        // Assert - Lookup-Tabellen
        Assert.That(_context.Geschlechter, Is.Not.Null);
        Assert.That(_context.Versicherungstypen, Is.Not.Null);
        Assert.That(_context.Versicherungsstatus, Is.Not.Null);
        Assert.That(_context.Beziehungstypen, Is.Not.Null);
        Assert.That(_context.Termintypen, Is.Not.Null);
        Assert.That(_context.Terminstatus, Is.Not.Null);
        Assert.That(_context.Rechnungsstatus, Is.Not.Null);
        Assert.That(_context.Therapietypen, Is.Not.Null);
        Assert.That(_context.Therapiestatus, Is.Not.Null);
        Assert.That(_context.Koerperregionen, Is.Not.Null);
        Assert.That(_context.Dokumenttypen, Is.Not.Null);
        Assert.That(_context.EinwilligungsKategorien, Is.Not.Null);
        Assert.That(_context.Kommunikationskanaele, Is.Not.Null);
        Assert.That(_context.Prioritaeten, Is.Not.Null);
        Assert.That(_context.Rollen, Is.Not.Null);
        Assert.That(_context.Vertragstypen, Is.Not.Null);
        Assert.That(_context.Vertragsstatus, Is.Not.Null);
        Assert.That(_context.Teilnahmeformen, Is.Not.Null);
        Assert.That(_context.Raumtypen, Is.Not.Null);
        Assert.That(_context.Ausstattungstypen, Is.Not.Null);
        Assert.That(_context.Adresstypen, Is.Not.Null);
        Assert.That(_context.Kontakttypen, Is.Not.Null);
        Assert.That(_context.Staatsangehoerigkeiten, Is.Not.Null);
        Assert.That(_context.Familienstaende, Is.Not.Null);
        Assert.That(_context.CRMStatusTypen, Is.Not.Null);
        Assert.That(_context.Kundentypen, Is.Not.Null);
        Assert.That(_context.Aenderungstypen, Is.Not.Null);
    }

    [Test]
    public async Task AddPatient_ShouldSaveSuccessfully()
    {
        // Arrange
        var geschlecht = new Geschlecht
        {
            Bezeichnung = "Männlich",
            Code = "M"
        };
        _context.Geschlechter.Add(geschlecht);
        await _context.SaveChangesAsync();

        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            Geburtsdatum = new DateTime(1990, 5, 15),
            GeschlechtId = geschlecht.Id
        };

        // Act
        _context.Patienten.Add(patient);
        var result = await _context.SaveChangesAsync();

        // Assert
        Assert.That(result, Is.EqualTo(1));
        Assert.That(patient.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(patient.ErstelltAm, Is.Not.EqualTo(default(DateTime)));
        Assert.That(patient.RowVersion, Is.Not.Null);
    }

    [Test]
    public async Task AddMitarbeiter_ShouldSaveSuccessfully()
    {
        // Arrange
        var rolle = new Rolle
        {
            Bezeichnung = "Physiotherapeut",
            Code = "PHYSIO"
        };
        _context.Rollen.Add(rolle);
        await _context.SaveChangesAsync();

        var mitarbeiter = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt",
            RolleId = rolle.Id,
            PersonalNummer = "MA001",
            IstAktiv = true
        };

        // Act
        _context.Mitarbeiter.Add(mitarbeiter);
        var result = await _context.SaveChangesAsync();

        // Assert
        Assert.That(result, Is.EqualTo(1));
        Assert.That(mitarbeiter.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(mitarbeiter.ErstelltAm, Is.Not.EqualTo(default(DateTime)));
        Assert.That(mitarbeiter.RowVersion, Is.Not.Null);
    }

    [Test]
    public async Task AddTermin_ShouldSaveSuccessfully()
    {
        // Arrange
        var geschlecht = new Geschlecht { Bezeichnung = "Männlich", Code = "M" };
        var rolle = new Rolle { Bezeichnung = "Physiotherapeut", Code = "PHYSIO" };
        var termintyp = new Termintyp { Bezeichnung = "Physiotherapie", Code = "PHYSIO" };
        var terminstatus = new Terminstatus { Bezeichnung = "Geplant", Code = "GEPLANT" };
        var raumtyp = new Raumtyp { Bezeichnung = "Behandlungsraum", Code = "BEHANDLUNG" };

        _context.Geschlechter.Add(geschlecht);
        _context.Rollen.Add(rolle);
        _context.Termintypen.Add(termintyp);
        _context.Terminstatus.Add(terminstatus);
        _context.Raumtypen.Add(raumtyp);
        await _context.SaveChangesAsync();

        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            GeschlechtId = geschlecht.Id
        };

        var mitarbeiter = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt",
            RolleId = rolle.Id,
            PersonalNummer = "MA001"
        };

        var raum = new Raum
        {
            Bezeichnung = "Behandlungsraum 1",
            Nummer = "R001",
            RaumtypId = raumtyp.Id
        };

        _context.Patienten.Add(patient);
        _context.Mitarbeiter.Add(mitarbeiter);
        _context.Raeume.Add(raum);
        await _context.SaveChangesAsync();

        var termin = new Termin
        {
            PatientId = patient.Id,
            MitarbeiterId = mitarbeiter.Id,
            RaumId = raum.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1).AddHours(10),
            DauerMinuten = 60,
            KategorieId = Guid.NewGuid()
        };

        // Act
        _context.Termine.Add(termin);
        var result = await _context.SaveChangesAsync();

        // Assert
        Assert.That(result, Is.EqualTo(1));
        Assert.That(termin.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(termin.ErstelltAm, Is.Not.EqualTo(default(DateTime)));
        Assert.That(termin.RowVersion, Is.Not.Null);
    }

    [Test]
    public async Task QueryFilter_ShouldExcludeDeletedEntities()
    {
        // Arrange
        var geschlecht = new Geschlecht { Bezeichnung = "Männlich", Code = "M" };
        _context.Geschlechter.Add(geschlecht);
        await _context.SaveChangesAsync();

        var patient1 = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            GeschlechtId = geschlecht.Id,
            IstGelöscht = false
        };

        var patient2 = new Patient
        {
            Vorname = "Anna",
            Nachname = "Schmidt",
            GeschlechtId = geschlecht.Id,
            IstGelöscht = true // Gelöscht
        };

        _context.Patienten.AddRange(patient1, patient2);
        await _context.SaveChangesAsync();

        // Act
        var activePatients = await _context.Patienten.ToListAsync();

        // Assert
        Assert.That(activePatients.Count, Is.EqualTo(1));
        Assert.That(activePatients.First().Vorname, Is.EqualTo("Max"));
    }

    [Test]
    public async Task SoftDelete_ShouldMarkAsDeleted()
    {
        // Arrange
        var geschlecht = new Geschlecht { Bezeichnung = "Männlich", Code = "M" };
        _context.Geschlechter.Add(geschlecht);
        await _context.SaveChangesAsync();

        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            GeschlechtId = geschlecht.Id
        };

        _context.Patienten.Add(patient);
        await _context.SaveChangesAsync();

        // Act
        patient.IstGelöscht = true;
        patient.GelöschtAm = DateTime.UtcNow;
        patient.GelöschtVon = Guid.NewGuid();
        await _context.SaveChangesAsync();

        // Assert
        var deletedPatient = await _context.Patienten
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == patient.Id);

        Assert.That(deletedPatient, Is.Not.Null);
        Assert.That(deletedPatient.IstGelöscht, Is.True);
        Assert.That(deletedPatient.GelöschtAm, Is.Not.Null);
        Assert.That(deletedPatient.GelöschtVon, Is.Not.Null);

        // Verify it's excluded from normal queries
        var activePatients = await _context.Patienten.ToListAsync();
        Assert.That(activePatients, Does.Not.Contain(deletedPatient));
    }

    [Test]
    public async Task ConcurrencyCheck_ShouldThrowOnConflict()
    {
        // Arrange
        var geschlecht = new Geschlecht { Bezeichnung = "Männlich", Code = "M" };
        _context.Geschlechter.Add(geschlecht);
        await _context.SaveChangesAsync();

        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            GeschlechtId = geschlecht.Id
        };

        _context.Patienten.Add(patient);
        await _context.SaveChangesAsync();

        // Act & Assert
        using var context2 = new LindebergsHealthDbContext(_options);
        var patient1 = await _context.Patienten.FirstAsync(p => p.Id == patient.Id);
        var patient2 = await context2.Patienten.FirstAsync(p => p.Id == patient.Id);

        patient1.Vorname = "Max Updated 1";
        patient2.Vorname = "Max Updated 2";

        await _context.SaveChangesAsync(); // Erste Änderung erfolgreich

        // Zweite Änderung sollte Concurrency Exception werfen
        Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            await context2.SaveChangesAsync());
    }

    [Test]
    public async Task SeedData_ShouldBeAvailable()
    {
        // Act
        var geschlechter = await _context.Geschlechter.ToListAsync();
        var rollen = await _context.Rollen.ToListAsync();
        var termintypen = await _context.Termintypen.ToListAsync();

        // Assert
        Assert.That(geschlechter.Count, Is.GreaterThan(0));
        Assert.That(rollen.Count, Is.GreaterThan(0));
        Assert.That(termintypen.Count, Is.GreaterThan(0));

        // Verify specific seed data
        Assert.That(geschlechter.Any(g => g.Code == "M"), Is.True);
        Assert.That(geschlechter.Any(g => g.Code == "W"), Is.True);
        Assert.That(geschlechter.Any(g => g.Code == "D"), Is.True);
    }

    [Test]
    public async Task Relationships_ShouldLoadCorrectly()
    {
        // Arrange
        var geschlecht = new Geschlecht { Bezeichnung = "Männlich", Code = "M" };
        var rolle = new Rolle { Bezeichnung = "Physiotherapeut", Code = "PHYSIO" };
        var termintyp = new Termintyp { Bezeichnung = "Physiotherapie", Code = "PHYSIO" };
        var terminstatus = new Terminstatus { Bezeichnung = "Geplant", Code = "GEPLANT" };
        var raumtyp = new Raumtyp { Bezeichnung = "Behandlungsraum", Code = "BEHANDLUNG" };

        _context.Geschlechter.Add(geschlecht);
        _context.Rollen.Add(rolle);
        _context.Termintypen.Add(termintyp);
        _context.Terminstatus.Add(terminstatus);
        _context.Raumtypen.Add(raumtyp);
        await _context.SaveChangesAsync();

        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            GeschlechtId = geschlecht.Id
        };

        var mitarbeiter = new Mitarbeiter
        {
            Vorname = "Dr. Anna",
            Nachname = "Schmidt",
            RolleId = rolle.Id,
            PersonalNummer = "MA001"
        };

        var raum = new Raum
        {
            Bezeichnung = "Behandlungsraum 1",
            Nummer = "R001",
            RaumtypId = raumtyp.Id
        };

        _context.Patienten.Add(patient);
        _context.Mitarbeiter.Add(mitarbeiter);
        _context.Raeume.Add(raum);
        await _context.SaveChangesAsync();

        var termin = new Termin
        {
            PatientId = patient.Id,
            MitarbeiterId = mitarbeiter.Id,
            RaumId = raum.Id,
            TermintypId = termintyp.Id,
            TerminstatusId = terminstatus.Id,
            Datum = DateTime.Today.AddDays(1).AddHours(10),
            DauerMinuten = 60,
            KategorieId = Guid.NewGuid()
        };

        _context.Termine.Add(termin);
        await _context.SaveChangesAsync();

        // Act
        var terminWithRelations = await _context.Termine
            .Include(t => t.Patient)
            .Include(t => t.Mitarbeiter)
            .Include(t => t.Raum)
            .Include(t => t.Termintyp)
            .Include(t => t.Terminstatus)
            .FirstAsync(t => t.Id == termin.Id);

        // Assert
        Assert.That(terminWithRelations.Patient, Is.Not.Null);
        Assert.That(terminWithRelations.Patient.Vorname, Is.EqualTo("Max"));
        Assert.That(terminWithRelations.Mitarbeiter, Is.Not.Null);
        Assert.That(terminWithRelations.Mitarbeiter.Vorname, Is.EqualTo("Dr. Anna"));
        Assert.That(terminWithRelations.Raum, Is.Not.Null);
        Assert.That(terminWithRelations.Raum.Bezeichnung, Is.EqualTo("Behandlungsraum 1"));
        Assert.That(terminWithRelations.Termintyp, Is.Not.Null);
        Assert.That(terminWithRelations.Termintyp.Bezeichnung, Is.EqualTo("Physiotherapie"));
        Assert.That(terminWithRelations.Terminstatus, Is.Not.Null);
        Assert.That(terminWithRelations.Terminstatus.Bezeichnung, Is.EqualTo("Geplant"));
    }

    [Test]
    public async Task AuditFields_ShouldBeSetAutomatically()
    {
        // Arrange
        var geschlecht = new Geschlecht { Bezeichnung = "Männlich", Code = "M" };
        _context.Geschlechter.Add(geschlecht);
        await _context.SaveChangesAsync();

        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            GeschlechtId = geschlecht.Id
        };

        var beforeSave = DateTime.UtcNow;

        // Act
        _context.Patienten.Add(patient);
        await _context.SaveChangesAsync();

        var afterSave = DateTime.UtcNow;

        // Assert
        Assert.That(patient.ErstelltAm, Is.GreaterThanOrEqualTo(beforeSave));
        Assert.That(patient.ErstelltAm, Is.LessThanOrEqualTo(afterSave));
        Assert.That(patient.GeändertAm, Is.GreaterThanOrEqualTo(beforeSave));
        Assert.That(patient.GeändertAm, Is.LessThanOrEqualTo(afterSave));
        Assert.That(patient.RowVersion, Is.Not.Null);
        Assert.That(patient.RowVersion.Length, Is.EqualTo(8));
    }

    [Test]
    public async Task UpdateEntity_ShouldUpdateAuditFields()
    {
        // Arrange
        var geschlecht = new Geschlecht { Bezeichnung = "Männlich", Code = "M" };
        _context.Geschlechter.Add(geschlecht);
        await _context.SaveChangesAsync();

        var patient = new Patient
        {
            Vorname = "Max",
            Nachname = "Mustermann",
            GeschlechtId = geschlecht.Id
        };

        _context.Patienten.Add(patient);
        await _context.SaveChangesAsync();

        var originalGeändertAm = patient.GeändertAm;
        var originalRowVersion = patient.RowVersion;

        // Wait a bit to ensure different timestamp
        await Task.Delay(10);

        // Act
        patient.Vorname = "Maximilian";
        await _context.SaveChangesAsync();

        // Assert
        Assert.That(patient.GeändertAm, Is.GreaterThan(originalGeändertAm));
        Assert.That(patient.RowVersion, Is.Not.EqualTo(originalRowVersion));
    }
} 