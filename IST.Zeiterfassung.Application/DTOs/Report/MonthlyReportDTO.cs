namespace IST.Zeiterfassung.Application.DTOs.Report;

public class MonthlyReportDTO
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }

    public int Monat { get; set; }
    public int Jahr { get; set; }
    public Dictionary<string, int> Projekttage { get; set; } = new();

    public List<DailySummaryDTO> Tage { get; set; } = new();

    public TimeSpan Gesamtnettozeit =>
        new TimeSpan(Tage.Sum(t => t.Nettozeit.Ticks));

    public int Urlaubstage { get; set; }
    public int Krankheitstage { get; set; }
    public int HomeOfficeTage { get; set; }
}
