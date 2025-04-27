using IST.Zeiterfassung.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IST.Zeiterfassung.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;            // Nachname neu
        public DateTime? BirthDate { get; set; }                         // Geburtsdatum neu
        public string EmployeeNumber { get; set; } = string.Empty;       // Mitarbeitnummer neu
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public Role Role { get; set; } = Role.Employee;
        public bool Aktiv { get; set; } = true;
        public DateTime ErstelltAm { get; set; } = DateTime.UtcNow;
        public string FeiertagsRegion { get; set; } = "AT"; // Standard: Österreich
      
        public Guid? ZeitmodellId { get; set; }
      
        [ForeignKey(nameof(ZeitmodellId))]
        public Zeitmodell? Zeitmodell { get; set; }

        public List<TimeEntry> TimeEntries { get; set; } = new();
        public List<Absence> Absences { get; set; } = new();

        // Loginoptionen
        public LoginMethod LoginMethode { get; set; } = LoginMethod.Passwort;
        public string? NfcId { get; set; }
        public string? QrToken { get; set; }
        public DateTime? QrTokenExpiresAt { get; set; }
        public DateTime? LetzteErfassung { get; set; }
        public string? LetzterLoginOrt { get; set; }
    }
}
