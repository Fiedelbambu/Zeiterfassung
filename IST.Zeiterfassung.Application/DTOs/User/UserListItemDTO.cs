namespace IST.Zeiterfassung.Application.DTOs.User;

public class UserListItemDTO
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "Mitarbeiter";
    public bool Aktiv { get; set; }
    public DateTime ErstelltAm { get; set; }
}
