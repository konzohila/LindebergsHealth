namespace LindebergsHealth.Domain.Entities;

/// <summary>
/// Seed-Daten für alle Lookup-Tabellen
/// Diese Klasse enthält die Standard-Werte, die beim Initialisieren der Datenbank eingefügt werden
/// </summary>
public static class LookupSeedData
{
    // ===== GESCHLECHT =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Geschlechter = new()
    {
        ("M", "Männlich", 1),
        ("W", "Weiblich", 2),
        ("D", "Divers", 3),
        ("U", "Unbekannt", 4)
    };

    // ===== FAMILIENSTAND =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Familienstände = new()
    {
        ("LED", "Ledig", 1),
        ("VER", "Verheiratet", 2),
        ("GES", "Getrennt lebend", 3),
        ("GES", "Geschieden", 4),
        ("VER", "Verwitwet", 5),
        ("LEP", "Lebenspartnerschaft", 6)
    };

    // ===== STAATSANGEHÖRIGKEIT =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Staatsangehörigkeiten = new()
    {
        ("DE", "Deutsch", 1),
        ("AT", "Österreichisch", 2),
        ("CH", "Schweizerisch", 3),
        ("FR", "Französisch", 4),
        ("IT", "Italienisch", 5),
        ("ES", "Spanisch", 6),
        ("PL", "Polnisch", 7),
        ("TR", "Türkisch", 8),
        ("RU", "Russisch", 9),
        ("US", "US-Amerikanisch", 10),
        ("GB", "Britisch", 11),
        ("NL", "Niederländisch", 12),
        ("BE", "Belgisch", 13),
        ("DK", "Dänisch", 14),
        ("SE", "Schwedisch", 15),
        ("NO", "Norwegisch", 16),
        ("FI", "Finnisch", 17),
        ("XX", "Staatenlos", 98),
        ("UN", "Unbekannt", 99)
    };

    // ===== VERSICHERUNGSTYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Versicherungstypen = new()
    {
        ("GKV", "Gesetzliche Krankenversicherung", 1),
        ("PKV", "Private Krankenversicherung", 2),
        ("BEI", "Beihilfe", 3),
        ("SEL", "Selbstzahler", 4),
        ("BG", "Berufsgenossenschaft", 5)
    };

    // ===== VERSICHERUNGSSTATUS =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Versicherungsstatus = new()
    {
        ("VOL", "Vollversicherung", 1),
        ("TEI", "Teilversicherung", 2),
        ("ZUS", "Zusatzversicherung", 3),
        ("FAM", "Familienversicherung", 4),
        ("STU", "Studentenversicherung", 5)
    };

    // ===== CRM STATUS TYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> CRMStatusTypen = new()
    {
        ("AKT", "Aktiv", 1),
        ("INA", "Inaktiv", 2),
        ("VIP", "VIP-Patient", 3),
        ("NEU", "Neukunde", 4),
        ("WIE", "Wiederkehrend", 5),
        ("PRO", "Problempatient", 6)
    };

    // ===== KUNDENTYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Kundentypen = new()
    {
        ("PRI", "Privatperson", 1),
        ("UNT", "Unternehmer", 2),
        ("PRO", "Prominenter", 3),
        ("SPO", "Sportler", 4),
        ("MED", "Mediziner", 5),
        ("THE", "Therapeut", 6)
    };

    // ===== EMPFEHLUNGSTYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Empfehlungstypen = new()
    {
        ("ARZ", "Arzt", 1),
        ("THE", "Therapeut", 2),
        ("FRE", "Freund/Familie", 3),
        ("KOL", "Kollege", 4),
        ("INT", "Internet", 5),
        ("SOC", "Social Media", 6),
        ("WER", "Werbung", 7),
        ("VER", "Veranstaltung", 8),
        ("PAT", "Anderer Patient", 9)
    };

