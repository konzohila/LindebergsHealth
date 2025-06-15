namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Praxis-Entity für Praxisinformationen
/// </summary>
public class Praxis : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;

    // Navigation Properties
    public ICollection<PraxisAdresse> Adressen { get; set; } = new List<PraxisAdresse>();
    public ICollection<Raum> Räume { get; set; } = new List<Raum>();
    public ICollection<Mitarbeiter> Mitarbeiter { get; set; } = new List<Mitarbeiter>();
}

/// <summary>
/// Praxis-Historisierung
/// </summary>
public class PraxisHistory : BaseHistoryEntity
{
    public string Name { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public string Telefon { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
}

/// <summary>
/// Ausstattung-Entity für Praxisausstattung
/// </summary>
public class Ausstattung : BaseEntity
{
    public Guid RaumId { get; set; }

    // Foreign Keys für Lookup-Tabellen
    public Guid AusstattungstypId { get; set; }
    public Ausstattungstyp Ausstattungstyp { get; set; } = null!;

    // Ausstattungsdaten
    public string Bezeichnung { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public DateTime? AnschaffungsDatum { get; set; }
    public decimal? AnschaffungsWert { get; set; }
    public bool Aktiv { get; set; } = true;

    // Navigation Properties
    public Raum Raum { get; set; } = null!;
}

/// <summary>
/// Ausstattung-Historisierung
/// </summary>
public class AusstattungHistory : BaseHistoryEntity
{
    public Guid RaumId { get; set; }

    // Foreign Keys für Lookup-Tabellen
    public Guid AusstattungstypId { get; set; }
    public Ausstattungstyp Ausstattungstyp { get; set; } = null!;

    // Ausstattungsdaten
    public string Bezeichnung { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public DateTime? AnschaffungsDatum { get; set; }
    public decimal? AnschaffungsWert { get; set; }
    public bool Aktiv { get; set; }
}

/// <summary>
/// Raum-Entity
/// </summary>
public class Raum : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty; // Raum 1
    public int Etage { get; set; }
    public string Ausstattung { get; set; } = string.Empty; // Funktionale Beschreibung
    public bool Aktiv { get; set; } = true;

    // Navigation Properties
    public ICollection<Termin> Termine { get; set; } = new List<Termin>();
}

/// <summary>
/// Raum-Historisierung
/// </summary>
public class RaumHistory : BaseHistoryEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public int Etage { get; set; }
    public string Ausstattung { get; set; } = string.Empty;
    public bool Aktiv { get; set; }
}

/// <summary>
/// Dokument-Entity
/// </summary>
public class Dokument : BaseEntity
{
    public Guid PatientId { get; set; }

    // Foreign Keys für Lookup-Tabellen
    public Guid DateitypId { get; set; }
    public Dateityp Dateityp { get; set; } = null!;

    public Guid DokumenttypId { get; set; }
    public Dokumenttyp Dokumenttyp { get; set; } = null!;

    // Dokumentdaten
    public string Titel { get; set; } = string.Empty; // MRT Bericht
    public string Dateipfad { get; set; } = string.Empty;
    public bool SichtbarFürPatient { get; set; }
    public Guid HochgeladenVon { get; set; }
    public DateTime HochgeladenAm { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Patient Patient { get; set; } = null!;
}

/// <summary>
/// Dokument-Historisierung
/// </summary>
public class DokumentHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }

    // Foreign Keys für Lookup-Tabellen
    public Guid DateitypId { get; set; }
    public Dateityp Dateityp { get; set; } = null!;

    public Guid DokumenttypId { get; set; }
    public Dokumenttyp Dokumenttyp { get; set; } = null!;

    // Dokumentdaten
    public string Titel { get; set; } = string.Empty;
    public string Dateipfad { get; set; } = string.Empty;
    public bool SichtbarFürPatient { get; set; }
    public Guid HochgeladenVon { get; set; }
    public DateTime HochgeladenAm { get; set; }
}

/// <summary>
/// Kooperationspartner-Entity
/// </summary>
public class Kooperationspartner : BaseEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid FachrichtungId { get; set; }
    public Fachrichtung Fachrichtung { get; set; } = null!;

