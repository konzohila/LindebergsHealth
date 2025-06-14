namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Mitarbeiter-Vertrag
/// </summary>
public class MitarbeiterVertrag : BaseEntity
{
    public Guid MitarbeiterId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid MitarbeiterFunktionId { get; set; }
    public MitarbeiterFunktion MitarbeiterFunktion { get; set; } = null!;
    
    public Guid AbteilungId { get; set; }
    public Abteilung Abteilung { get; set; } = null!;
    
    public Guid VertragsformId { get; set; }
    public Vertragsform Vertragsform { get; set; } = null!;
    
    public Guid GehaltstypId { get; set; }
    public Gehaltstyp Gehaltstyp { get; set; } = null!;
    
    public Guid VertragsstatusId { get; set; }
    public Vertragsstatus Vertragsstatus { get; set; } = null!;
    
    // Vertragsdaten
    public int StundenWoche { get; set; } // Vertragliche Stunden
    public DateTime Eintrittsdatum { get; set; }
    public string Vorbeschäftigung { get; set; } = string.Empty; // Letzte Tätigkeit
    public int Berufsjahre { get; set; } // Berufserfahrung
    public string GehaltsHistorieLink { get; set; } = string.Empty; // Verknüpfung zur Historie
    public int Urlaubsanspruch { get; set; } // in Tagen
    public string Verantwortlichkeiten { get; set; } = string.Empty; // Mehrfachauswahl

    // Navigation Properties
    public Mitarbeiter Mitarbeiter { get; set; } = null!;
}

/// <summary>
/// MitarbeiterVertrag-Historisierung
/// </summary>
public class MitarbeiterVertragHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid MitarbeiterFunktionId { get; set; }
    public MitarbeiterFunktion MitarbeiterFunktion { get; set; } = null!;
    
    public Guid AbteilungId { get; set; }
    public Abteilung Abteilung { get; set; } = null!;
    
    public Guid VertragsformId { get; set; }
    public Vertragsform Vertragsform { get; set; } = null!;
    
    public Guid GehaltstypId { get; set; }
    public Gehaltstyp Gehaltstyp { get; set; } = null!;
    
    public Guid VertragsstatusId { get; set; }
    public Vertragsstatus Vertragsstatus { get; set; } = null!;
    
    // Vertragsdaten
    public int StundenWoche { get; set; }
    public DateTime Eintrittsdatum { get; set; }
    public string Vorbeschäftigung { get; set; } = string.Empty;
    public int Berufsjahre { get; set; }
    public string GehaltsHistorieLink { get; set; } = string.Empty;
    public int Urlaubsanspruch { get; set; }
    public string Verantwortlichkeiten { get; set; } = string.Empty;
}

/// <summary>
/// Mitarbeiter-Fortbildung
/// </summary>
public class MitarbeiterFortbildung : BaseEntity
{
    public Guid MitarbeiterId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid TeilnahmeformId { get; set; }
    public Teilnahmeform Teilnahmeform { get; set; } = null!;
    
    // Fortbildungsdaten
    public string Titel { get; set; } = string.Empty; // Name der Fortbildung
    public string Anbieter { get; set; } = string.Empty; // Durchführende Organisation
    public int DauerInTagen { get; set; } // z.B. 3
    public int Punkte { get; set; } // z.B. 24
    public bool DiplomErhalten { get; set; } // Zertifikat?
    public string DateiUpload { get; set; } = string.Empty; // Pfad zur Datei
    public bool ZuschussErhalten { get; set; } // Förderung?
    public decimal ZuschussBetrag { get; set; } // €-Betrag

    // Navigation Properties
    public Mitarbeiter Mitarbeiter { get; set; } = null!;
}

/// <summary>
/// MitarbeiterFortbildung-Historisierung
/// </summary>
public class MitarbeiterFortbildungHistory : BaseHistoryEntity
{
    public Guid MitarbeiterId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid TeilnahmeformId { get; set; }
    public Teilnahmeform Teilnahmeform { get; set; } = null!;
    
    // Fortbildungsdaten
    public string Titel { get; set; } = string.Empty;
    public string Anbieter { get; set; } = string.Empty;
    public int DauerInTagen { get; set; }
    public int Punkte { get; set; }
    public bool DiplomErhalten { get; set; }
    public string DateiUpload { get; set; } = string.Empty;
    public bool ZuschussErhalten { get; set; }
    public decimal ZuschussBetrag { get; set; }
}

/// <summary>
/// Patient-Beziehungsperson
/// </summary>
public class PatientBeziehungsperson : BaseEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid BeziehungstypId { get; set; }
    public Beziehungstyp Beziehungstyp { get; set; } = null!;
    
    // Personendaten
    public string Name { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool DarfTermineVereinbaren { get; set; }
    public bool DarfRechnungenKlaeren { get; set; }

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// PatientBeziehungsperson-Historisierung
/// </summary>
public class PatientBeziehungspersonHistory : BaseHistoryEntity
{
    // Foreign Key für Lookup-Tabelle
    public Guid BeziehungstypId { get; set; }
    public Beziehungstyp Beziehungstyp { get; set; } = null!;
    
    // Personendaten
    public string Name { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool DarfTermineVereinbaren { get; set; }
    public bool DarfRechnungenKlaeren { get; set; }
}

/// <summary>
/// Patient-Empfehlung
/// </summary>
public class PatientEmpfehlung : BaseEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid EmpfehlungstypId { get; set; }
    public Empfehlungstyp Empfehlungstyp { get; set; } = null!;
    public string Detail { get; set; } = string.Empty; // Details (Name, Plattform etc.)

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// PatientEmpfehlung-Historisierung
/// </summary>
public class PatientEmpfehlungHistory : BaseHistoryEntity
{
    // Foreign Key für Lookup-Tabelle
    public Guid EmpfehlungstypId { get; set; }
    public Empfehlungstyp Empfehlungstyp { get; set; } = null!;
    public string Detail { get; set; } = string.Empty;
}

/// <summary>
/// Patient-Notfallkontakt
/// </summary>
public class PatientNotfallkontakt : BaseEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid BeziehungstypId { get; set; }
    public Beziehungstyp Beziehungstyp { get; set; } = null!;
    
    // Kontaktdaten
    public string Name { get; set; } = string.Empty;
    public string Telefonnummer { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// PatientNotfallkontakt-Historisierung
/// </summary>
public class PatientNotfallkontaktHistory : BaseHistoryEntity
{
    // Foreign Key für Lookup-Tabelle
    public Guid BeziehungstypId { get; set; }
    public Beziehungstyp Beziehungstyp { get; set; } = null!;
    
    // Kontaktdaten
    public string Name { get; set; } = string.Empty;
    public string Telefonnummer { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
} 