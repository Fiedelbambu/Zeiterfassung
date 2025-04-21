using IST.Zeiterfassung.Domain.Enums;
using System.Collections.Generic;



namespace IST.Zeiterfassung.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public LoginMethod LoginMethoden { get; set; } = LoginMethod.Passwort;

        public Role Role { get; set; } = Role.Employee;
        public string FeiertagsRegion { get; set; } = "AT"; // Standard: Österreich
        public bool Aktiv { get; set; } = true;
        public DateTime ErstelltAm { get; set; } = DateTime.UtcNow;

        public List<TimeEntry> TimeEntries { get; set; } = new();
        public List<Absence> Absences { get; set; } = new();


        // Loginoptionen
        public LoginMethod LoginMethode { get; set; } = LoginMethod.Passwort;

        public string? NfcId { get; set; } // UID als Hex-String
        public string? QrToken { get; set; } // GUID oder temporärer Hash
        public DateTime? QrTokenExpiresAt { get; set; }
        public DateTime? LetzteErfassung { get; set; }
        public string? LetzterLoginOrt { get; set; }


    }
}
