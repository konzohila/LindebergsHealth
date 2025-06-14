using Microsoft.EntityFrameworkCore;
using LindebergsHealth.Domain.Entities;

namespace LindebergsHealth.Infrastructure.Data;

/// <summary>
/// Entity Framework DbContext für Lindebergs Health Praxissoftware
/// Vollständig konfiguriert für Performance, DSGVO-Compliance und deutsche Praxisumgebung
/// </summary>
public class LindebergsHealthDbContext : DbContext
{
    public LindebergsHealthDbContext(DbContextOptions<LindebergsHealthDbContext> options) : base(options)
    {
    }

    #region DbSets - Haupt-Entities

    // Patienten-Management
    public DbSet<Patient> Patienten { get; set; } = null!;
    public DbSet<PatientErweiterung> PatientenErweiterungen { get; set; } = null!;
    public DbSet<PatientVersicherung> PatientenVersicherungen { get; set; } = null!;
    public DbSet<PatientBeziehungsperson> PatientenBeziehungspersonen { get; set; } = null!;
    public DbSet<PatientKommunikation> PatientenKommunikationen { get; set; } = null!;
    public DbSet<PatientEmpfehlung> PatientenEmpfehlungen { get; set; } = null!;
    public DbSet<PatientNotfallkontakt> PatientenNotfallkontakte { get; set; } = null!;

    // Mitarbeiter-Management
    public DbSet<Mitarbeiter> Mitarbeiter { get; set; } = null!;
    public DbSet<MitarbeiterDetails> MitarbeiterDetails { get; set; } = null!;
    public DbSet<MitarbeiterNotfallkontakt> MitarbeiterNotfallkontakte { get; set; } = null!;
    public DbSet<MitarbeiterVertrag> MitarbeiterVerträge { get; set; } = null!;
    public DbSet<MitarbeiterFortbildung> MitarbeiterFortbildungen { get; set; } = null!;

    // Termin-Management
    public DbSet<Termin> Termine { get; set; } = null!;
    public DbSet<Terminvorlage> Terminvorlagen { get; set; } = null!;
    public DbSet<Terminblockierung> Terminblockierungen { get; set; } = null!;
    public DbSet<Terminänderung> Terminänderungen { get; set; } = null!;
    public DbSet<Warteliste> Wartelisten { get; set; } = null!;
    public DbSet<Terminserie> Terminserien { get; set; } = null!;

    // Therapie-Management
    public DbSet<TherapieSerie> Therapieserien { get; set; } = null!;
    public DbSet<TherapieEinheit> TherapieEinheiten { get; set; } = null!;
    public DbSet<TherapeutenCheckliste> TherapeutenChecklisten { get; set; } = null!;
    public DbSet<Koerperstatus> Koerperstatuseinträge { get; set; } = null!;

    // Finanz-Management
    public DbSet<Rechnung> Rechnungen { get; set; } = null!;
    public DbSet<RechnungsPosition> RechnungsPositionen { get; set; } = null!;
    public DbSet<Zahlungseingang> Zahlungseingänge { get; set; } = null!;
    public DbSet<Budget> Budgets { get; set; } = null!;
    public DbSet<BudgetPosition> BudgetPositionen { get; set; } = null!;
    public DbSet<Kostenstelle> Kostenstellen { get; set; } = null!;
    public DbSet<KostenstellenBuchung> KostenstellenBuchungen { get; set; } = null!;
    public DbSet<Steuerdaten> Steuerdaten { get; set; } = null!;
    public DbSet<Gehalt> Gehälter { get; set; } = null!;

    // CRM-Management
    public DbSet<CRMStatus> CRMStatus { get; set; } = null!;
    public DbSet<CRMNetzwerk> CRMNetzwerke { get; set; } = null!;

    // Praxis-Management
    public DbSet<Praxis> Praxen { get; set; } = null!;
    public DbSet<Raum> Räume { get; set; } = null!;
    public DbSet<Ausstattung> Ausstattungen { get; set; } = null!;
    public DbSet<Kooperationspartner> Kooperationspartner { get; set; } = null!;

