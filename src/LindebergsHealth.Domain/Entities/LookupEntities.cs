namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Basis-Klasse für alle Lookup-Tabellen
/// </summary>
public abstract class BaseLookupEntity : BaseEntity
{
    public string Bezeichnung { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Sortierung { get; set; }
    public bool Aktiv { get; set; } = true;
    public string? Beschreibung { get; set; }
}

// ===== PATIENT-BEZOGENE LOOKUPS =====

/// <summary>
/// Geschlecht-Lookup
/// </summary>
public class Geschlecht : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Patient> Patienten { get; set; } = new List<Patient>();
    public ICollection<MitarbeiterDetails> MitarbeiterDetails { get; set; } = new List<MitarbeiterDetails>();
}

/// <summary>
/// Familienstand-Lookup
/// </summary>
public class Familienstand : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<PatientErweiterung> PatientenErweiterungen { get; set; } = new List<PatientErweiterung>();
}

/// <summary>
/// Staatsangehörigkeit-Lookup
/// </summary>
public class Staatsangehoerigkeit : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<PatientErweiterung> PatientenErweiterungen { get; set; } = new List<PatientErweiterung>();
}

/// <summary>
/// Versicherungstyp-Lookup (PKV, GKV, Beihilfe, etc.)
/// </summary>
public class Versicherungstyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<PatientVersicherung> PatientenVersicherungen { get; set; } = new List<PatientVersicherung>();
}

/// <summary>
/// Versicherungsstatus-Lookup (Vollversicherung, Teilversicherung, etc.)
/// </summary>
public class Versicherungsstatus : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<PatientVersicherung> PatientenVersicherungen { get; set; } = new List<PatientVersicherung>();
}

// ===== CRM-BEZOGENE LOOKUPS =====

/// <summary>
/// Kommunikationstyp-Lookup (Telefon, E-Mail, Brief, etc.)
/// </summary>
public class Kommunikationstyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<PatientKommunikation> PatientenKommunikationen { get; set; } = new List<PatientKommunikation>();
}

/// <summary>
/// CRM-Status-Lookup (Aktiv, Inaktiv, VIP, etc.)
/// </summary>
public class CRMStatusTyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<CRMStatus> CRMStatus { get; set; } = new List<CRMStatus>();
}

/// <summary>
/// Kundentyp-Lookup (Unternehmer, Promi, Standard, etc.)
/// </summary>
public class Kundentyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<CRMStatus> CRMStatus { get; set; } = new List<CRMStatus>();
}

/// <summary>
/// Empfehlungstyp-Lookup (Arzt, Freund, Internet, etc.)
/// </summary>
public class Empfehlungstyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<CRMNetzwerk> CRMNetzwerke { get; set; } = new List<CRMNetzwerk>();
    public ICollection<PatientEmpfehlung> PatientenEmpfehlungen { get; set; } = new List<PatientEmpfehlung>();
}

/// <summary>
/// Netzwerktyp-Lookup (Sport, Familie, Beruf, etc.)
/// </summary>
public class Netzwerktyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<CRMNetzwerk> CRMNetzwerke { get; set; } = new List<CRMNetzwerk>();
}

// ===== MITARBEITER-BEZOGENE LOOKUPS =====

/// <summary>
/// Funktion/Berufsbezeichnung-Lookup
/// </summary>
public class MitarbeiterFunktion : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<MitarbeiterVertrag> MitarbeiterVertraege { get; set; } = new List<MitarbeiterVertrag>();
}

/// <summary>
/// Abteilung-Lookup (OST, CRM, Verwaltung, etc.)
/// </summary>
public class Abteilung : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<MitarbeiterVertrag> MitarbeiterVertraege { get; set; } = new List<MitarbeiterVertrag>();
}

/// <summary>
/// Vertragsform-Lookup (Vollzeit, Teilzeit, Minijob, etc.)
/// </summary>
public class Vertragsform : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<MitarbeiterVertrag> MitarbeiterVertraege { get; set; } = new List<MitarbeiterVertrag>();
}

/// <summary>
/// Gehaltstyp-Lookup (Fix, Bonus, Beteiligung, etc.)
/// </summary>
public class Gehaltstyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<MitarbeiterVertrag> MitarbeiterVertraege { get; set; } = new List<MitarbeiterVertrag>();
}

