namespace LindebergsHealth.Application.Termine.Dto
{
    // Für Termin-Listen (z.B. Übersicht, Kalender)
    // Für Termin-Listen (z.B. Übersicht, Kalender)
    public record TerminListDto
    {
        public Guid Id { get; init; }
        public string Titel { get; init; } = string.Empty;
        public DateTime Datum { get; init; }
        public int DauerMinuten { get; init; }
        public string? RaumName { get; init; }
        public string? PatientName { get; init; }
        // ... weitere Felder für die Übersicht
    }

    // Für Detailansicht eines Termins
    public record TerminDetailDto : TerminListDto
    {
        public string? Beschreibung { get; init; }
        public Guid? MitarbeiterId { get; init; }
        public Guid? KategorieId { get; init; }
        // ... weitere Detailfelder
    }

    // Für das Anlegen/Bearbeiten eines Termins (API-Input)
    public record CreateTerminDto
    {
        public string Titel { get; init; } = string.Empty;
        public string? Beschreibung { get; init; }
        public DateTime Datum { get; init; }
        public int DauerMinuten { get; init; }
        public Guid? MitarbeiterId { get; init; }
        public Guid? PatientId { get; init; }
        public Guid? RaumId { get; init; }
        public Guid? KategorieId { get; init; }
        // ... weitere Felder
    }

    public record UpdateTerminDto : CreateTerminDto
    {
        public Guid Id { get; init; }
    }
}
