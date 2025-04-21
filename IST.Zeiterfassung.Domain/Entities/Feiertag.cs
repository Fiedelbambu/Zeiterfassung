namespace IST.Zeiterfassung.Domain.Entities;

public class Feiertag
{
    public Guid Id { get; set; }
    public DateTime Datum { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string RegionCode { get; set; } = "AT"; // z. B. "AT-9" oder "DE-BY"
    public bool IstArbeitsfrei { get; set; } = true;
    public string Kommentar { get; set; } 
}
