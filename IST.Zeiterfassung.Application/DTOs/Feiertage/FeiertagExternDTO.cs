namespace IST.Zeiterfassung.Application.DTOs.Feiertage
{
    public class FeiertagExternDTO
    {
        public string datum { get; set; } = string.Empty;
        public string? hinweis { get; set; }
        public bool gesetzlich { get; set; }
    }
}
