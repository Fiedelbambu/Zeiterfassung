namespace IST.Zeiterfassung.Application.DTOs.Zeitmodell;

public class ZeitmodellDTO
{
    public Guid Id { get; set; }
    public string Bezeichnung { get; set; } = string.Empty;

    public TimeSpan WochenSollzeit { get; set; }

    public bool IstGleitzeit { get; set; }

    public Dictionary<DayOfWeek, TimeSpan> SollzeitProTag { get; set; } = new();

    public bool GleitzeitkontoAktiv { get; set; }

    public TimeSpan? GleitzeitMonatslimit { get; set; }

    public bool SaldoÜbertragAktiv { get; set; }
}
