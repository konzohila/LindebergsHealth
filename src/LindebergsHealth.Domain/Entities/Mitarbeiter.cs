namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Mitarbeiter-Hauptentität
/// </summary>
public class Mitarbeiter : BaseEntity
{
    public string Vorname { get; set; } = string.Empty;
    public string Nachname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual MitarbeiterDetails? Details { get; set; }
    public virtual MitarbeiterNotfallkontakt? Notfallkontakt { get; set; }
    public virtual MitarbeiterVertrag? Vertrag { get; set; }
    public virtual ICollection<MitarbeiterFortbildung> Fortbildungen { get; set; } = new List<MitarbeiterFortbildung>();
    public virtual ICollection<Termin> Termine { get; set; } = new List<Termin>();
    public virtual ICollection<TherapeutenCheckliste> Checklisten { get; set; } = new List<TherapeutenCheckliste>();
    public virtual ICollection<Gehalt> Gehaelter { get; set; } = new List<Gehalt>();
    
    // Normalisierte Adress- und Kontaktdaten
    public virtual ICollection<MitarbeiterAdresse> Adressen { get; set; } = new List<MitarbeiterAdresse>();
    public virtual ICollection<MitarbeiterKontakt> Kontakte { get; set; } = new List<MitarbeiterKontakt>();
}

/// <summary>
/// Mitarbeiter-Details
/// </summary>
public class MitarbeiterDetails : BaseEntity
{
    public Guid MitarbeiterId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid GeschlechtId { get; set; }
    public virtual Geschlecht Geschlecht { get; set; } = null!;
    
    public Guid FamilienstandId { get; set; }
    public virtual Familienstand Familienstand { get; set; } = null!;
    
    // Persönliche Daten
    public DateTime Geburtsdatum { get; set; }
    public string Sozialversicherungsnummer { get; set; } = string.Empty;
    public string SteuerId { get; set; } = string.Empty;
    public int Kinder { get; set; }

    // Navigation Properties
    public virtual Mitarbeiter Mitarbeiter { get; set; } = null!;
}

/// <summary>
/// MitarbeiterDetails-Historisierung
/// </summary>
public class MitarbeiterDetailsHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid GeschlechtId { get; set; }
    public Geschlecht Geschlecht { get; set; } = null!;
    
    public Guid FamilienstandId { get; set; }
    public Familienstand Familienstand { get; set; } = null!;
    
    // Persönliche Daten
    public DateTime Geburtsdatum { get; set; }
    public string Sozialversicherungsnummer { get; set; } = string.Empty;
    public string SteuerId { get; set; } = string.Empty;
    public int Kinder { get; set; }
}

/// <summary>
/// Mitarbeiter-Notfallkontakt
/// </summary>
public class MitarbeiterNotfallkontakt : BaseEntity
{
    public Guid MitarbeiterId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid BeziehungstypId { get; set; }
    public virtual Beziehungstyp Beziehungstyp { get; set; } = null!;
    
    // Kontaktdaten
    public string Name { get; set; } = string.Empty;
    public string Telefonnummer { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Navigation Properties
    public virtual Mitarbeiter Mitarbeiter { get; set; } = null!;
}

/// <summary>
/// MitarbeiterNotfallkontakt-Historisierung
/// </summary>
public class MitarbeiterNotfallkontaktHistory : BaseHistoryEntity
{
    // Foreign Key für Lookup-Tabelle
    public Guid BeziehungstypId { get; set; }
    public Beziehungstyp Beziehungstyp { get; set; } = null!;
    
    // Kontaktdaten
    public string Name { get; set; } = string.Empty;
    public string Telefonnummer { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
} 