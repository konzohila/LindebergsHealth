namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Terminvorlage für wiederkehrende Termine
/// </summary>
public class Terminvorlage : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    
    // Foreign Keys für Lookup-Tabellen
    public Guid TermintypId { get; set; }
    public virtual Termintyp Termintyp { get; set; } = null!;
    
    // Vorlagendetails
    public int StandardDauerMinuten { get; set; }
    public decimal StandardPreis { get; set; }
    public string Farbe { get; set; } = string.Empty; // Hex-Farbcode
    public bool IstAktiv { get; set; } = true;
    
    // Wiederholung
    public bool IstWiederholend { get; set; }
    public int? WiederholungsIntervallTage { get; set; }
    public int? MaximalAnzahlWiederholungen { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Termin> Termine { get; set; } = new List<Termin>();
}

/// <summary>
/// Terminvorlage-Historisierung
/// </summary>
public class TerminvorlageHistory : BaseHistoryEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public Guid TermintypId { get; set; }
    public Termintyp Termintyp { get; set; } = null!;
    public int StandardDauerMinuten { get; set; }
    public decimal StandardPreis { get; set; }
    public string Farbe { get; set; } = string.Empty;
    public bool IstAktiv { get; set; }
    public bool IstWiederholend { get; set; }
    public int? WiederholungsIntervallTage { get; set; }
    public int? MaximalAnzahlWiederholungen { get; set; }
}

/// <summary>
/// Terminblockierung für Urlaub, Fortbildung, etc.
/// </summary>
public class Terminblockierung : BaseEntity
{
    public Guid? MitarbeiterId { get; set; } // null = alle Mitarbeiter
    public Guid? RaumId { get; set; } // null = alle Räume
    
    // Foreign Keys für Lookup-Tabellen
    public Guid BlockierungsgrundId { get; set; }
    public virtual Blockierungsgrund Blockierungsgrund { get; set; } = null!;
    
    // Blockierungsdetails
    public DateTime StartDatum { get; set; }
    public DateTime EndDatum { get; set; }
    public bool GanzerTag { get; set; } = true;
    public TimeOnly? StartZeit { get; set; }
    public TimeOnly? EndZeit { get; set; }
    
    public string Titel { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public bool IstWiederholend { get; set; }
    public string WiederholungsMuster { get; set; } = string.Empty; // JSON für komplexe Muster
    
    // Navigation Properties
    public virtual Mitarbeiter? Mitarbeiter { get; set; }
    public virtual Raum? Raum { get; set; }
}

/// <summary>
/// Terminblockierung-Historisierung
/// </summary>
public class TerminblockierungHistory : BaseHistoryEntity
{
    public Guid? MitarbeiterId { get; set; }
    public Guid? RaumId { get; set; }
    public Guid BlockierungsgrundId { get; set; }
    public Blockierungsgrund Blockierungsgrund { get; set; } = null!;
    public DateTime StartDatum { get; set; }
    public DateTime EndDatum { get; set; }
    public bool GanzerTag { get; set; }
    public TimeOnly? StartZeit { get; set; }
    public TimeOnly? EndZeit { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public bool IstWiederholend { get; set; }
    public string WiederholungsMuster { get; set; } = string.Empty;
}

/// <summary>
/// Terminänderungen und Stornierungen
/// </summary>
public class Terminänderung : BaseEntity
{
    public Guid TerminId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid ÄnderungsgrundId { get; set; }
    public virtual Änderungsgrund Änderungsgrund { get; set; } = null!;
    
    // Änderungsdetails
    public DateTime ÄnderungsDatum { get; set; } = DateTime.UtcNow;
    public Guid ÄnderungVon { get; set; } // MitarbeiterId
    public string Beschreibung { get; set; } = string.Empty;
    
    // Alte Werte (JSON)
    public string AlteWerte { get; set; } = string.Empty;
    public string NeueWerte { get; set; } = string.Empty;
    
    // Kosten
    public decimal? Stornogebühr { get; set; }
    public decimal? Umbuchungsgebühr { get; set; }
    