    // Dokument-Management
    public DbSet<Dokument> Dokumente { get; set; } = null!;
    public DbSet<Einwilligung> Einwilligungen { get; set; } = null!;
    public DbSet<Kommunikationsverlauf> Kommunikationsverläufe { get; set; } = null!;

    // Adress- und Kontakt-Management
    public DbSet<Adresse> Adressen { get; set; } = null!;
    public DbSet<Kontakt> Kontakte { get; set; } = null!;
    public DbSet<PatientAdresse> PatientenAdressen { get; set; } = null!;
    public DbSet<PatientKontakt> PatientenKontakte { get; set; } = null!;
    public DbSet<MitarbeiterAdresse> MitarbeiterAdressen { get; set; } = null!;
    public DbSet<MitarbeiterKontakt> MitarbeiterKontakte { get; set; } = null!;
    public DbSet<KooperationspartnerAdresse> KooperationspartnerAdressen { get; set; } = null!;
    public DbSet<KooperationspartnerKontakt> KooperationspartnerKontakte { get; set; } = null!;

    #endregion

    #region DbSets - Lookup-Entities

    public DbSet<Geschlecht> Geschlechter { get; set; } = null!;
    public DbSet<Familienstand> Familienstände { get; set; } = null!;
    public DbSet<Versicherungstyp> Versicherungstypen { get; set; } = null!;
    public DbSet<Beziehungstyp> Beziehungstypen { get; set; } = null!;
    public DbSet<Kommunikationstyp> Kommunikationstypen { get; set; } = null!;
    public DbSet<Empfehlungstyp> Empfehlungstypen { get; set; } = null!;
    public DbSet<Termintyp> Termintypen { get; set; } = null!;
    public DbSet<Terminstatus> Terminstatusarten { get; set; } = null!;
    public DbSet<Therapietyp> Therapietypen { get; set; } = null!;
    public DbSet<Therapiestatus> Therapiestatusarten { get; set; } = null!;
    public DbSet<Schmerztyp> Schmerztypen { get; set; } = null!;
    public DbSet<Rechnungsstatus> Rechnungsstatusarten { get; set; } = null!;
    public DbSet<CRMStatusTyp> CRMStatusTypen { get; set; } = null!;
    public DbSet<Netzwerktyp> Netzwerktypen { get; set; } = null!;
    public DbSet<Raumtyp> Raumtypen { get; set; } = null!;
    public DbSet<Ausstattungstyp> Ausstattungstypen { get; set; } = null!;
    public DbSet<Kooperationstyp> Kooperationstypen { get; set; } = null!;
    public DbSet<Dokumenttyp> Dokumenttypen { get; set; } = null!;
    public DbSet<Einwilligungstyp> Einwilligungstypen { get; set; } = null!;
    public DbSet<Adresstyp> Adresstypen { get; set; } = null!;
    public DbSet<Kontakttyp> Kontakttypen { get; set; } = null!;
    public DbSet<Leistungstyp> Leistungstypen { get; set; } = null!;
    public DbSet<Zahlungsart> Zahlungsarten { get; set; } = null!;
    public DbSet<Budgetkategorie> Budgetkategorien { get; set; } = null!;
    public DbSet<Kostentyp> Kostentypen { get; set; } = null!;
    public DbSet<Buchungsart> Buchungsarten { get; set; } = null!;
    public DbSet<Blockierungsgrund> Blockierungsgründe { get; set; } = null!;
    public DbSet<Änderungsgrund> Änderungsgründe { get; set; } = null!;
    public DbSet<Priorität> Prioritäten { get; set; } = null!;

    #endregion

    #region DbSets - History-Entities

