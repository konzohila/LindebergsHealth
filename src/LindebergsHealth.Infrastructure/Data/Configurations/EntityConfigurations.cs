using LindebergsHealth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LindebergsHealth.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework Konfigurationen für bessere Wartbarkeit
/// Getrennte Konfigurationen für verschiedene Entity-Gruppen
/// </summary>

#region Patient Konfigurationen

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(e => e.Id);

        // String-Längen
        builder.Property(e => e.Vorname).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Nachname).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(255);
        builder.Property(e => e.Telefon).HasMaxLength(50);
        builder.Property(e => e.Versicherungsnummer).HasMaxLength(50);

        // Indizes für Performance
        builder.HasIndex(e => e.Nachname).HasDatabaseName("IX_Patient_Nachname");
        builder.HasIndex(e => new { e.Nachname, e.Vorname }).HasDatabaseName("IX_Patient_Name");
        builder.HasIndex(e => new { e.Nachname, e.Vorname, e.Geburtsdatum }).HasDatabaseName("IX_Patient_NameGeburt");
        builder.HasIndex(e => e.Versicherungsnummer).HasDatabaseName("IX_Patient_Versicherungsnummer");
        builder.HasIndex(e => e.Email).HasDatabaseName("IX_Patient_Email");
        builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_Patient_IsDeleted");
        builder.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_Patient_CreatedAt");
        builder.HasIndex(e => e.ModifiedAt).HasDatabaseName("IX_Patient_ModifiedAt");

        // Beziehungen
        builder.HasOne(e => e.Geschlecht)
               .WithMany()
               .HasForeignKey(e => e.GeschlechtId)
               .OnDelete(DeleteBehavior.Restrict);

        // Global Query Filter für Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class PatientErweiterungConfiguration : IEntityTypeConfiguration<PatientErweiterung>
{
    public void Configure(EntityTypeBuilder<PatientErweiterung> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Beruf).HasMaxLength(100);
        builder.Property(e => e.Arbeitgeber).HasMaxLength(200);
        builder.Property(e => e.Notizen).HasMaxLength(2000);

        builder.HasOne(e => e.Patient)
               .WithOne(p => p.Erweiterung)
               .HasForeignKey<PatientErweiterung>(e => e.PatientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Familienstand)
               .WithMany()
               .HasForeignKey(e => e.FamilienstandId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PatientVersicherungConfiguration : IEntityTypeConfiguration<PatientVersicherung>
{
    public void Configure(EntityTypeBuilder<PatientVersicherung> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Versicherungsname).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Versicherungsnummer).HasMaxLength(50).IsRequired();

        builder.HasOne(e => e.Patient)
               .WithMany(p => p.Versicherungen)
               .HasForeignKey(e => e.PatientId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Versicherungstyp)
               .WithMany()
               .HasForeignKey(e => e.VersicherungstypId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

#endregion

#region Mitarbeiter Konfigurationen

public class MitarbeiterConfiguration : IEntityTypeConfiguration<Mitarbeiter>
{
    public void Configure(EntityTypeBuilder<Mitarbeiter> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Vorname).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Nachname).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(255).IsRequired();

        // Unique Constraints
        builder.HasIndex(e => e.Email).IsUnique().HasDatabaseName("IX_Mitarbeiter_Email_Unique");

        // Performance Indizes
        builder.HasIndex(e => e.Nachname).HasDatabaseName("IX_Mitarbeiter_Nachname");
        builder.HasIndex(e => e.IsActive).HasDatabaseName("IX_Mitarbeiter_IsActive");
        builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_Mitarbeiter_IsDeleted");

        // Global Query Filter für Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class MitarbeiterDetailsConfiguration : IEntityTypeConfiguration<MitarbeiterDetails>
{
    public void Configure(EntityTypeBuilder<MitarbeiterDetails> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Sozialversicherungsnummer).HasMaxLength(50);
        builder.Property(e => e.SteuerId).HasMaxLength(50);

        builder.HasOne(e => e.Mitarbeiter)
               .WithOne(m => m.Details)
               .HasForeignKey<MitarbeiterDetails>(e => e.MitarbeiterId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

#endregion

#region Termin Konfigurationen

public class TerminConfiguration : IEntityTypeConfiguration<Termin>
{
    public void Configure(EntityTypeBuilder<Termin> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Titel).HasMaxLength(200);
        builder.Property(e => e.Beschreibung).HasMaxLength(1000);
        builder.Property(e => e.Notizen).HasMaxLength(2000);

        // Performance Indizes
        builder.HasIndex(e => e.Datum).HasDatabaseName("IX_Termin_Datum");
        builder.HasIndex(e => new { e.Datum, e.MitarbeiterId }).HasDatabaseName("IX_Termin_DatumMitarbeiter");
        builder.HasIndex(e => new { e.PatientId, e.Datum }).HasDatabaseName("IX_Termin_PatientDatum");
        builder.HasIndex(e => e.TerminstatusId).HasDatabaseName("IX_Termin_Status");
        builder.HasIndex(e => e.IsDeleted).HasDatabaseName("IX_Termin_IsDeleted");
        builder.HasIndex(e => new { e.IsDeleted, e.Datum, e.MitarbeiterId }).HasDatabaseName("IX_Termin_DeletedDatumMitarbeiter");

        // Beziehungen
        builder.HasOne(e => e.Patient)
               .WithMany(p => p.Termine)
               .HasForeignKey(e => e.PatientId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Mitarbeiter)
               .WithMany(m => m.Termine)
               .HasForeignKey(e => e.MitarbeiterId)
               .OnDelete(DeleteBehavior.Restrict);

        // Global Query Filter für Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class TerminvorlageConfiguration : IEntityTypeConfiguration<Terminvorlage>
{
    public void Configure(EntityTypeBuilder<Terminvorlage> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Bezeichnung).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Beschreibung).HasMaxLength(1000);
        builder.Property(e => e.Farbe).HasMaxLength(7); // Hex-Farbcode

        // Decimal Precision
        builder.Property(e => e.StandardPreis).HasPrecision(18, 2);
    }
}

public class WartlisteConfiguration : IEntityTypeConfiguration<Warteliste>
{
    public void Configure(EntityTypeBuilder<Warteliste> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Notizen).HasMaxLength(1000);

        // Performance Index
        builder.HasIndex(e => new { e.IstAktiv, e.EingetragenenAm }).HasDatabaseName("IX_Warteliste_AktivDatum");
    }
}

#endregion

#region Finanz Konfigurationen

public class RechnungConfiguration : IEntityTypeConfiguration<Rechnung>
{
    public void Configure(EntityTypeBuilder<Rechnung> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Rechnungsnummer).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Beschreibung).HasMaxLength(1000);

        // Decimal Precision für Geldbeträge
        builder.Property(e => e.Betrag).HasPrecision(18, 2);
        builder.Property(e => e.Steuerbetrag).HasPrecision(18, 2);
        builder.Property(e => e.Gesamtbetrag).HasPrecision(18, 2);

        // Unique Constraint
        builder.HasIndex(e => e.Rechnungsnummer).IsUnique().HasDatabaseName("IX_Rechnung_Nummer_Unique");

        // Performance Indizes
        builder.HasIndex(e => e.Rechnungsdatum).HasDatabaseName("IX_Rechnung_Datum");
        builder.HasIndex(e => e.PatientId).HasDatabaseName("IX_Rechnung_Patient");
        builder.HasIndex(e => e.RechnungsstatusId).HasDatabaseName("IX_Rechnung_Status");
        builder.HasIndex(e => new { e.IsDeleted, e.PatientId, e.Rechnungsdatum }).HasDatabaseName("IX_Rechnung_DeletedPatientDatum");

        // Global Query Filter für Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class RechnungsPositionConfiguration : IEntityTypeConfiguration<RechnungsPosition>
{
    public void Configure(EntityTypeBuilder<RechnungsPosition> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Bezeichnung).HasMaxLength(200).IsRequired();
        builder.Property(e => e.GOÄ_Ziffer).HasMaxLength(20);
        builder.Property(e => e.Beschreibung).HasMaxLength(1000);

        // Decimal Precision
        builder.Property(e => e.Einzelpreis).HasPrecision(18, 2);
        builder.Property(e => e.Faktor).HasPrecision(5, 2);
        builder.Property(e => e.Gesamtpreis).HasPrecision(18, 2);
    }
}

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Notizen).HasMaxLength(1000);

        // Decimal Precision
        builder.Property(e => e.GeplanteBetrag).HasPrecision(18, 2);
        builder.Property(e => e.TatsächlicherBetrag).HasPrecision(18, 2);

        // Performance Index
        builder.HasIndex(e => new { e.Jahr, e.Monat }).HasDatabaseName("IX_Budget_JahrMonat");
    }
}

