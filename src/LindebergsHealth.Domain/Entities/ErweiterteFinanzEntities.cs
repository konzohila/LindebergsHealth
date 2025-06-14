namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Erweiterte Rechnungsposition für detaillierte Abrechnung
/// </summary>
public class RechnungsPosition : BaseEntity
{
    public Guid RechnungId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid LeistungstypId { get; set; }
    public virtual Leistungstyp Leistungstyp { get; set; } = null!;
    
    // Positionsdaten
    public string Bezeichnung { get; set; } = string.Empty;
    public string GOÄ_Ziffer { get; set; } = string.Empty; // Gebührenordnung für Ärzte
    public decimal Einzelpreis { get; set; }
    public int Anzahl { get; set; } = 1;
    public decimal Faktor { get; set; } = 1.0m; // GOÄ-Faktor
    public decimal Gesamtpreis { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual Rechnung Rechnung { get; set; } = null!;
}

/// <summary>
/// RechnungsPosition-Historisierung
/// </summary>
public class RechnungsPositionHistory : BaseHistoryEntity
{
    public Guid RechnungId { get; set; }
    public Guid LeistungstypId { get; set; }
    public Leistungstyp Leistungstyp { get; set; } = null!;
    public string Bezeichnung { get; set; } = string.Empty;
    public string GOÄ_Ziffer { get; set; } = string.Empty;
    public decimal Einzelpreis { get; set; }
    public int Anzahl { get; set; }
    public decimal Faktor { get; set; }
    public decimal Gesamtpreis { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
}

/// <summary>
/// Zahlungseingang für Rechnungen
/// </summary>
public class Zahlungseingang : BaseEntity
{
    public Guid RechnungId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid ZahlungsartId { get; set; }
    public virtual Zahlungsart Zahlungsart { get; set; } = null!;
    
    // Zahlungsdaten
    public decimal Betrag { get; set; }
    public DateTime Eingangsdatum { get; set; }
    public string Referenz { get; set; } = string.Empty; // Überweisungsreferenz
    public string Notizen { get; set; } = string.Empty;
    public bool IstTeilzahlung { get; set; }
    
    // Navigation Properties
    public virtual Rechnung Rechnung { get; set; } = null!;
}

/// <summary>
/// Zahlungseingang-Historisierung
/// </summary>
public class ZahlungseingangHistory : BaseHistoryEntity
{
    public Guid RechnungId { get; set; }
    public Guid ZahlungsartId { get; set; }
    public Zahlungsart Zahlungsart { get; set; } = null!;
    public decimal Betrag { get; set; }
    public DateTime Eingangsdatum { get; set; }
    public string Referenz { get; set; } = string.Empty;
    public string Notizen { get; set; } = string.Empty;
    public bool IstTeilzahlung { get; set; }
}

/// <summary>
/// Budgetplanung für verschiedene Kategorien
/// </summary>
public class Budget : BaseEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid BudgetkategorieId { get; set; }
    public virtual Budgetkategorie Budgetkategorie { get; set; } = null!;
    
    // Budgetdaten
    public int Jahr { get; set; }
    public int? Monat { get; set; } // null = Jahresbudget
    public decimal GeplanteBetrag { get; set; }
    public decimal TatsächlicherBetrag { get; set; }
    public decimal Abweichung => TatsächlicherBetrag - GeplanteBetrag;
    public decimal AbweichungProzent => GeplanteBetrag != 0 ? (Abweichung / GeplanteBetrag) * 100 : 0;
    public string Notizen { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual ICollection<BudgetPosition> Positionen { get; set; } = new List<BudgetPosition>();
}

/// <summary>
/// Budget-Historisierung
/// </summary>
public class BudgetHistory : BaseHistoryEntity
{
    public Guid BudgetkategorieId { get; set; }
    public Budgetkategorie Budgetkategorie { get; set; } = null!;
    public int Jahr { get; set; }
    public int? Monat { get; set; }
    public decimal GeplanteBetrag { get; set; }
    public decimal TatsächlicherBetrag { get; set; }
    public string Notizen { get; set; } = string.Empty;
}

/// <summary>
/// Detaillierte Budgetpositionen
/// </summary>
public class BudgetPosition : BaseEntity
{
    public Guid BudgetId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid KostentypId { get; set; }
    public virtual Kostentyp Kostentyp { get; set; } = null!;
    
