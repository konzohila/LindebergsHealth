namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Konfigurationshinweise für Entity Framework
/// Diese Klasse dient als Dokumentation für wichtige EF-Konfigurationen
/// </summary>
public static class EntityConfigurationHints
{
    /// <summary>
    /// Wichtige Indizes für Performance-Optimierung
    /// Diese sollten in der DbContext-Konfiguration implementiert werden
    /// </summary>
    public static class RequiredIndexes
    {
        // Häufig verwendete Suchfelder
        public const string PatientNachname = "Patient.Nachname";
        public const string PatientVorname = "Patient.Vorname";
        public const string PatientGeburtsdatum = "Patient.Geburtsdatum";

        // Termin-Performance
        public const string TerminDatum = "Termin.Datum";
        public const string TerminMitarbeiterId = "Termin.MitarbeiterId";
        public const string TerminPatientId = "Termin.PatientId";
        public const string TerminStatus = "Termin.TerminstatusId";

        // Finanz-Performance
        public const string RechnungDatum = "Rechnung.Rechnungsdatum";
        public const string RechnungPatientId = "Rechnung.PatientId";
        public const string RechnungBezahltAm = "Rechnung.BezahltAm";

        // Adress-Performance
        public const string AdressePostleitzahl = "Adresse.Postleitzahl";
        public const string AdresseOrt = "Adresse.Ort";

        // Soft Delete Performance
        public const string IsDeleted = "*.IsDeleted"; // Auf allen Entities

        // Audit Trail Performance
        public const string CreatedAt = "*.CreatedAt"; // Auf allen Entities
        public const string ModifiedAt = "*.ModifiedAt"; // Auf allen Entities
    }

    /// <summary>
    /// Composite Indizes für komplexe Abfragen
    /// </summary>
    public static class CompositeIndexes
    {
        public const string PatientNameGeburt = "Patient(Nachname, Vorname, Geburtsdatum)";
        public const string TerminDatumMitarbeiter = "Termin(Datum, MitarbeiterId)";
        public const string TerminPatientDatum = "Termin(PatientId, Datum)";
        public const string AdresseOrtPlz = "Adresse(Ort, Postleitzahl)";
        public const string RechnungPatientDatum = "Rechnung(PatientId, Rechnungsdatum)";
    }

    /// <summary>
    /// Unique Constraints für Datenintegrität
    /// </summary>
    public static class UniqueConstraints
    {
        public const string RechnungNummer = "Rechnung.Rechnungsnummer";
        public const string MitarbeiterEmail = "Mitarbeiter.Email";
        public const string KostenstelleCode = "Kostenstelle.Code";
    }

    /// <summary>
    /// Cascade Delete Konfigurationen
    /// </summary>
    public static class CascadeRules
    {
        // Restrict (keine automatische Löschung)
        public const string PatientToTermin = "Patient -> Termin (Restrict)";
        public const string MitarbeiterToTermin = "Mitarbeiter -> Termin (Restrict)";

        // Cascade (automatische Löschung)
        public const string PatientToAdresse = "Patient -> PatientAdresse (Cascade)";
        public const string RechnungToPosition = "Rechnung -> RechnungsPosition (Cascade)";

        // Set Null
        public const string MitarbeiterToKostenstelle = "Mitarbeiter -> Kostenstelle.VerantwortlicherMitarbeiterId (SetNull)";
    }

    /// <summary>
    /// Validierungsregeln für Entities
    /// </summary>
    public static class ValidationRules
    {
        // String Längen
        public const int MaxNameLength = 100;
        public const int MaxEmailLength = 255;
        public const int MaxTelefonLength = 20;
        public const int MaxPostleitzahlLength = 10;
        public const int MaxNotesLength = 2000;

        // Numerische Bereiche
        public const int MinSchmerzSkala = 0;
        public const int MaxSchmerzSkala = 10;
        public const decimal MinBetrag = 0;
        public const decimal MaxBetrag = 999999.99m;

        // Datum Bereiche
        public static readonly DateTime MinGeburtsdatum = new DateTime(1900, 1, 1);
        public static readonly DateTime MaxGeburtsdatum = DateTime.Today;
    }

    /// <summary>
    /// Precision und Scale für Decimal-Felder
    /// </summary>
    public static class DecimalPrecision
    {
        // Finanzbeträge: 18,2 (bis 999.999.999.999.999,99)
        public const int FinanzPrecision = 18;
        public const int FinanzScale = 2;

        // Geo-Koordinaten: 18,6 (ausreichend für GPS-Genauigkeit)
        public const int GeoPrecision = 18;
        public const int GeoScale = 6;

        // Faktoren: 5,3 (bis 99,999)
        public const int FaktorPrecision = 5;
        public const int FaktorScale = 3;
    }
}