    // Navigation Properties
    public virtual Termin Termin { get; set; } = null!;
}

/// <summary>
/// Terminänderung-Historisierung
/// </summary>
public class TerminänderungHistory : BaseHistoryEntity
{
    public Guid TerminId { get; set; }
    public Guid ÄnderungsgrundId { get; set; }
    public Änderungsgrund Änderungsgrund { get; set; } = null!;
    public DateTime ÄnderungsDatum { get; set; }
    public Guid ÄnderungVon { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    public string AlteWerte { get; set; } = string.Empty;
    public string NeueWerte { get; set; } = string.Empty;
    public decimal? Stornogebühr { get; set; }
    public decimal? Umbuchungsgebühr { get; set; }
}

/// <summary>
/// Warteliste für ausgebuchte Termine
/// </summary>
public class Warteliste : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid? MitarbeiterId { get; set; } // Wunsch-Therapeut
    
    // Foreign Keys für Lookup-Tabellen
    public Guid TermintypId { get; set; }
    public virtual Termintyp Termintyp { get; set; } = null!;
    
    public Guid PrioritätId { get; set; }
    public virtual Priorität Priorität { get; set; } = null!;
    
    // Wartelistendetails
    public DateTime EingetragenenAm { get; set; } = DateTime.UtcNow;
    public DateTime? WunschDatumVon { get; set; }
    public DateTime? WunschDatumBis { get; set; }
    public TimeOnly? WunschZeitVon { get; set; }
    public TimeOnly? WunschZeitBis { get; set; }
    
    public string Notizen { get; set; } = string.Empty;
    public bool IstAktiv { get; set; } = true;
    public DateTime? TerminGefundenAm { get; set; }
    public Guid? ZugewiesenerTerminId { get; set; }
    
    // Navigation Properties
    public virtual Patient Patient { get; set; } = null!;
    public virtual Mitarbeiter? Mitarbeiter { get; set; }
    public virtual Termin? ZugewiesenerTermin { get; set; }
}

/// <summary>
/// Warteliste-Historisierung
/// </summary>
public class WartelisteHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    public Guid? MitarbeiterId { get; set; }
    public Guid TermintypId { get; set; }
    public Termintyp Termintyp { get; set; } = null!;
    public Guid PrioritätId { get; set; }
    public Priorität Priorität { get; set; } = null!;
    public DateTime EingetragenenAm { get; set; }
    public DateTime? WunschDatumVon { get; set; }
    public DateTime? WunschDatumBis { get; set; }
    public TimeOnly? WunschZeitVon { get; set; }
    public TimeOnly? WunschZeitBis { get; set; }
    public string Notizen { get; set; } = string.Empty;
    public bool IstAktiv { get; set; }
    public DateTime? TerminGefundenAm { get; set; }
    public Guid? ZugewiesenerTerminId { get; set; }
}

/// <summary>
/// Terminserien für Behandlungszyklen
/// </summary>
public class Terminserie : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid MitarbeiterId { get; set; }
    
    // Foreign Keys für Lookup-Tabellen
    public Guid TermintypId { get; set; }
    public virtual Termintyp Termintyp { get; set; } = null!;
    
    // Seriendetails
    public string Bezeichnung { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public int GeplanteAnzahlTermine { get; set; }
    public int TatsächlicheAnzahlTermine { get; set; }
    public int DauerMinuten { get; set; }
    
    // Wiederholung
    public int IntervallTage { get; set; } = 7; // Standard: wöchentlich
    public DayOfWeek? WunschWochentag { get; set; }
    public TimeOnly? WunschUhrzeit { get; set; }
    
    // Status
    public DateTime StartDatum { get; set; }
    public DateTime? EndDatum { get; set; }
    public bool IstAbgeschlossen { get; set; }
    public string Notizen { get; set; } = string.Empty;
    
    // Navigation Properties
    public virtual Patient Patient { get; set; } = null!;
    public virtual Mitarbeiter Mitarbeiter { get; set; } = null!;
    public virtual ICollection<Termin> Termine { get; set; } = new List<Termin>();
}

/// <summary>
/// Terminserie-Historisierung
/// </summary>
public class TerminserieHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    public Guid MitarbeiterId { get; set; }
    public Guid TermintypId { get; set; }
    public Termintyp Termintyp { get; set; } = null!;
    public string Bezeichnung { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public int GeplanteAnzahlTermine { get; set; }
    public int TatsächlicheAnzahlTermine { get; set; }
    public int DauerMinuten { get; set; }
    public int IntervallTage { get; set; }
    public DayOfWeek? WunschWochentag { get; set; }
    public TimeOnly? WunschUhrzeit { get; set; }
    public DateTime StartDatum { get; set; }
    public DateTime? EndDatum { get; set; }
    public bool IstAbgeschlossen { get; set; }
    public string Notizen { get; set; } = string.Empty;
} 