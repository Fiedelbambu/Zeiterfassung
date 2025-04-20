namespace IST.Zeiterfassung.Application.DTOs.Report;

public class DailySummaryDTO
{
    public DateTime Datum { get; set; }

    public TimeSpan Gearbeitet { get; set; }
    public TimeSpan Pause { get; set; }
    public TimeSpan Nettozeit => Gearbeitet - Pause;

    public bool IstMontage { get; set; }
    public List<string> Projekte { get; set; } = new();
    public List<string> Beschreibungen { get; set; } = new();
}
