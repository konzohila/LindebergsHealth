namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Erweiterte Patientendaten
/// </summary>
public class PatientErweiterung : BaseEntity
{
    public string Notizen { get; set; } = string.Empty;
    public Guid PatientId { get; set; }

    // Foreign Keys für Lookup-Tabellen
    public Guid StaatsangehoerigkeitId { get; set; }
    public Staatsangehoerigkeit Staatsangehoerigkeit { get; set; } = null!;

    public Guid FamilienstandId { get; set; }
    public Familienstand Familienstand { get; set; } = null!;

    // Adressdaten (hier könnten später strukturierte Adressen verwendet werden)
    public string Adresse { get; set; } = string.Empty;
    public string Rechnungsadresse { get; set; } = string.Empty;

    // Kontaktdaten
    public string TelefonMobil { get; set; } = string.Empty;
    public string TelefonArbeit { get; set; } = string.Empty;
    public string EmailPrivat { get; set; } = string.Empty;

    // Berufsdaten
    public string Beruf { get; set; } = string.Empty;
    public string Arbeitgeber { get; set; } = string.Empty;
    public int AnzahlKinder { get; set; }
    public string Sprachkenntnisse { get; set; } = string.Empty; // CSV oder JSON

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// PatientErweiterung-Historisierung
/// </summary>
public class PatientErweiterungHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid StaatsangehoerigkeitId { get; set; }
    public Staatsangehoerigkeit Staatsangehoerigkeit { get; set; } = null!;

    public Guid FamilienstandId { get; set; }
    public Familienstand Familienstand { get; set; } = null!;

    // Adressdaten
    public string Adresse { get; set; } = string.Empty;
    public string Rechnungsadresse { get; set; } = string.Empty;

    // Kontaktdaten
    public string TelefonMobil { get; set; } = string.Empty;
    public string TelefonArbeit { get; set; } = string.Empty;
    public string EmailPrivat { get; set; } = string.Empty;

    // Berufsdaten
    public string Beruf { get; set; } = string.Empty;
    public string Arbeitgeber { get; set; } = string.Empty;

    public int AnzahlKinder { get; set; }
    public string Sprachkenntnisse { get; set; } = string.Empty;
}
