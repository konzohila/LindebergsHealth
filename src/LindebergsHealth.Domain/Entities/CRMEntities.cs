namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// CRM-Status für Patienten
/// </summary>
public class CRMStatus : BaseEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid CRMStatusTypId { get; set; }
    public CRMStatusTyp CRMStatusTyp { get; set; } = null!;
    
    public Guid KundentypId { get; set; }
    public Kundentyp Kundentyp { get; set; } = null!;
    public string JahresplanRelevanz { get; set; } = string.Empty;
    public string PrivilegedClientStatus { get; set; } = string.Empty;
    public string Kundenbewertung { get; set; } = string.Empty;
    public string RecallIntervall { get; set; } = string.Empty; // Quartalsweise
    public bool JokerRegelungAktiv { get; set; }
    public DateTime Geburtstag { get; set; }
    public string Haupttherapeut { get; set; } = string.Empty;
    public string Mittherapeuten { get; set; } = string.Empty; // CSV-Liste
    public string CRMKommentar { get; set; } = string.Empty;

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// CRMStatus-Historisierung
/// </summary>
public class CRMStatusHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid CRMStatusTypId { get; set; }
    public CRMStatusTyp CRMStatusTyp { get; set; } = null!;
    
    public Guid KundentypId { get; set; }
    public Kundentyp Kundentyp { get; set; } = null!;
    public string JahresplanRelevanz { get; set; } = string.Empty;
    public string PrivilegedClientStatus { get; set; } = string.Empty;
    public string Kundenbewertung { get; set; } = string.Empty;
    public string RecallIntervall { get; set; } = string.Empty;
    public bool JokerRegelungAktiv { get; set; }
    public DateTime Geburtstag { get; set; }
    public string Haupttherapeut { get; set; } = string.Empty;
    public string Mittherapeuten { get; set; } = string.Empty;
    public string CRMKommentar { get; set; } = string.Empty;
}

/// <summary>
/// CRM-Netzwerk für Patientenempfehlungen
/// </summary>
public class CRMNetzwerk : BaseEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid EmpfehlungstypId { get; set; }
    public Empfehlungstyp Empfehlungstyp { get; set; } = null!;
    
    public Guid NetzwerktypId { get; set; }
    public Netzwerktyp Netzwerktyp { get; set; } = null!;
    
    // Netzwerkdaten
    public string EmpfehlendePerson { get; set; } = string.Empty;
    public string Rolle { get; set; } = string.Empty;
    public string SozialeMedien { get; set; } = string.Empty; // verknüpfte Kanäle
    public string Weiterempfehlung { get; set; } = string.Empty; // Retreat

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// CRMNetzwerk-Historisierung
/// </summary>
public class CRMNetzwerkHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid EmpfehlungstypId { get; set; }
    public Empfehlungstyp Empfehlungstyp { get; set; } = null!;
    
    public Guid NetzwerktypId { get; set; }
    public Netzwerktyp Netzwerktyp { get; set; } = null!;
    
    // Netzwerkdaten
    public string EmpfehlendePerson { get; set; } = string.Empty;
    public string Rolle { get; set; } = string.Empty;
    public string SozialeMedien { get; set; } = string.Empty;
    public string Weiterempfehlung { get; set; } = string.Empty;
}

/// <summary>
/// Patient-Kommunikationseinstellungen
/// </summary>
public class PatientKommunikation : BaseEntity
{
    public Guid PatientId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid KommunikationskanalId { get; set; }
    public Kommunikationskanal Kommunikationskanal { get; set; } = null!;
    
    public Guid SpracheId { get; set; }
    public Sprache Sprache { get; set; } = null!;
    
    // Kommunikationseinstellungen
    public bool KommunikationErlaubt { get; set; } // DSGVO-Zustimmung
    public string CheckupErinnerung { get; set; } = string.Empty; // Intervall
    public bool SocialMediaEinwilligung { get; set; }
    public bool BefundaustauschErlaubt { get; set; }

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// PatientKommunikation-Historisierung
/// </summary>
public class PatientKommunikationHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid KommunikationskanalId { get; set; }
    public Kommunikationskanal Kommunikationskanal { get; set; } = null!;
    
    public Guid SpracheId { get; set; }
    public Sprache Sprache { get; set; } = null!;
    
    // Kommunikationseinstellungen
    public bool KommunikationErlaubt { get; set; }
    public string CheckupErinnerung { get; set; } = string.Empty;
    public bool SocialMediaEinwilligung { get; set; }
    public bool BefundaustauschErlaubt { get; set; }
} 