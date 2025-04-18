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
    
        
        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
        public List<Absence> Absences { get; set; } = new();


    }
}