    // Positionsdaten
    public string Bezeichnung { get; set; } = string.Empty;
    public decimal GeplanteBetrag { get; set; }
    public decimal TatsächlicherBetrag { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    public bool IstWiederkehrend { get; set; }
    public int? WiederholungsIntervallMonate { get; set; }
    
    // Navigation Properties
    public virtual Budget Budget { get; set; } = null!;
}

/// <summary>
/// BudgetPosition-Historisierung
/// </summary>
public class BudgetPositionHistory : BaseHistoryEntity
{
    public Guid BudgetId { get; set; }
    public Guid KostentypId { get; set; }
    public Kostentyp Kostentyp { get; set; } = null!;
    public string Bezeichnung { get; set; } = string.Empty;
    public decimal GeplanteBetrag { get; set; }
    public decimal TatsächlicherBetrag { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    public bool IstWiederkehrend { get; set; }
    public int? WiederholungsIntervallMonate { get; set; }
}

/// <summary>
/// Kostenstellenrechnung
/// </summary>
public class Kostenstelle : BaseEntity
{
    public string Code { get; set; } = string.Empty; // OST, PHY, TRA, ADMIN
    public string Bezeichnung { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public bool Aktiv { get; set; } = true;
    
    // Verantwortlichkeit
    public Guid? VerantwortlicherMitarbeiterId { get; set; }
    public virtual Mitarbeiter? VerantwortlicherMitarbeiter { get; set; }
    
    // Navigation Properties
    public virtual ICollection<KostenstellenBuchung> Buchungen { get; set; } = new List<KostenstellenBuchung>();
}

/// <summary>
/// Kostenstelle-Historisierung
/// </summary>
public class KostenstelleHistory : BaseHistoryEntity
{
    public string Code { get; set; } = string.Empty;
    public string Bezeichnung { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public bool Aktiv { get; set; }
    public Guid? VerantwortlicherMitarbeiterId { get; set; }
}

/// <summary>
/// Buchungen auf Kostenstellen
/// </summary>
public class KostenstellenBuchung : BaseEntity
{
    public Guid KostenstelleId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid BuchungsartId { get; set; }
    public virtual Buchungsart Buchungsart { get; set; } = null!;
    
    // Buchungsdaten
    public DateTime Buchungsdatum { get; set; }
    public decimal Betrag { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    public string Belegnummer { get; set; } = string.Empty;
    public string Referenz { get; set; } = string.Empty; // Verweis auf Rechnung, Gehalt, etc.
    
    // Navigation Properties
    public virtual Kostenstelle Kostenstelle { get; set; } = null!;
}

/// <summary>
/// KostenstellenBuchung-Historisierung
/// </summary>
public class KostenstellenBuchungHistory : BaseHistoryEntity
{
    public Guid KostenstelleId { get; set; }
    public Guid BuchungsartId { get; set; }
    public Buchungsart Buchungsart { get; set; } = null!;
    public DateTime Buchungsdatum { get; set; }
    public decimal Betrag { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    public string Belegnummer { get; set; } = string.Empty;
    public string Referenz { get; set; } = string.Empty;
}

/// <summary>
/// Steuerliche Informationen für Rechnungen
/// </summary>
public class Steuerdaten : BaseEntity
{
    public Guid RechnungId { get; set; }
    
    // Steuerdaten
    public decimal Nettobetrag { get; set; }
    public decimal Steuersatz { get; set; } = 19.0m; // Standard-MwSt
    public decimal Steuerbetrag { get; set; }
    public decimal Bruttobetrag { get; set; }
    public bool IstSteuerbefreit { get; set; }
    public string Steuerbefreiungsgrund { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual Rechnung Rechnung { get; set; } = null!;
}

/// <summary>
/// Steuerdaten-Historisierung
/// </summary>
public class SteuerdatenHistory : BaseHistoryEntity
{
    public Guid RechnungId { get; set; }
    public decimal Nettobetrag { get; set; }
    public decimal Steuersatz { get; set; }
    public decimal Steuerbetrag { get; set; }
    public decimal Bruttobetrag { get; set; }
    public bool IstSteuerbefreit { get; set; }
    public string Steuerbefreiungsgrund { get; set; } = string.Empty;
} 