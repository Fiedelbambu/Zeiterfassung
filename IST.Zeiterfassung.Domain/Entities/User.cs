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
        
        public Role Role { get; set; } = Role.Employee;
        public string FeiertagsRegion { get; set; } = "AT"; // Standard: Österreich
        public bool Aktiv { get; set; } = true;
        public DateTime ErstelltAm { get; set; } = DateTime.UtcNow;

        public List<TimeEntry> TimeEntries { get; set; } = new();
        public List<Absence> Absences { get; set; } = new();


    }
}
