namespace IST.Zeiterfassung.Application.DTOs.Feiertage
{
    public class FeiertagNagerDTO
    {
        public DateTime date { get; set; }
        public string localName { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string countryCode { get; set; } = "AT";
        public string? type { get; set; } // z. B. "Public"
    }
}
