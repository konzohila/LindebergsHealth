namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Termin-Entity
/// </summary>
public class Termin : BaseEntity
{
    public bool IsDeleted { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public string Notizen { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
    public int DauerMinuten { get; set; }
    public Guid MitarbeiterId { get; set; }
    public Guid? PatientId { get; set; } // Optional für interne Termine
    public Guid RaumId { get; set; }
    public Guid KategorieId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid TermintypId { get; set; }
    public Termintyp Termintyp { get; set; } = null!;
    
    public Guid TerminstatusId { get; set; }
    public Terminstatus Terminstatus { get; set; } = null!;

    // Navigation Properties
    public virtual Patient? Patient { get; set; }
    public virtual Mitarbeiter Mitarbeiter { get; set; } = null!;
    public virtual Raum Raum { get; set; } = null!;
    
    // Erweiterte Termin-Navigation Properties
    public virtual Terminvorlage? Terminvorlage { get; set; }
    public virtual Terminserie? Terminserie { get; set; }
    public virtual ICollection<Terminänderung> Änderungen { get; set; } = new List<Terminänderung>();
    public virtual ICollection<Warteliste> WartelistenEinträge { get; set; } = new List<Warteliste>();
    public virtual ICollection<Rechnung> Rechnungen { get; set; } = new List<Rechnung>();
}

/// <summary>
/// Termin-Historisierung
/// </summary>
public class TerminHistory : BaseHistoryEntity
{
    public DateTime Datum { get; set; }
    public int DauerMinuten { get; set; }
    public Guid MitarbeiterId { get; set; }
    public Guid? PatientId { get; set; }
    public Guid RaumId { get; set; }
    public Guid KategorieId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid TermintypId { get; set; }
    public Termintyp Termintyp { get; set; } = null!;
    
    public Guid TerminstatusId { get; set; }
    public Terminstatus Terminstatus { get; set; } = null!;
}

/// <summary>
/// Terminkategorie
/// </summary>
public class TerminKategorie : BaseEntity
{
    public string Bereich { get; set; } = string.Empty; // OST, PHY, TRA
    public string Bereichsbezeichnung { get; set; } = string.Empty; // z.B. Rosa
    public string Farbe { get; set; } = string.Empty; // z.B. Gelb
    public string Farbcode { get; set; } = string.Empty; // HEX-Wert
    public string Code { get; set; } = string.Empty; // z.B. NEU-OST-30
    public string Kundentyp { get; set; } = string.Empty; // NEU, RA
    public string Versicherung { get; set; } = string.Empty; // PKV, GKV
    public int DauerMinuten { get; set; }
    public string Häufigkeit { get; set; } = string.Empty;
    public string Kommentar { get; set; } = string.Empty;

    // Navigation Properties
    public ICollection<Termin> Termine { get; set; } = new List<Termin>();
}

/// <summary>
/// TerminKategorie-Historisierung
/// </summary>
public class TerminKategorieHistory : BaseHistoryEntity
{
    public string Bereich { get; set; } = string.Empty;
    public string Bereichsbezeichnung { get; set; } = string.Empty;
    public string Farbe { get; set; } = string.Empty;
    public string Farbcode { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Kundentyp { get; set; } = string.Empty;
    public string Versicherung { get; set; } = string.Empty;
    public int DauerMinuten { get; set; }
    public string Häufigkeit { get; set; } = string.Empty;
    public string Kommentar { get; set; } = string.Empty;
} 