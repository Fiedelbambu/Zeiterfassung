namespace IST.Zeiterfassung.Application.DTOs.Admin;

public class SystemStatusDTO
{
    public int GesamtBenutzer { get; set; }
    public int AktiveBenutzer { get; set; }
    public DateTime? LetzterLogin { get; set; }

    public int Zeitbuchungen { get; set; }
    public int Abwesenheiten { get; set; }

    public int LoginVersucheGesamt { get; set; }
    public int LoginFehlversuche { get; set; }
}