/// <summary>
/// Vertragsstatus-Lookup (Aktiv, Gekündigt, Pausiert, etc.)
/// </summary>
public class Vertragsstatus : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<MitarbeiterVertrag> MitarbeiterVertraege { get; set; } = new List<MitarbeiterVertrag>();
}

/// <summary>
/// Teilnahmeform-Lookup für Fortbildungen (Digital, Präsenz, Hybrid)
/// </summary>
public class Teilnahmeform : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<MitarbeiterFortbildung> MitarbeiterFortbildungen { get; set; } = new List<MitarbeiterFortbildung>();
}

// ===== TERMIN-BEZOGENE LOOKUPS =====

/// <summary>
/// Termintyp-Lookup (Behandlung, Beratung, Nachkontrolle, etc.)
/// </summary>
public class Termintyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Termin> Termine { get; set; } = new List<Termin>();
}

/// <summary>
/// Terminstatus-Lookup (Geplant, Bestätigt, Abgeschlossen, Abgesagt, etc.)
/// </summary>
public class Terminstatus : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Termin> Termine { get; set; } = new List<Termin>();
}

// ===== THERAPIE-BEZOGENE LOOKUPS =====

/// <summary>
/// Therapietyp-Lookup (Osteopathie, Physiotherapie, Massage, etc.)
/// </summary>
public class Therapietyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<TherapieEinheit> TherapieEinheiten { get; set; } = new List<TherapieEinheit>();
    public ICollection<TherapieSerie> Therapieserien { get; set; } = new List<TherapieSerie>();
}

/// <summary>
/// Therapiestatus-Lookup (Geplant, Laufend, Abgeschlossen, Abgebrochen, etc.)
/// </summary>
public class Therapiestatus : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<TherapieEinheit> TherapieEinheiten { get; set; } = new List<TherapieEinheit>();
}

/// <summary>
/// Schmerztyp-Lookup (Akut, Chronisch, Stechend, Dumpf, etc.)
/// </summary>
public class Schmerztyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Koerperstatus> Koerperstatuseintraege { get; set; } = new List<Koerperstatus>();
}

/// <summary>
/// Körperregion-Lookup (LWS, HWS, Knie, etc.)
/// </summary>
public class Koerperregion : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Koerperstatus> Koerperstatuseintraege { get; set; } = new List<Koerperstatus>();
}

/// <summary>
/// Therapieserie-Status-Lookup (Laufend, Beendet, Unterbrochen, etc.)
/// </summary>
public class TherapieserieStatus : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<TherapieSerie> Therapieserien { get; set; } = new List<TherapieSerie>();
}

/// <summary>
/// Checklisten-Kategorie-Lookup (Einarbeitung, Verlauf, Qualität, etc.)
/// </summary>
public class ChecklistenKategorie : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<TherapeutenCheckliste> TherapeutenChecklisten { get; set; } = new List<TherapeutenCheckliste>();
}

/// <summary>
/// Aufgaben-Status-Lookup (Offen, In Bearbeitung, Erledigt, etc.)
/// </summary>
public class AufgabenStatus : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<TherapeutenCheckliste> TherapeutenChecklisten { get; set; } = new List<TherapeutenCheckliste>();
}

// ===== FINANZ-BEZOGENE LOOKUPS =====

/// <summary>
/// Finanz-Kategorie-Lookup (Einnahmen, Ausgaben, etc.)
/// </summary>
public class FinanzKategorie : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<FinanzPosition> FinanzPositionen { get; set; } = new List<FinanzPosition>();
}

/// <summary>
/// Finanz-Typ-Lookup (Plan, Ist, Differenz, etc.)
/// </summary>
public class FinanzTyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<FinanzPosition> FinanzPositionen { get; set; } = new List<FinanzPosition>();
}

/// <summary>
/// Rechnungsstatus-Lookup (Entwurf, Versendet, Bezahlt, Überfällig, etc.)
/// </summary>
public class Rechnungsstatus : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Rechnung> Rechnungen { get; set; } = new List<Rechnung>();
}

// ===== KOOPERATIONS-BEZOGENE LOOKUPS =====

/// <summary>
/// Fachrichtung-Lookup (Orthopädie, Neurologie, etc.)
/// </summary>
public class Fachrichtung : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Kooperationspartner> Kooperationspartner { get; set; } = new List<Kooperationspartner>();
}

