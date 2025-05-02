using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.DTOs.Settings
{
    public class SystemSettingsDTO
    {
        // Allgemein
        public string Language { get; set; } = "de";
        public FontSizeOption FontSize { get; set; } = FontSizeOption.Normal;
        public string? BackgroundImageUrl { get; set; }

        // Monatsbericht & Versand
        public bool AutoSendReports { get; set; }
        public bool DownloadOnly { get; set; }
        public int SendOnDay { get; set; }
        public bool ReportWithSignatureField { get; set; }

        // Token & Authentifizierung
        public List<TokenConfigDTO> TokenConfigs { get; set; } = new();
        public TimeSpan TokenMaxLifetime { get; set; }
        public bool QrTokenSingleUse { get; set; }

        // Erinnerung & Prüfregeln
        public bool EnableReminder { get; set; }
        public int RemindAfterDays { get; set; }
        public List<string> ErrorTypesToCheck { get; set; } = new(); // besser später als Enum-List mit Konverter

        // Feiertage
        public string HolidaySource { get; set; } = "API"; // "API" oder "Manuell"
        public string HolidayRegionCode { get; set; } = "DE-BY";
        public bool AutoSyncHolidays { get; set; }
    }

    public class TokenConfigDTO
    {
        public LoginMethod LoginType { get; set; }
        public TimeSpan ValidityDuration { get; set; }
    }
}