public class KostenstelleConfiguration : IEntityTypeConfiguration<Kostenstelle>
{
    public void Configure(EntityTypeBuilder<Kostenstelle> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code).HasMaxLength(20).IsRequired();
        builder.Property(e => e.Bezeichnung).HasMaxLength(200).IsRequired();
        builder.Property(e => e.Beschreibung).HasMaxLength(1000);

        // Unique Constraint
        builder.HasIndex(e => e.Code).IsUnique().HasDatabaseName("IX_Kostenstelle_Code_Unique");
    }
}

public class GehaltConfiguration : IEntityTypeConfiguration<Gehalt>
{
    public void Configure(EntityTypeBuilder<Gehalt> builder)
    {
        builder.HasKey(e => e.Id);

        // Decimal Precision für Geldbeträge
        builder.Property(e => e.Grundgehalt).HasPrecision(18, 2);
        builder.Property(e => e.Zulagen).HasPrecision(18, 2);
        builder.Property(e => e.Abzuege).HasPrecision(18, 2);
        builder.Property(e => e.Nettogehalt).HasPrecision(18, 2);
        builder.Property(e => e.Steuern).HasPrecision(18, 2);
        builder.Property(e => e.Sozialversicherung).HasPrecision(18, 2);

        // Performance Index
        builder.HasIndex(e => new { e.MitarbeiterId, e.Jahr, e.Monat }).HasDatabaseName("IX_Gehalt_MitarbeiterJahrMonat");
    }
}

