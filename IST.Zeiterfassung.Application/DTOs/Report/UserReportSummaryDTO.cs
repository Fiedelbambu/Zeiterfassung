namespace IST.Zeiterfassung.Application.DTOs.Report;

public class UserReportSummaryDTO
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;

    public TimeSpan Gesamtnettozeit { get; set; }
    public int Urlaubstage { get; set; }
    public int Krankheitstage { get; set; }
    public int HomeOfficeTage { get; set; }

    public TimeSpan Sollzeit { get; set; }
    public TimeSpan GleitzeitAbweichung => Gesamtnettozeit - Sollzeit;
}
