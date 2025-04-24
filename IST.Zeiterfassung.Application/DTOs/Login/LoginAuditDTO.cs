using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.DTOs.Login;

public class LoginAuditDTO
{
    public DateTime Zeitpunkt { get; set; }
    public string? Username { get; set; }
    public string IPAdresse { get; set; } = "";
    public bool Erfolgreich { get; set; }
    public LoginMethod LoginMethod { get; set; }
    public string? Ort { get; set; }
}