#endregion

#region Adress- und Kontakt Konfigurationen

public class AdresseConfiguration : IEntityTypeConfiguration<Adresse>
{
    public void Configure(EntityTypeBuilder<Adresse> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Strasse).HasMaxLength(200);
        builder.Property(e => e.Hausnummer).HasMaxLength(20);
        builder.Property(e => e.Zusatz).HasMaxLength(100);
        builder.Property(e => e.Postleitzahl).HasMaxLength(10);
        builder.Property(e => e.Ort).HasMaxLength(100);
        builder.Property(e => e.Land).HasMaxLength(100);
        builder.Property(e => e.ValidationSource).HasMaxLength(100);

        // Decimal Precision für Geo-Koordinaten
        builder.Property(e => e.Latitude).HasPrecision(18, 6);
        builder.Property(e => e.Longitude).HasPrecision(18, 6);

        // Performance Indizes
        builder.HasIndex(e => e.Postleitzahl).HasDatabaseName("IX_Adresse_PLZ");
        builder.HasIndex(e => new { e.Postleitzahl, e.Ort }).HasDatabaseName("IX_Adresse_PLZOrt");

        // Global Query Filter für Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class KontaktConfiguration : IEntityTypeConfiguration<Kontakt>
{
    public void Configure(EntityTypeBuilder<Kontakt> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Wert).HasMaxLength(255).IsRequired();
        builder.Property(e => e.Notizen).HasMaxLength(1000);

        // Performance Index
        builder.HasIndex(e => e.KontakttypId).HasDatabaseName("IX_Kontakt_Typ");

        // Global Query Filter für Soft Delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

public class PatientAdresseConfiguration : IEntityTypeConfiguration<PatientAdresse>
{
    public void Configure(EntityTypeBuilder<PatientAdresse> builder)
    {
        builder.HasKey(e => e.Id);

        // Composite Index für Performance
        builder.HasIndex(e => new { e.PatientId, e.IstHauptadresse }).HasDatabaseName("IX_PatientAdresse_PatientHaupt");
    }
}

public class PatientKontaktConfiguration : IEntityTypeConfiguration<PatientKontakt>
{
    public void Configure(EntityTypeBuilder<PatientKontakt> builder)
    {
        builder.HasKey(e => e.Id);

        // Composite Index für Performance
        builder.HasIndex(e => new { e.PatientId, e.IstHauptkontakt }).HasDatabaseName("IX_PatientKontakt_PatientHaupt");
    }
}

#endregion

#region Lookup Konfigurationen

public class LookupEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property<string>("Name").HasMaxLength(100).IsRequired();
        builder.Property<string>("Beschreibung").HasMaxLength(500);

        // Performance Index auf Name
        builder.HasIndex("Name").HasDatabaseName($"IX_{typeof(T).Name}_Name");

        // Unique Constraint auf Name
        builder.HasIndex("Name").IsUnique().HasDatabaseName($"IX_{typeof(T).Name}_Name_Unique");
    }
}