    // ===== NETZWERKTYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Netzwerktypen = new()
    {
        ("FAM", "Familie", 1),
        ("FRE", "Freunde", 2),
        ("SPO", "Sport", 3),
        ("BER", "Beruflich", 4),
        ("HOB", "Hobby", 5),
        ("VER", "Verein", 6),
        ("KIR", "Kirchlich", 7),
        ("NAC", "Nachbarschaft", 8)
    };

    // ===== KOMMUNIKATIONSKANAL =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Kommunikationskanäle = new()
    {
        ("TEL", "Telefon", 1),
        ("EMA", "E-Mail", 2),
        ("SMS", "SMS", 3),
        ("WHA", "WhatsApp", 4),
        ("POS", "Post", 5),
        ("FAX", "Fax", 6),
        ("PER", "Persönlich", 7),
        ("VID", "Videoanruf", 8)
    };

    // ===== SPRACHE =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Sprachen = new()
    {
        ("DE", "Deutsch", 1),
        ("EN", "Englisch", 2),
        ("FR", "Französisch", 3),
        ("ES", "Spanisch", 4),
        ("IT", "Italienisch", 5),
        ("RU", "Russisch", 6),
        ("TR", "Türkisch", 7),
        ("PL", "Polnisch", 8),
        ("NL", "Niederländisch", 9),
        ("AR", "Arabisch", 10)
    };

    // ===== TERMINTYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Termintypen = new()
    {
        ("BEH", "Behandlung", 1),
        ("BER", "Beratung", 2),
        ("ERS", "Ersttermin", 3),
        ("FOL", "Folgetermin", 4),
        ("NOT", "Notfall", 5),
        ("INT", "Interner Termin", 6),
        ("PAU", "Pause", 7),
        ("FOR", "Fortbildung", 8),
        ("BES", "Besprechung", 9)
    };

    // ===== TERMINSTATUS =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Terminstatus = new()
    {
        ("GEP", "Geplant", 1),
        ("BES", "Bestätigt", 2),
        ("LAU", "Laufend", 3),
        ("ABG", "Abgeschlossen", 4),
        ("ABS", "Abgesagt", 5),
        ("VER", "Verschoben", 6),
        ("NIC", "Nicht erschienen", 7)
    };

    // ===== MITARBEITER FUNKTION =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> MitarbeiterFunktionen = new()
    {
        ("THE", "Therapeut", 1),
        ("PHY", "Physiotherapeut", 2),
        ("OST", "Osteopath", 3),
        ("MAS", "Masseur", 4),
        ("HEI", "Heilpraktiker", 5),
        ("REZ", "Rezeption", 6),
        ("VER", "Verwaltung", 7),
        ("LEI", "Leitung", 8),
        ("PRA", "Praxisinhaber", 9),
        ("AUS", "Auszubildender", 10)
    };

    // ===== ABTEILUNG =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Abteilungen = new()
    {
        ("OST", "Osteopathie", 1),
        ("PHY", "Physiotherapie", 2),
        ("MAS", "Massage", 3),
        ("CRM", "Customer Relations", 4),
        ("VER", "Verwaltung", 5),
        ("REZ", "Rezeption", 6),
        ("LEI", "Leitung", 7)
    };

    // ===== VERTRAGSFORM =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Vertragsformen = new()
    {
        ("VOL", "Vollzeit", 1),
        ("TEI", "Teilzeit", 2),
        ("MIN", "Minijob", 3),
        ("MID", "Midijob", 4),
        ("FRE", "Freiberuflich", 5),
        ("HON", "Honorarbasis", 6),
        ("AUS", "Ausbildung", 7),
        ("PRA", "Praktikum", 8)
    };

    // ===== GEHALTSTYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Gehaltstypen = new()
    {
        ("FIX", "Fixgehalt", 1),
        ("STU", "Stundenlohn", 2),
        ("BON", "Bonus", 3),
        ("BET", "Beteiligung", 4),
        ("PRO", "Provision", 5),
        ("HON", "Honorar", 6)
    };

