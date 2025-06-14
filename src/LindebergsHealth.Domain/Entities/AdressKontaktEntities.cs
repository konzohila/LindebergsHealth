namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Normalisierte Adress-Entity
/// </summary>
public class Adresse : BaseEntity
{
    public bool IsDeleted { get; set; }
    public string Strasse { get; set; } = string.Empty;
    public string Hausnummer { get; set; } = string.Empty;
    public string Zusatz { get; set; } = string.Empty; // c/o, Stockwerk, etc.
    public string Postleitzahl { get; set; } = string.Empty;
    public string Ort { get; set; } = string.Empty;
    public string Land { get; set; } = "Deutschland";

    // Geo-Koordinaten für Routenplanung
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    // Validierung und Qualität
    public bool IstValidiert { get; set; }
    public DateTime? ValidiertAm { get; set; }
    public string ValidationSource { get; set; } = string.Empty; // Google, DHL, etc.

    // Navigation Properties
    public virtual ICollection<PatientAdresse> PatientenAdressen { get; set; } = new List<PatientAdresse>();
    public virtual ICollection<MitarbeiterAdresse> MitarbeiterAdressen { get; set; } = new List<MitarbeiterAdresse>();
    public virtual ICollection<KooperationspartnerAdresse> KooperationspartnerAdressen { get; set; } = new List<KooperationspartnerAdresse>();
}

/// <summary>
/// Adresse-Historisierung
/// </summary>
public class AdresseHistory : BaseHistoryEntity
{
    public string Strasse { get; set; } = string.Empty;
    public string Hausnummer { get; set; } = string.Empty;
    public string Zusatz { get; set; } = string.Empty;
    public string Postleitzahl { get; set; } = string.Empty;
    public string Ort { get; set; } = string.Empty;
    public string Land { get; set; } = "Deutschland";
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IstValidiert { get; set; }
    public DateTime? ValidiertAm { get; set; }
    public string ValidationSource { get; set; } = string.Empty;
}

/// <summary>
/// Normalisierte Kontakt-Entity
/// </summary>
public class Kontakt : BaseEntity
{
    public bool IsDeleted { get; set; }
    // Foreign Key für Kontakttyp-Lookup
    public Guid KontakttypId { get; set; }
    public virtual Kontakttyp Kontakttyp { get; set; } = null!;

    public string Wert { get; set; } = string.Empty; // Telefonnummer, E-Mail, etc.
    public bool IstHauptkontakt { get; set; }
    public bool IstValidiert { get; set; }
    public DateTime? ValidiertAm { get; set; }
    public string Notizen { get; set; } = string.Empty;

    // Navigation Properties
    public virtual ICollection<PatientKontakt> PatientenKontakte { get; set; } = new List<PatientKontakt>();
    public virtual ICollection<MitarbeiterKontakt> MitarbeiterKontakte { get; set; } = new List<MitarbeiterKontakt>();
    public virtual ICollection<KooperationspartnerKontakt> KooperationspartnerKontakte { get; set; } = new List<KooperationspartnerKontakt>();
}

/// <summary>
/// Kontakt-Historisierung
/// </summary>
public class KontaktHistory : BaseHistoryEntity
{
    public Guid KontakttypId { get; set; }
    public Kontakttyp Kontakttyp { get; set; } = null!;
    public string Wert { get; set; } = string.Empty;
    public bool IstHauptkontakt { get; set; }
    public bool IstValidiert { get; set; }
    public DateTime? ValidiertAm { get; set; }
    public string Notizen { get; set; } = string.Empty;
}

/// <summary>
/// Patient-Adresse-Verknüpfung
/// </summary>
public class PatientAdresse : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid AdresseId { get; set; }

    // Foreign Key für Adresstyp-Lookup
    public Guid AdresstypId { get; set; }
    public virtual Adresstyp Adresstyp { get; set; } = null!;

    public bool IstHauptadresse { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }

    // Navigation Properties
    public virtual Patient Patient { get; set; } = null!;
    public virtual Adresse Adresse { get; set; } = null!;
}

/// <summary>
/// PatientAdresse-Historisierung
/// </summary>
public class PatientAdresseHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    public Guid AdresseId { get; set; }
    public Guid AdresstypId { get; set; }
    public Adresstyp Adresstyp { get; set; } = null!;
    public bool IstHauptadresse { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }
}

