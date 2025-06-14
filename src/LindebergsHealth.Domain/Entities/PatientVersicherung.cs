namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Patientenversicherung
/// </summary>
public class PatientVersicherung : BaseEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid VersicherungstypId { get; set; }
    public Versicherungstyp Versicherungstyp { get; set; } = null!;
    
    public Guid VersicherungsstatusId { get; set; }
    public Versicherungsstatus Versicherungsstatus { get; set; } = null!;
    
    // Versicherungsdaten
    public string NameDerVersicherung { get; set; } = string.Empty;
    public string VersicherterName { get; set; } = string.Empty; // Falls abweichend
    public DateTime VersicherterGeburtsdatum { get; set; }
    public bool HeilpraktikerErlaubt { get; set; }
    public string Mitversicherte { get; set; } = string.Empty; // Freitext

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// PatientVersicherung-Historisierung
/// </summary>
public class PatientVersicherungHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid VersicherungstypId { get; set; }
    public Versicherungstyp Versicherungstyp { get; set; } = null!;
    
    public Guid VersicherungsstatusId { get; set; }
    public Versicherungsstatus Versicherungsstatus { get; set; } = null!;
    
    // Versicherungsdaten
    public string NameDerVersicherung { get; set; } = string.Empty;
    public string VersicherterName { get; set; } = string.Empty;
    public DateTime VersicherterGeburtsdatum { get; set; }
    public bool HeilpraktikerErlaubt { get; set; }
    public string Mitversicherte { get; set; } = string.Empty;
} 