    // ===== VERTRAGSSTATUS =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Vertragsstatus = new()
    {
        ("AKT", "Aktiv", 1),
        ("PRO", "Probezeit", 2),
        ("BEF", "Befristet", 3),
        ("KÜN", "Gekündigt", 4),
        ("BEE", "Beendet", 5),
        ("RUH", "Ruhend", 6)
    };

    // ===== TEILNAHMEFORM =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Teilnahmeformen = new()
    {
        ("PRÄ", "Präsenz", 1),
        ("DIG", "Digital/Online", 2),
        ("HYB", "Hybrid", 3),
        ("SEL", "Selbststudium", 4)
    };

    // ===== BEZIEHUNGSTYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Beziehungstypen = new()
    {
        ("EHE", "Ehepartner", 1),
        ("PAR", "Partner", 2),
        ("KIN", "Kind", 3),
        ("ELT", "Elternteil", 4),
        ("GES", "Geschwister", 5),
        ("GRO", "Großeltern", 6),
        ("ENK", "Enkelkind", 7),
        ("FRE", "Freund", 8),
        ("KOL", "Kollege", 9),
        ("NAC", "Nachbar", 10),
        ("BET", "Betreuer", 11),
        ("ARZ", "Arzt", 12),
        ("SON", "Sonstige", 99)
    };

    // ===== KÖRPERREGION =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Körperregionen = new()
    {
        ("HWS", "Halswirbelsäule", 1),
        ("BWS", "Brustwirbelsäule", 2),
        ("LWS", "Lendenwirbelsäule", 3),
        ("SCH", "Schulter", 4),
        ("ARM", "Arm", 5),
        ("ELL", "Ellenbogen", 6),
        ("HAN", "Hand", 7),
        ("HÜF", "Hüfte", 8),
        ("BEI", "Bein", 9),
        ("KNI", "Knie", 10),
        ("FUS", "Fuß", 11),
        ("KOP", "Kopf", 12),
        ("NAC", "Nacken", 13),
        ("RÜC", "Rücken", 14),
        ("BRU", "Brustkorb", 15),
        ("BAU", "Bauch", 16),
        ("BEC", "Becken", 17)
    };

    // ===== THERAPIESERIE STATUS =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> TherapieserieStatus = new()
    {
        ("GEP", "Geplant", 1),
        ("LAU", "Laufend", 2),
        ("UNT", "Unterbrochen", 3),
        ("BEE", "Beendet", 4),
        ("ABG", "Abgebrochen", 5)
    };

    // ===== CHECKLISTEN KATEGORIE =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> ChecklistenKategorien = new()
    {
        ("EIN", "Einarbeitung", 1),
        ("VER", "Verlaufskontrolle", 2),
        ("QUA", "Qualitätssicherung", 3),
        ("DOK", "Dokumentation", 4),
        ("HYG", "Hygiene", 5),
        ("NOT", "Notfall", 6),
        ("FOR", "Fortbildung", 7)
    };

    // ===== AUFGABEN STATUS =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> AufgabenStatus = new()
    {
        ("OFF", "Offen", 1),
        ("BEA", "In Bearbeitung", 2),
        ("ERL", "Erledigt", 3),
        ("VER", "Verschoben", 4),
        ("ABG", "Abgebrochen", 5)
    };

    // ===== EINWILLIGUNGS KATEGORIE =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> EinwilligungsKategorien = new()
    {
        ("DSG", "DSGVO Datenschutz", 1),
        ("VID", "Videoaufzeichnung", 2),
        ("FOT", "Fotoaufnahmen", 3),
        ("WER", "Werbung/Marketing", 4),
        ("FOR", "Forschung", 5),
        ("WEI", "Weiterleitung an Dritte", 6),
        ("TEL", "Telefonkontakt", 7),
        ("EMA", "E-Mail-Kontakt", 8)
    };

    // ===== FINANZ KATEGORIE =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> FinanzKategorien = new()
    {
        ("EIN", "Einnahmen", 1),
        ("AUS", "Ausgaben", 2),
        ("INV", "Investitionen", 3),
        ("STU", "Steuern", 4),
        ("VER", "Versicherungen", 5)
    };