/// <summary>
/// Patient-Kontakt-Verknüpfung
/// </summary>
public class PatientKontakt : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid KontaktId { get; set; }

    public bool IstHauptkontakt { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }

    // Navigation Properties
    public virtual Patient Patient { get; set; } = null!;
    public virtual Kontakt Kontakt { get; set; } = null!;
}

/// <summary>
/// PatientKontakt-Historisierung
/// </summary>
public class PatientKontaktHistory : BaseHistoryEntity
{
    public Guid PatientId { get; set; }
    public Guid KontaktId { get; set; }
    public bool IstHauptkontakt { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }
}

/// <summary>
/// Mitarbeiter-Adresse-Verknüpfung
/// </summary>
public class MitarbeiterAdresse : BaseEntity
{
    public Guid MitarbeiterId { get; set; }
    public Guid AdresseId { get; set; }

    // Foreign Key für Adresstyp-Lookup
    public Guid AdresstypId { get; set; }
    public virtual Adresstyp Adresstyp { get; set; } = null!;

    public bool IstHauptadresse { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }

    // Navigation Properties
    public virtual Mitarbeiter Mitarbeiter { get; set; } = null!;
    public virtual Adresse Adresse { get; set; } = null!;
}

/// <summary>
/// MitarbeiterAdresse-Historisierung
/// </summary>
public class MitarbeiterAdresseHistory : BaseHistoryEntity
{
    public Guid MitarbeiterId { get; set; }
    public Guid AdresseId { get; set; }
    public Guid AdresstypId { get; set; }
    public Adresstyp Adresstyp { get; set; } = null!;
    public bool IstHauptadresse { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }
}

/// <summary>
/// Mitarbeiter-Kontakt-Verknüpfung
/// </summary>
public class MitarbeiterKontakt : BaseEntity
{
    public Guid MitarbeiterId { get; set; }
    public Guid KontaktId { get; set; }

    public bool IstHauptkontakt { get; set; }
    public bool IstDienstlich { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }

    // Navigation Properties
    public virtual Mitarbeiter Mitarbeiter { get; set; } = null!;
    public virtual Kontakt Kontakt { get; set; } = null!;
}

/// <summary>
/// MitarbeiterKontakt-Historisierung
/// </summary>
public class MitarbeiterKontaktHistory : BaseHistoryEntity
{
    public Guid MitarbeiterId { get; set; }
    public Guid KontaktId { get; set; }
    public bool IstHauptkontakt { get; set; }
    public bool IstDienstlich { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }
}

/// <summary>
/// Kooperationspartner-Adresse-Verknüpfung
/// </summary>
public class KooperationspartnerAdresse : BaseEntity
{
    public Guid KooperationspartnerId { get; set; }
    public Guid AdresseId { get; set; }

    // Foreign Key für Adresstyp-Lookup
    public Guid AdresstypId { get; set; }
    public virtual Adresstyp Adresstyp { get; set; } = null!;

    public bool IstHauptadresse { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }

    // Navigation Properties
    public virtual Kooperationspartner Kooperationspartner { get; set; } = null!;
    public virtual Adresse Adresse { get; set; } = null!;
}

/// <summary>
/// KooperationspartnerAdresse-Historisierung
/// </summary>
public class KooperationspartnerAdresseHistory : BaseHistoryEntity
{
    public Guid KooperationspartnerId { get; set; }
    public Guid AdresseId { get; set; }
    public Guid AdresstypId { get; set; }
    public Adresstyp Adresstyp { get; set; } = null!;
    public bool IstHauptadresse { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }
}

/// <summary>
/// Kooperationspartner-Kontakt-Verknüpfung
/// </summary>
public class KooperationspartnerKontakt : BaseEntity
{
    public Guid KooperationspartnerId { get; set; }
    public Guid KontaktId { get; set; }

    public bool IstHauptkontakt { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }

    // Navigation Properties
    public virtual Kooperationspartner Kooperationspartner { get; set; } = null!;
    public virtual Kontakt Kontakt { get; set; } = null!;
}

/// <summary>
/// KooperationspartnerKontakt-Historisierung
/// </summary>
public class KooperationspartnerKontaktHistory : BaseHistoryEntity
{
    public Guid KooperationspartnerId { get; set; }
    public Guid KontaktId { get; set; }
    public bool IstHauptkontakt { get; set; }
    public DateTime GueltigVon { get; set; }
    public DateTime? GueltigBis { get; set; }
}
