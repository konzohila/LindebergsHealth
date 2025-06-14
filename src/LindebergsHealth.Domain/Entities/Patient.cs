namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Patient-Hauptentität
/// </summary>
public class Patient : BaseEntity
{
    public string Telefon { get; set; } = string.Empty;
    public string Versicherungsnummer { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string Vorname { get; set; } = string.Empty;
    public string Nachname { get; set; } = string.Empty;
    public DateTime Geburtsdatum { get; set; }

    // Foreign Key für Geschlecht-Lookup
    public Guid GeschlechtId { get; set; }

    // Navigation Properties
    public virtual Geschlecht Geschlecht { get; set; } = null!;
    public virtual PatientErweiterung? Erweiterung { get; set; }
    public virtual ICollection<PatientVersicherung> Versicherungen { get; set; } = new List<PatientVersicherung>();
    public virtual ICollection<PatientBeziehungsperson> Beziehungspersonen { get; set; } = new List<PatientBeziehungsperson>();
    public virtual PatientKommunikation? Kommunikation { get; set; }
    public virtual PatientEmpfehlung? Empfehlung { get; set; }
    public virtual PatientNotfallkontakt? Notfallkontakt { get; set; }

    // CRM-Bereich
    public virtual CRMStatus? CRMStatus { get; set; }
    public virtual ICollection<CRMNetzwerk> CRMNetzwerke { get; set; } = new List<CRMNetzwerk>();

    // Termine und Behandlung
    public virtual ICollection<Termin> Termine { get; set; } = new List<Termin>();
    public virtual ICollection<Koerperstatus> Koerperstatuseintraege { get; set; } = new List<Koerperstatus>();
    public virtual ICollection<TherapieSerie> Therapieserien { get; set; } = new List<TherapieSerie>();

    // Finanzen
    public virtual ICollection<Rechnung> Rechnungen { get; set; } = new List<Rechnung>();

    // Dokumente und Kommunikation
    public virtual ICollection<Dokument> Dokumente { get; set; } = new List<Dokument>();
    public virtual ICollection<Einwilligung> Einwilligungen { get; set; } = new List<Einwilligung>();
    public virtual ICollection<Kommunikationsverlauf> Kommunikationsverlaeufe { get; set; } = new List<Kommunikationsverlauf>();

    // Normalisierte Adress- und Kontaktdaten
    public virtual ICollection<PatientAdresse> Adressen { get; set; } = new List<PatientAdresse>();
    public virtual ICollection<PatientKontakt> Kontakte { get; set; } = new List<PatientKontakt>();
}

/// <summary>
/// Patient-Historisierung
/// </summary>
public class PatientHistory : BaseHistoryEntity
{
    public string Vorname { get; set; } = string.Empty;
    public string Nachname { get; set; } = string.Empty;
    public DateTime Geburtsdatum { get; set; }

    // Foreign Key für Geschlecht-Lookup
    public Guid GeschlechtId { get; set; }
    public Geschlecht Geschlecht { get; set; } = null!;
}