// Spezifische Lookup-Konfigurationen
public class GeschlechtConfiguration : LookupEntityConfiguration<Geschlecht> { }
public class FamilienstandConfiguration : LookupEntityConfiguration<Familienstand> { }
public class VersicherungstypConfiguration : LookupEntityConfiguration<Versicherungstyp> { }
public class BeziehungstypConfiguration : LookupEntityConfiguration<Beziehungstyp> { }
public class KommunikationstypConfiguration : LookupEntityConfiguration<Kommunikationstyp> { }
public class EmpfehlungstypConfiguration : LookupEntityConfiguration<Empfehlungstyp> { }
public class TermintypConfiguration : LookupEntityConfiguration<Termintyp> { }
public class TerminstatusConfiguration : LookupEntityConfiguration<Terminstatus> { }
public class TherapietypConfiguration : LookupEntityConfiguration<Therapietyp> { }
public class TherapiestatusConfiguration : LookupEntityConfiguration<Therapiestatus> { }
public class SchmerztypConfiguration : LookupEntityConfiguration<Schmerztyp> { }
public class RechnungsstatusConfiguration : LookupEntityConfiguration<Rechnungsstatus> { }
public class CRMStatusTypConfiguration : LookupEntityConfiguration<CRMStatusTyp> { }
public class NetzwerktypConfiguration : LookupEntityConfiguration<Netzwerktyp> { }
public class RaumtypConfiguration : LookupEntityConfiguration<Raumtyp> { }
public class AusstattungstypConfiguration : LookupEntityConfiguration<Ausstattungstyp> { }
public class KooperationstypConfiguration : LookupEntityConfiguration<Kooperationstyp> { }
public class DokumenttypConfiguration : LookupEntityConfiguration<Dokumenttyp> { }
public class EinwilligungstypConfiguration : LookupEntityConfiguration<Einwilligungstyp> { }
public class AdresstypConfiguration : LookupEntityConfiguration<Adresstyp> { }
public class KontakttypConfiguration : LookupEntityConfiguration<Kontakttyp> { }
public class LeistungstypConfiguration : LookupEntityConfiguration<Leistungstyp> { }
public class ZahlungsartConfiguration : LookupEntityConfiguration<Zahlungsart> { }
public class BudgetkategorieConfiguration : LookupEntityConfiguration<Budgetkategorie> { }
public class KostentypConfiguration : LookupEntityConfiguration<Kostentyp> { }
public class BuchungsartConfiguration : LookupEntityConfiguration<Buchungsart> { }
public class BlockierungsgrundConfiguration : LookupEntityConfiguration<Blockierungsgrund> { }
public class ÄnderungsgrundConfiguration : LookupEntityConfiguration<Änderungsgrund> { }
public class PrioritätConfiguration : LookupEntityConfiguration<Priorität> { }

#endregion
