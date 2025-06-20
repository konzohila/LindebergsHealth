namespace LindebergsHealth.BlazorApp.Models;

public class TerminUiModel
{
    public Guid Id { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string? Beschreibung { get; set; }
    public DateTime Datum { get; set; }
    public int DauerMinuten { get; set; }
    public string? RaumName { get; set; }
    public string? PatientName { get; set; }
    // Alias für Syncfusion:
    public string Subject
    {
        get => Titel;
        set => Titel = value;
    }
    // Hilfsfeld für den Kalender:
    public DateTime EndTime => Datum.AddMinutes(DauerMinuten);
}
