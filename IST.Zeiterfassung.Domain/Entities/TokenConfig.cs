using System;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Domain.Entities
{
    public class TokenConfig
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public LoginMethod LoginType { get; set; }
        public TimeSpan ValidityDuration { get; set; }

        // FK zur SystemSettings-Tabelle
        public Guid? SystemSettingsId { get; set; }
    }
}
