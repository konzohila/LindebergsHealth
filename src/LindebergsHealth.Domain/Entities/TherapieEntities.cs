namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// TherapieEinheit-Entity für einzelne Therapiesitzungen
/// </summary>
public class TherapieEinheit : BaseEntity
{
    public Guid TerminId { get; set; }
    public Guid PatientId { get; set; }
    public Guid TherapeutId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid TherapietypId { get; set; }
    public Therapietyp Therapietyp { get; set; } = null!;
    
    public Guid TherapiestatusId { get; set; }
    public Therapiestatus Therapiestatus { get; set; } = null!;
    
    // Therapiedaten
    public string Befund { get; set; } = string.Empty;
    public string Behandlung { get; set; } = string.Empty;
    public string Notizen { get; set; } = string.Empty;
    public int Dauer { get; set; } // in Minuten
    
    // Navigation Properties
    public Termin Termin { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public Mitarbeiter Therapeut { get; set; } = null!;
}

/// <summary>
/// TherapieEinheit-Historisierung
/// </summary>
public class TherapieEinheitHistory : BaseHistoryEntity
{
    public Guid TerminId { get; set; }
    public Guid PatientId { get; set; }
    public Guid TherapeutId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid TherapietypId { get; set; }
    public Therapietyp Therapietyp { get; set; } = null!;
    
    public Guid TherapiestatusId { get; set; }
    public Therapiestatus Therapiestatus { get; set; } = null!;
    
    // Therapiedaten
    public string Befund { get; set; } = string.Empty;
    public string Behandlung { get; set; } = string.Empty;
    public string Notizen { get; set; } = string.Empty;
    public int Dauer { get; set; }
}

/// <summary>
/// Körperstatus-Entity für Befunde
/// </summary>
public class Koerperstatus : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid TherapeutId { get; set; } // Therapeut der den Befund erstellt hat
    
    // Foreign Key für Lookup-Tabelle
    public Guid KoerperregionId { get; set; }
    public Koerperregion Koerperregion { get; set; } = null!;
    public string Befund { get; set; } = string.Empty; // Beobachtungen / Testergebnisse
    public int SchmerzSkala { get; set; } // 0-10 Skala

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
    public Mitarbeiter Therapeut { get; set; } = null!;
}

/// <summary>
/// Koerperstatus-Historisierung
/// </summary>
public class KoerperstatusHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    public Guid TherapeutId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid KoerperregionId { get; set; }
    public Koerperregion Koerperregion { get; set; } = null!;
    public string Befund { get; set; } = string.Empty;
    public int SchmerzSkala { get; set; }
}

/// <summary>
/// TherapieSerie-Entity
/// </summary>
public class TherapieSerie : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid StartTerminId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid TherapieserieStatusId { get; set; }
    public TherapieserieStatus TherapieserieStatus { get; set; } = null!;
    
    // Therapiedaten
    public int AnzahlTermine { get; set; } // 5 oder 10
    public string Bezeichnung { get; set; } = string.Empty; // Manuelle Therapie

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
    public Termin StartTermin { get; set; } = null!;
}

/// <summary>
/// TherapieSerie-Historisierung
/// </summary>
public class TherapieSerieHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    public Guid StartTerminId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid TherapieserieStatusId { get; set; }
    public TherapieserieStatus TherapieserieStatus { get; set; } = null!;
    
    // Therapiedaten
    public int AnzahlTermine { get; set; }
    public string Bezeichnung { get; set; } = string.Empty;
}

/// <summary>
/// Therapeuten-Checkliste
/// </summary>
public class TherapeutenCheckliste : BaseEntity
{
    public Guid MitarbeiterId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid ChecklistenKategorieId { get; set; }
    public ChecklistenKategorie ChecklistenKategorie { get; set; } = null!;
    
    public Guid AufgabenStatusId { get; set; }
    public AufgabenStatus AufgabenStatus { get; set; } = null!;
    
    // Aufgabendaten
    public string Aufgabe { get; set; } = string.Empty; // Patientenakte prüfen
    public DateTime? FälligBis { get; set; }

    // Navigation Properties
    public Mitarbeiter Mitarbeiter { get; set; } = null!;
}

/// <summary>
/// TherapeutenCheckliste-Historisierung
/// </summary>
public class TherapeutenChecklisteHistory : BaseHistoryEntity
{
    public Guid MitarbeiterId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid ChecklistenKategorieId { get; set; }
    public ChecklistenKategorie ChecklistenKategorie { get; set; } = null!;
    
    public Guid AufgabenStatusId { get; set; }
    public AufgabenStatus AufgabenStatus { get; set; } = null!;
    
    // Aufgabendaten
    public string Aufgabe { get; set; } = string.Empty;
    public DateTime? FälligBis { get; set; }
}

/// <summary>
/// Einwilligung-Entity für DSGVO
/// </summary>
public class Einwilligung : BaseEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid EinwilligungsKategorieId { get; set; }
    public EinwilligungsKategorie EinwilligungsKategorie { get; set; } = null!;
    public string Text { get; set; } = string.Empty; // Einwilligungstext
    public bool Zustimmung { get; set; }
    public DateTime Datum { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// Einwilligung-Historisierung
/// </summary>
public class EinwilligungHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid EinwilligungsKategorieId { get; set; }
    public EinwilligungsKategorie EinwilligungsKategorie { get; set; } = null!;
    public string Text { get; set; } = string.Empty;
    public bool Zustimmung { get; set; }
    public DateTime Datum { get; set; }
}

/// <summary>
/// Kommunikationsverlauf-Entity
/// </summary>
public class Kommunikationsverlauf : BaseEntity
{
    public Guid PatientId { get; set; }
    public DateTime Datum { get; set; } = DateTime.UtcNow;
    
    // Foreign Key für Lookup-Tabelle
    public Guid KommunikationskanalId { get; set; }
    public Kommunikationskanal Kommunikationskanal { get; set; } = null!;
    public string Inhalt { get; set; } = string.Empty; // Gesprächs- oder Nachrichtendetails
    public Guid VerfasstVon { get; set; } // User / Mitarbeiter

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// Kommunikationsverlauf-Historisierung
/// </summary>
public class KommunikationsverlaufHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    public DateTime Datum { get; set; }
    
    // Foreign Key für Lookup-Tabelle
    public Guid KommunikationskanalId { get; set; }
    public Kommunikationskanal Kommunikationskanal { get; set; } = null!;
    public string Inhalt { get; set; } = string.Empty;
    public Guid VerfasstVonOriginal { get; set; }
} 