    // ===== FINANZ TYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> FinanzTypen = new()
    {
        ("PLA", "Planung", 1),
        ("IST", "Ist-Wert", 2),
        ("DIF", "Differenz", 3),
        ("PRO", "Prognose", 4)
    };

    // ===== DATEITYP =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Dateitypen = new()
    {
        ("PDF", "PDF-Dokument", 1),
        ("DOC", "Word-Dokument", 2),
        ("XLS", "Excel-Tabelle", 3),
        ("JPG", "JPEG-Bild", 4),
        ("PNG", "PNG-Bild", 5),
        ("TIF", "TIFF-Bild", 6),
        ("TXT", "Text-Datei", 7),
        ("ZIP", "ZIP-Archiv", 8)
    };



    // ===== FACHRICHTUNG =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Fachrichtungen = new()
    {
        ("OST", "Osteopathie", 1),
        ("PHY", "Physiotherapie", 2),
        ("ORT", "Orthopädie", 3),
        ("NEU", "Neurologie", 4),
        ("CHI", "Chirurgie", 5),
        ("ALL", "Allgemeinmedizin", 6),
        ("INT", "Innere Medizin", 7),
        ("GYN", "Gynäkologie", 8),
        ("PÄD", "Pädiatrie", 9),
        ("PSY", "Psychologie", 10),
        ("RAD", "Radiologie", 11),
        ("LAB", "Labor", 12)
    };

    // ===== KOMMUNIKATIONSFORM =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> Kommunikationsformen = new()
    {
        ("TEL", "Telefon", 1),
        ("EMA", "E-Mail", 2),
        ("FAX", "Fax", 3),
        ("POS", "Post", 4),
        ("PER", "Persönlich", 5),
        ("DIG", "Digital", 6)
    };

    // ===== KOOPERATIONS STATUS =====
    public static readonly List<(string Code, string Bezeichnung, int Sortierung)> KooperationsStatus = new()
    {
        ("AKT", "Aktiv", 1),
        ("ÖFF", "Öffentlich empfohlen", 2),
        ("PRI", "Privat empfohlen", 3),
        ("RUH", "Ruhend", 4),
        ("BEE", "Beendet", 5)
    };

    // ===== DOKUMENT-BEZOGENE LOOKUPS =====