/// <summary>
/// Kooperations-Status-Lookup (Aktiv, Inaktiv, Öffentlich empfohlen, etc.)
/// </summary>
public class KooperationsStatus : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<KooperationDetails> KooperationDetails { get; set; } = new List<KooperationDetails>();
}

/// <summary>
/// Kommunikationsform-Lookup (Telefon, E-Mail, Fax, etc.)
/// </summary>
public class Kommunikationsform : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Kooperationspartner> Kooperationspartner { get; set; } = new List<Kooperationspartner>();
}

// ===== DOKUMENT-BEZOGENE LOOKUPS =====

/// <summary>
/// Dokumenttyp-Lookup (Befund, Formular, Rechnung, etc.)
/// </summary>
public class Dokumenttyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Dokument> Dokumente { get; set; } = new List<Dokument>();
}

/// <summary>
/// Dateityp-Lookup (PDF, DOCX, JPG, etc.)
/// </summary>
public class Dateityp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Dokument> Dokumente { get; set; } = new List<Dokument>();
}

/// <summary>
/// Lookup für Adresstypen
/// </summary>
public class Adresstyp : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Kontakttypen
/// </summary>
public class Kontakttyp : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Leistungstypen
/// </summary>
public class Leistungstyp : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Zahlungsarten
/// </summary>
public class Zahlungsart : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Budgetkategorien
/// </summary>
public class Budgetkategorie : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Kostentypen
/// </summary>
public class Kostentyp : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Buchungsarten
/// </summary>
public class Buchungsart : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Blockierungsgründe
/// </summary>
public class Blockierungsgrund : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Änderungsgründe
/// </summary>
public class Änderungsgrund : BaseLookupEntity
{
}

/// <summary>
/// Lookup für Prioritäten
/// </summary>
public class Priorität : BaseLookupEntity
{
}

// ===== KOMMUNIKATIONS-BEZOGENE LOOKUPS =====

/// <summary>
/// Kommunikationskanal-Lookup (Telefon, E-Mail, SMS, etc.)
/// </summary>
public class Kommunikationskanal : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Kommunikationsverlauf> Kommunikationsverlaeufe { get; set; } = new List<Kommunikationsverlauf>();
    public ICollection<PatientKommunikation> PatientenKommunikation { get; set; } = new List<PatientKommunikation>();
}

/// <summary>
/// Einwilligungs-Kategorie-Lookup (DSGVO, Video, Marketing, etc.)
/// </summary>
public class EinwilligungsKategorie : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Einwilligung> Einwilligungen { get; set; } = new List<Einwilligung>();
}

/// <summary>
/// Beziehungstyp-Lookup (Partner, Eltern, Kind, Bevollmächtigter, etc.)
/// </summary>
public class Beziehungstyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<PatientBeziehungsperson> PatientenBeziehungspersonen { get; set; } = new List<PatientBeziehungsperson>();
    public ICollection<PatientNotfallkontakt> PatientenNotfallkontakte { get; set; } = new List<PatientNotfallkontakt>();
    public ICollection<MitarbeiterNotfallkontakt> MitarbeiterNotfallkontakte { get; set; } = new List<MitarbeiterNotfallkontakt>();
}

/// <summary>
/// Sprache-Lookup (Deutsch, Englisch, etc.)
/// </summary>
public class Sprache : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<PatientKommunikation> PatientenKommunikation { get; set; } = new List<PatientKommunikation>();
}

// ===== PRAXIS-BEZOGENE LOOKUPS =====

/// <summary>
/// Raumtyp-Lookup (Behandlungsraum, Büro, Wartebereich, etc.)
/// </summary>
public class Raumtyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Raum> Räume { get; set; } = new List<Raum>();
}

/// <summary>
/// Ausstattungstyp-Lookup (Behandlungsliege, Computer, Medizingerät, etc.)
/// </summary>
public class Ausstattungstyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Ausstattung> Ausstattungen { get; set; } = new List<Ausstattung>();
}

/// <summary>
/// Kooperationstyp-Lookup (Überweiser, Partner, Netzwerk, etc.)
/// </summary>
public class Kooperationstyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Kooperationspartner> Kooperationspartner { get; set; } = new List<Kooperationspartner>();
}

/// <summary>
/// Einwilligungstyp-Lookup (DSGVO, Behandlung, Foto, etc.)
/// </summary>
public class Einwilligungstyp : BaseLookupEntity
{
    // Navigation Properties
    public ICollection<Einwilligung> Einwilligungen { get; set; } = new List<Einwilligung>();
} 