    public DbSet<PatientHistory> PatientenHistory { get; set; } = null!;
    public DbSet<MitarbeiterDetailsHistory> MitarbeiterDetailsHistory { get; set; } = null!;
    public DbSet<TerminHistory> TermineHistory { get; set; } = null!;
    public DbSet<RechnungHistory> RechnungenHistory { get; set; } = null!;
    public DbSet<AdresseHistory> AdressenHistory { get; set; } = null!;
    public DbSet<KontaktHistory> KontakteHistory { get; set; } = null!;
    // Weitere History-Entities...

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Alle Entity-Konfigurationen aus separaten Klassen anwenden
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LindebergsHealthDbContext).Assembly);
        
        // Seed Data laden
        SeedLookupData(modelBuilder);
    }

    #region Seed Data

    private void SeedLookupData(ModelBuilder modelBuilder)
    {
        // Mitarbeiter Haupt-Entity
        modelBuilder.Entity<Mitarbeiter>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Vorname).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Nachname).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            
            // Unique Constraints
            entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("IX_Mitarbeiter_Email_Unique");
            
            // Performance Indizes
            entity.HasIndex(e => e.Nachname).HasDatabaseName("IX_Mitarbeiter_Nachname");
            entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_Mitarbeiter_IsActive");
        });

        // Mitarbeiter Details
        modelBuilder.Entity<MitarbeiterDetails>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Sozialversicherungsnummer).HasMaxLength(50);
            entity.Property(e => e.SteuerId).HasMaxLength(50);
            
            entity.HasOne(e => e.Mitarbeiter)
                  .WithOne(m => m.Details)
                  .HasForeignKey<MitarbeiterDetails>(e => e.MitarbeiterId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureTerminEntities(ModelBuilder modelBuilder)
    {
        // Termin Haupt-Entity
        modelBuilder.Entity<Termin>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Titel).HasMaxLength(200);
            entity.Property(e => e.Beschreibung).HasMaxLength(1000);
            entity.Property(e => e.Notizen).HasMaxLength(2000);
            
            // Performance Indizes
            entity.HasIndex(e => e.Datum).HasDatabaseName("IX_Termin_Datum");
            entity.HasIndex(e => new { e.Datum, e.MitarbeiterId }).HasDatabaseName("IX_Termin_DatumMitarbeiter");
            entity.HasIndex(e => new { e.PatientId, e.Datum }).HasDatabaseName("IX_Termin_PatientDatum");
            entity.HasIndex(e => e.TerminstatusId).HasDatabaseName("IX_Termin_Status");
            
            // Beziehungen
            entity.HasOne(e => e.Patient)
                  .WithMany(p => p.Termine)
                  .HasForeignKey(e => e.PatientId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasOne(e => e.Mitarbeiter)
                  .WithMany(m => m.Termine)
                  .HasForeignKey(e => e.MitarbeiterId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Terminvorlage
        modelBuilder.Entity<Terminvorlage>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Bezeichnung).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Beschreibung).HasMaxLength(1000);
            entity.Property(e => e.Farbe).HasMaxLength(7); // Hex-Farbcode
            
            // Decimal Precision
            entity.Property(e => e.StandardPreis).HasPrecision(18, 2);
        });

        // Warteliste
        modelBuilder.Entity<Warteliste>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Notizen).HasMaxLength(1000);
            
            // Performance Index
            entity.HasIndex(e => new { e.IstAktiv, e.EingetragenenAm }).HasDatabaseName("IX_Warteliste_AktivDatum");
        });
    }

    private void ConfigureFinanzEntities(ModelBuilder modelBuilder)
    {
        // Rechnung
        modelBuilder.Entity<Rechnung>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Rechnungsnummer).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Beschreibung).HasMaxLength(1000);
            
            // Decimal Precision für Geldbeträge
            entity.Property(e => e.Betrag).HasPrecision(18, 2);
            entity.Property(e => e.Steuerbetrag).HasPrecision(18, 2);
            entity.Property(e => e.Gesamtbetrag).HasPrecision(18, 2);
            
            // Unique Constraint
            entity.HasIndex(e => e.Rechnungsnummer).IsUnique().HasDatabaseName("IX_Rechnung_Nummer_Unique");
            
            // Performance Indizes
            entity.HasIndex(e => e.Rechnungsdatum).HasDatabaseName("IX_Rechnung_Datum");
            entity.HasIndex(e => e.PatientId).HasDatabaseName("IX_Rechnung_Patient");
            entity.HasIndex(e => e.RechnungsstatusId).HasDatabaseName("IX_Rechnung_Status");
        });

        // RechnungsPosition
        modelBuilder.Entity<RechnungsPosition>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Bezeichnung).HasMaxLength(200).IsRequired();
            entity.Property(e => e.GOÄ_Ziffer).HasMaxLength(20);
            entity.Property(e => e.Beschreibung).HasMaxLength(1000);
            
            // Decimal Precision
            entity.Property(e => e.Einzelpreis).HasPrecision(18, 2);
            entity.Property(e => e.Faktor).HasPrecision(5, 2);
            entity.Property(e => e.Gesamtpreis).HasPrecision(18, 2);
        });

        // Budget
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Notizen).HasMaxLength(1000);
            
            // Decimal Precision
            entity.Property(e => e.GeplanteBetrag).HasPrecision(18, 2);
            entity.Property(e => e.TatsächlicherBetrag).HasPrecision(18, 2);
            
            // Performance Index
            entity.HasIndex(e => new { e.Jahr, e.Monat }).HasDatabaseName("IX_Budget_JahrMonat");
        });

        // Kostenstelle
        modelBuilder.Entity<Kostenstelle>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Code).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Bezeichnung).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Beschreibung).HasMaxLength(1000);
            
            // Unique Constraint
            entity.HasIndex(e => e.Code).IsUnique().HasDatabaseName("IX_Kostenstelle_Code_Unique");
        });

        // Gehalt
        modelBuilder.Entity<Gehalt>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Decimal Precision für Geldbeträge
            entity.Property(e => e.Grundgehalt).HasPrecision(18, 2);
            entity.Property(e => e.Zulagen).HasPrecision(18, 2);
            entity.Property(e => e.Abzuege).HasPrecision(18, 2);
            entity.Property(e => e.Nettogehalt).HasPrecision(18, 2);
            entity.Property(e => e.Steuern).HasPrecision(18, 2);
            entity.Property(e => e.Sozialversicherung).HasPrecision(18, 2);
            
            // Performance Index
            entity.HasIndex(e => new { e.MitarbeiterId, e.Jahr, e.Monat }).HasDatabaseName("IX_Gehalt_MitarbeiterJahrMonat");
        });
    }

    private void ConfigureAdressKontaktEntities(ModelBuilder modelBuilder)
    {
        // Adresse
        modelBuilder.Entity<Adresse>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Strasse).HasMaxLength(200);
            entity.Property(e => e.Hausnummer).HasMaxLength(20);
            entity.Property(e => e.Zusatz).HasMaxLength(100);
            entity.Property(e => e.Postleitzahl).HasMaxLength(10);
            entity.Property(e => e.Ort).HasMaxLength(100);
            entity.Property(e => e.Land).HasMaxLength(100);
            entity.Property(e => e.ValidationSource).HasMaxLength(100);
            
            // Decimal Precision für Geo-Koordinaten
            entity.Property(e => e.Latitude).HasPrecision(18, 6);
            entity.Property(e => e.Longitude).HasPrecision(18, 6);
            
            // Performance Indizes
            entity.HasIndex(e => e.Postleitzahl).HasDatabaseName("IX_Adresse_PLZ");
            entity.HasIndex(e => new { e.Postleitzahl, e.Ort }).HasDatabaseName("IX_Adresse_PLZOrt");
        });

        // Kontakt
        modelBuilder.Entity<Kontakt>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Wert).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Notizen).HasMaxLength(1000);
            
            // Performance Index
            entity.HasIndex(e => e.KontakttypId).HasDatabaseName("IX_Kontakt_Typ");
        });

        // PatientAdresse
        modelBuilder.Entity<PatientAdresse>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Composite Index für Performance
            entity.HasIndex(e => new { e.PatientId, e.IstHauptadresse }).HasDatabaseName("IX_PatientAdresse_PatientHaupt");
        });

        // PatientKontakt
        modelBuilder.Entity<PatientKontakt>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Composite Index für Performance
            entity.HasIndex(e => new { e.PatientId, e.IstHauptkontakt }).HasDatabaseName("IX_PatientKontakt_PatientHaupt");
        });
    }

    private void ConfigureLookupEntities(ModelBuilder modelBuilder)
    {
        // Alle Lookup-Entities haben ähnliche Konfiguration
        var lookupTypes = new[]
        {
            typeof(Geschlecht), typeof(Familienstand), typeof(Versicherungstyp),
            typeof(Beziehungstyp), typeof(Kommunikationstyp), typeof(Empfehlungstyp),
            typeof(Termintyp), typeof(Terminstatus), typeof(Therapietyp),
            typeof(Therapiestatus), typeof(Schmerztyp), typeof(Rechnungsstatus),
            typeof(CRMStatusTyp), typeof(Netzwerktyp), typeof(Raumtyp),
            typeof(Ausstattungstyp), typeof(Kooperationstyp), typeof(Dokumenttyp),
            typeof(Einwilligungstyp), typeof(Adresstyp), typeof(Kontakttyp),
            typeof(Leistungstyp), typeof(Zahlungsart), typeof(Budgetkategorie),
            typeof(Kostentyp), typeof(Buchungsart), typeof(Blockierungsgrund),
            typeof(Änderungsgrund), typeof(Priorität)
        };

        foreach (var lookupType in lookupTypes)
        {
            modelBuilder.Entity(lookupType, entity =>
            {
                entity.Property("Name").HasMaxLength(100).IsRequired();
                entity.Property("Beschreibung").HasMaxLength(500);
                
                // Performance Index auf Name
                entity.HasIndex("Name").HasDatabaseName($"IX_{lookupType.Name}_Name");
                
                // Unique Constraint auf Name
                entity.HasIndex("Name").IsUnique().HasDatabaseName($"IX_{lookupType.Name}_Name_Unique");
            });
        }
    }

    private void ConfigureHistoryEntities(ModelBuilder modelBuilder)
    {
        // History-Entities haben spezielle Konfiguration
        modelBuilder.Entity<PatientHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Performance Index für History-Abfragen
            entity.HasIndex(e => new { e.OriginalId, e.GültigVon }).HasDatabaseName("IX_PatientHistory_OriginalGueltigVon");
            entity.HasIndex(e => e.GültigVon).HasDatabaseName("IX_PatientHistory_GueltigVon");
        });

        // Weitere History-Entities ähnlich konfigurieren...
    }

    private void ConfigurePerformanceIndexes(ModelBuilder modelBuilder)
    {
        // Zusätzliche Performance-Indizes für häufige Abfragen
        
        // Soft Delete Performance
        modelBuilder.Entity<Patient>()
            .HasIndex(e => e.IstGelöscht)
            .HasDatabaseName("IX_Patient_IstGeloescht");
            
        modelBuilder.Entity<Mitarbeiter>()
            .HasIndex(e => e.IstGelöscht)
            .HasDatabaseName("IX_Mitarbeiter_IstGeloescht");
            
        modelBuilder.Entity<Termin>()
            .HasIndex(e => e.IstGelöscht)
            .HasDatabaseName("IX_Termin_IstGeloescht");

        // Audit Trail Performance
        modelBuilder.Entity<Patient>()
            .HasIndex(e => e.ErstelltAm)
            .HasDatabaseName("IX_Patient_ErstelltAm");
            
        modelBuilder.Entity<Patient>()
            .HasIndex(e => e.GeändertAm)
            .HasDatabaseName("IX_Patient_GeaendertAm");

        // Composite Indizes für häufige Abfragen
        modelBuilder.Entity<Termin>()
            .HasIndex(e => new { e.IstGelöscht, e.Datum, e.MitarbeiterId })
            .HasDatabaseName("IX_Termin_GeloeschtDatumMitarbeiter");
            
        modelBuilder.Entity<Rechnung>()
            .HasIndex(e => new { e.IstGelöscht, e.PatientId, e.Rechnungsdatum })
            .HasDatabaseName("IX_Rechnung_GeloeschtPatientDatum");
    }

    private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
    {
        // Global Query Filter für Soft Delete
        modelBuilder.Entity<Patient>().HasQueryFilter(e => !e.IstGelöscht);
        modelBuilder.Entity<Mitarbeiter>().HasQueryFilter(e => !e.IstGelöscht);
        modelBuilder.Entity<Termin>().HasQueryFilter(e => !e.IstGelöscht);
        modelBuilder.Entity<Rechnung>().HasQueryFilter(e => !e.IstGelöscht);
        modelBuilder.Entity<Adresse>().HasQueryFilter(e => !e.IstGelöscht);
        modelBuilder.Entity<Kontakt>().HasQueryFilter(e => !e.IstGelöscht);
        
        // Weitere Entities mit Soft Delete...
    }



    #endregion
} 