    public static readonly List<Dokumenttyp> Dokumenttypen = new()
    {
        new() { Id = Guid.NewGuid(), Code = "BEFUND", Bezeichnung = "Befund", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "RECHNUNG", Bezeichnung = "Rechnung", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "EINWILLIGUNG", Bezeichnung = "Einwilligung", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "ANAMNESE", Bezeichnung = "Anamnese", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "THERAPIEPLAN", Bezeichnung = "Therapieplan", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "ARZTBRIEF", Bezeichnung = "Arztbrief", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "REZEPT", Bezeichnung = "Rezept", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "FOTO", Bezeichnung = "Foto", Sortierung = 8, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstiges", Sortierung = 99, Aktiv = true }
    };

    // ===== ADRESS- UND KONTAKT-BEZOGENE LOOKUPS =====

    public static readonly List<Adresstyp> Adresstypen = new()
    {
        new() { Id = Guid.NewGuid(), Code = "HAUPT", Bezeichnung = "Hauptadresse", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "PRIVAT", Bezeichnung = "Privatadresse", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "GESCHAEFT", Bezeichnung = "Geschäftsadresse", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "RECHNUNGS", Bezeichnung = "Rechnungsadresse", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "LIEFER", Bezeichnung = "Lieferadresse", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "NOTFALL", Bezeichnung = "Notfalladresse", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TEMP", Bezeichnung = "Temporäre Adresse", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "ALT", Bezeichnung = "Alte Adresse", Sortierung = 8, Aktiv = false }
    };

    public static readonly List<Kontakttyp> Kontakttypen = new()
    {
        new() { Id = Guid.NewGuid(), Code = "TEL_MOBIL", Bezeichnung = "Mobiltelefon", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TEL_FEST", Bezeichnung = "Festnetz", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TEL_GESCHAEFT", Bezeichnung = "Geschäftstelefon", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "EMAIL_PRIVAT", Bezeichnung = "Private E-Mail", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "EMAIL_GESCHAEFT", Bezeichnung = "Geschäfts-E-Mail", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "FAX", Bezeichnung = "Fax", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "WHATSAPP", Bezeichnung = "WhatsApp", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TELEGRAM", Bezeichnung = "Telegram", Sortierung = 8, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SIGNAL", Bezeichnung = "Signal", Sortierung = 9, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SKYPE", Bezeichnung = "Skype", Sortierung = 10, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TEAMS", Bezeichnung = "Microsoft Teams", Sortierung = 11, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "ZOOM", Bezeichnung = "Zoom", Sortierung = 12, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "WEBSITE", Bezeichnung = "Website", Sortierung = 13, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "LINKEDIN", Bezeichnung = "LinkedIn", Sortierung = 14, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "XING", Bezeichnung = "Xing", Sortierung = 15, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstiges", Sortierung = 99, Aktiv = true }
    };

    // ===== ERWEITERTE FINANZ-BEZOGENE LOOKUPS =====

    public static readonly List<Leistungstyp> Leistungstypen = new()
    {
        new() { Id = Guid.NewGuid(), Code = "OST_ERST", Bezeichnung = "Osteopathie Erstbehandlung", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "OST_FOLGE", Bezeichnung = "Osteopathie Folgebehandlung", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "PHY_KG", Bezeichnung = "Krankengymnastik", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "PHY_MT", Bezeichnung = "Manuelle Therapie", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "PHY_MASSAGE", Bezeichnung = "Massage", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TRA_PERSONAL", Bezeichnung = "Personal Training", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TRA_GRUPPE", Bezeichnung = "Gruppentraining", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "BERATUNG", Bezeichnung = "Beratung", Sortierung = 8, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "DIAGNOSTIK", Bezeichnung = "Diagnostik", Sortierung = 9, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstiges", Sortierung = 99, Aktiv = true }
    };

    public static readonly List<Zahlungsart> Zahlungsarten = new()
    {
        new() { Id = Guid.NewGuid(), Code = "BAR", Bezeichnung = "Barzahlung", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "EC", Bezeichnung = "EC-Karte", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "KREDITKARTE", Bezeichnung = "Kreditkarte", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "UEBERWEISUNG", Bezeichnung = "Überweisung", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "LASTSCHRIFT", Bezeichnung = "Lastschrift", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "PAYPAL", Bezeichnung = "PayPal", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "KLARNA", Bezeichnung = "Klarna", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "VERSICHERUNG", Bezeichnung = "Versicherung", Sortierung = 8, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "GUTSCHEIN", Bezeichnung = "Gutschein", Sortierung = 9, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstiges", Sortierung = 99, Aktiv = true }
    };

    public static readonly List<Budgetkategorie> Budgetkategorien = new()
    {
        new() { Id = Guid.NewGuid(), Code = "UMSATZ", Bezeichnung = "Umsatzerlöse", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "PERSONAL", Bezeichnung = "Personalkosten", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "MIETE", Bezeichnung = "Miet- und Nebenkosten", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "MARKETING", Bezeichnung = "Marketing und Werbung", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "FORTBILDUNG", Bezeichnung = "Fortbildung und Weiterbildung", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "MATERIAL", Bezeichnung = "Material und Verbrauchsgüter", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "GERAETE", Bezeichnung = "Geräte und Ausstattung", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "IT", Bezeichnung = "IT und Software", Sortierung = 8, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "VERSICHERUNG", Bezeichnung = "Versicherungen", Sortierung = 9, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "STEUERN", Bezeichnung = "Steuern und Abgaben", Sortierung = 10, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstige Kosten", Sortierung = 99, Aktiv = true }
    };

    public static readonly List<Kostentyp> Kostentypen = new()
    {
        new() { Id = Guid.NewGuid(), Code = "FIXKOSTEN", Bezeichnung = "Fixkosten", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "VARIABLE", Bezeichnung = "Variable Kosten", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "EINMALIG", Bezeichnung = "Einmalige Kosten", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "INVESTITION", Bezeichnung = "Investitionen", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "ABSCHREIBUNG", Bezeichnung = "Abschreibungen", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "REPARATUR", Bezeichnung = "Reparatur und Wartung", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "BERATUNG", Bezeichnung = "Beratungskosten", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstiges", Sortierung = 99, Aktiv = true }
    };

    public static readonly List<Buchungsart> Buchungsarten = new()
    {
        new() { Id = Guid.NewGuid(), Code = "EINNAHME", Bezeichnung = "Einnahme", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "AUSGABE", Bezeichnung = "Ausgabe", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "UMBUCHUNG", Bezeichnung = "Umbuchung", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "KORREKTUR", Bezeichnung = "Korrektur", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "ABSCHREIBUNG", Bezeichnung = "Abschreibung", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "RUECKSTELLUNG", Bezeichnung = "Rückstellung", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstiges", Sortierung = 99, Aktiv = true }
    };

    // ===== ERWEITERTE TERMIN-BEZOGENE LOOKUPS =====

    public static readonly List<Blockierungsgrund> Blockierungsgründe = new()
    {
        new() { Id = Guid.NewGuid(), Code = "URLAUB", Bezeichnung = "Urlaub", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "KRANKHEIT", Bezeichnung = "Krankheit", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "FORTBILDUNG", Bezeichnung = "Fortbildung", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "KONGRESS", Bezeichnung = "Kongress", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "WARTUNG", Bezeichnung = "Wartung/Reparatur", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TEAMBESPRECHUNG", Bezeichnung = "Teambesprechung", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "VERWALTUNG", Bezeichnung = "Verwaltungszeit", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "FEIERTAG", Bezeichnung = "Feiertag", Sortierung = 8, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "BETRIEBSFERIEN", Bezeichnung = "Betriebsferien", Sortierung = 9, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstiges", Sortierung = 99, Aktiv = true }
    };

    public static readonly List<Änderungsgrund> Änderungsgründe = new()
    {
        new() { Id = Guid.NewGuid(), Code = "PATIENT_WUNSCH", Bezeichnung = "Patientenwunsch", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "PATIENT_KRANKHEIT", Bezeichnung = "Patient erkrankt", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "THERAPEUT_KRANKHEIT", Bezeichnung = "Therapeut erkrankt", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "THERAPEUT_URLAUB", Bezeichnung = "Therapeut im Urlaub", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "NOTFALL", Bezeichnung = "Notfall", Sortierung = 5, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "DOPPELBUCHUNG", Bezeichnung = "Doppelbuchung", Sortierung = 6, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "TECHNISCHE_PROBLEME", Bezeichnung = "Technische Probleme", Sortierung = 7, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "RAUMWECHSEL", Bezeichnung = "Raumwechsel erforderlich", Sortierung = 8, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "VERWALTUNG", Bezeichnung = "Administrative Gründe", Sortierung = 9, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "SONSTIGES", Bezeichnung = "Sonstiges", Sortierung = 99, Aktiv = true }
    };

    public static readonly List<Priorität> Prioritäten = new()
    {
        new() { Id = Guid.NewGuid(), Code = "NIEDRIG", Bezeichnung = "Niedrig", Sortierung = 1, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "NORMAL", Bezeichnung = "Normal", Sortierung = 2, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "HOCH", Bezeichnung = "Hoch", Sortierung = 3, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "DRINGEND", Bezeichnung = "Dringend", Sortierung = 4, Aktiv = true },
        new() { Id = Guid.NewGuid(), Code = "NOTFALL", Bezeichnung = "Notfall", Sortierung = 5, Aktiv = true }
    };
}
