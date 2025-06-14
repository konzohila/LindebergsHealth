namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Rechnung-Entity
/// </summary>
public class Rechnung : BaseEntity
{
    public bool IsDeleted { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    public decimal Steuerbetrag { get; set; }
    public decimal Gesamtbetrag { get; set; }
    public Guid RechnungsstatusId { get; set; }
    public Guid PatientId { get; set; }
    public Guid TerminId { get; set; }
    public decimal Betrag { get; set; }
    public DateTime? BezahltAm { get; set; }

    // Rechnungsdetails
    public string Rechnungsnummer { get; set; } = string.Empty;
    public DateTime Rechnungsdatum { get; set; } = DateTime.UtcNow;
    public DateTime? Fälligkeitsdatum { get; set; }
    public string Notizen { get; set; } = string.Empty;

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
    public Termin Termin { get; set; } = null!;

    // Erweiterte Finanz-Navigation Properties
    public ICollection<RechnungsPosition> Positionen { get; set; } = new List<RechnungsPosition>();
    public ICollection<Zahlungseingang> Zahlungseingänge { get; set; } = new List<Zahlungseingang>();
    public Steuerdaten? Steuerdaten { get; set; }
}

/// <summary>
/// Rechnung-Historisierung
/// </summary>
public class RechnungHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    public Guid TerminId { get; set; }
    public decimal Betrag { get; set; }
    public DateTime? BezahltAm { get; set; }

    // Rechnungsdetails
    public string Rechnungsnummer { get; set; } = string.Empty;
    public DateTime Rechnungsdatum { get; set; }
    public DateTime? Fälligkeitsdatum { get; set; }
    public string Notizen { get; set; } = string.Empty;
}

/// <summary>
/// Gehalt-Entity
/// </summary>
public class Gehalt : BaseEntity
{
    public decimal Zulagen { get; set; }
    public decimal Abzuege { get; set; }
    public decimal Nettogehalt { get; set; }
    public decimal Steuern { get; set; }
    public decimal Sozialversicherung { get; set; }
    public Guid MitarbeiterId { get; set; }
    public int Monat { get; set; } // 1-12
    public int Jahr { get; set; }
    public decimal Grundgehalt { get; set; }
    public decimal Bonus { get; set; }

    // Navigation Properties
    public Mitarbeiter Mitarbeiter { get; set; } = null!;
}

/// <summary>
/// Gehalt-Historisierung
/// </summary>
public class GehaltHistory : BaseHistoryEntity
{
    public Guid MitarbeiterId { get; set; }
    public int Monat { get; set; }
    public int Jahr { get; set; }
    public decimal Grundgehalt { get; set; }
    public decimal Bonus { get; set; }
}

/// <summary>
/// Finanzposition für Budgetplanung
/// </summary>
public class FinanzPosition : BaseEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid FinanzKategorieId { get; set; }
    public FinanzKategorie FinanzKategorie { get; set; } = null!;

    public Guid FinanzTypId { get; set; }
    public FinanzTyp FinanzTyp { get; set; } = null!;

    // Finanzdaten
    public string Bezeichnung { get; set; } = string.Empty; // GEHÄLTER
    public int Monat { get; set; } // 1-12
    public int Jahr { get; set; }
    public decimal Betrag { get; set; }
    public string Quelle { get; set; } = string.Empty; // Cash Flow Tabelle
}

/// <summary>
/// FinanzPosition-Historisierung
/// </summary>
public class FinanzPositionHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid FinanzKategorieId { get; set; }
    public FinanzKategorie FinanzKategorie { get; set; } = null!;

    public Guid FinanzTypId { get; set; }
    public FinanzTyp FinanzTyp { get; set; } = null!;

    // Finanzdaten
    public string Bezeichnung { get; set; } = string.Empty;
    public int Monat { get; set; }
    public int Jahr { get; set; }
    public decimal Betrag { get; set; }
    public string Quelle { get; set; } = string.Empty;
}