    public Guid KommunikationsformId { get; set; }
    public Kommunikationsform Kommunikationsform { get; set; } = null!;

    // Partnerdaten
    public string Name { get; set; } = string.Empty; // Arztpraxis
    public string Einrichtung { get; set; } = string.Empty; // Träger
    public string Ansprechpartner { get; set; } = string.Empty;
    public string QRCodeURL { get; set; } = string.Empty;

    // Navigation Properties
    public KooperationDetails? Details { get; set; }

    // Normalisierte Adress- und Kontaktdaten
    public ICollection<KooperationspartnerAdresse> Adressen { get; set; } = new List<KooperationspartnerAdresse>();
    public ICollection<KooperationspartnerKontakt> Kontakte { get; set; } = new List<KooperationspartnerKontakt>();
}

/// <summary>
/// Kooperationspartner-Historisierung
/// </summary>
public class KooperationspartnerHistory : BaseHistoryEntity
{
    // Foreign Keys für Lookup-Tabellen
    public Guid FachrichtungId { get; set; }
    public Fachrichtung Fachrichtung { get; set; } = null!;

    public Guid KommunikationsformId { get; set; }
    public Kommunikationsform Kommunikationsform { get; set; } = null!;

    // Partnerdaten
    public string Name { get; set; } = string.Empty;
    public string Einrichtung { get; set; } = string.Empty;
    public string Ansprechpartner { get; set; } = string.Empty;
    public string QRCodeURL { get; set; } = string.Empty;
}

/// <summary>
/// Kooperations-Details
/// </summary>
public class KooperationDetails : BaseEntity
{
    public Guid PartnerId { get; set; }

    // Foreign Key für Lookup-Tabelle
    public Guid KooperationsStatusId { get; set; }
    public KooperationsStatus KooperationsStatus { get; set; } = null!;
    public bool PatientenAktiv { get; set; }
    public string Rückempfehlungshäufigkeit { get; set; } = string.Empty; // wöchentlich
    public bool RückmeldungErwünscht { get; set; }
    public string TeilnahmeAngebote { get; set; } = string.Empty; // Retreat, Weiterbildung
    public bool DSGVOZustimmung { get; set; }
    public string CRMBeziehungspflege { get; set; } = string.Empty;

    // Navigation Properties
    public Kooperationspartner Partner { get; set; } = null!;
}

/// <summary>
/// KooperationDetails-Historisierung
/// </summary>
public class KooperationDetailsHistory : BaseHistoryEntity
{
    // Foreign Key für Lookup-Tabelle
    public Guid KooperationsStatusId { get; set; }
    public KooperationsStatus KooperationsStatus { get; set; } = null!;
    public bool PatientenAktiv { get; set; }
    public string Rückempfehlungshäufigkeit { get; set; } = string.Empty;
    public bool RückmeldungErwünscht { get; set; }
    public string TeilnahmeAngebote { get; set; } = string.Empty;
    public bool DSGVOZustimmung { get; set; }
    public string CRMBeziehungspflege { get; set; } = string.Empty;
}

/// <summary>
/// PraxisAdresse-Entity für Praxisadressen
/// </summary>
public class PraxisAdresse : BaseEntity
{
    public Guid PraxisId { get; set; }
    public Guid AdresseId { get; set; }

    // Foreign Keys für Lookup-Tabellen
    public Guid AdresstypId { get; set; }
    public Adresstyp Adresstyp { get; set; } = null!;

    public bool IstHauptadresse { get; set; }

    // Navigation Properties
    public Praxis Praxis { get; set; } = null!;
    public Adresse Adresse { get; set; } = null!;
}

/// <summary>
/// PraxisAdresse-Historisierung
/// </summary>
public class PraxisAdresseHistory : BaseHistoryEntity
{
    public Guid PraxisId { get; set; }
    public Guid AdresseId { get; set; }

    // Foreign Keys für Lookup-Tabellen
    public Guid AdresstypId { get; set; }
    public Adresstyp Adresstyp { get; set; } = null!;

    public bool IstHauptadresse { get; set; }
}
