using System;
using System.Collections.Generic;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Domain.Entities
{
    public class SystemSettings
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // Allgemein
        public string Language { get; set; } = "de";
        public int FontSize { get; set; } = 1; // Enum int-codiert
        public string? BackgroundImageUrl { get; set; }

        // Monatsbericht & Versand
        public bool AutoSendReports { get; set; }
        public bool DownloadOnly { get; set; }
        public int SendOnDay { get; set; } = 1;
        public bool ReportWithSignatureField { get; set; }

        // Authentifizierung & Token
        public List<TokenConfig> TokenConfigs { get; set; } = new();
        public TimeSpan TokenMaxLifetime { get; set; } = TimeSpan.FromHours(24);
        public bool QrTokenSingleUse { get; set; } = true;

        // Erinnerung & Prüfung
        public bool EnableReminder { get; set; } = false;
        public int RemindAfterDays { get; set; } = 3;
        public List<string> ErrorTypesToCheck { get; set; } = new();

        // Feiertage
        public string HolidaySource { get; set; } = "API"; // API oder Manuell
        public string HolidayRegionCode { get; set; } = "DE-BY";
        public bool AutoSyncHolidays { get; set; } = true;
    }
}
