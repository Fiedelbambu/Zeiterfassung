namespace IST.Zeiterfassung.Application.DTOs.Zeitmodell;

public class SaldoDTO
{
    public Guid UserId { get; set; }
    public int Jahr { get; set; }
    public int Monat { get; set; }

    public TimeSpan Gesamtnettozeit { get; set; }
    public TimeSpan SollzeitGesamt { get; set; }

    public TimeSpan Saldo => Gesamtnettozeit - SollzeitGesamt;

    public bool LimitÜberschritten { get; set; }
    public TimeSpan? Monatslimit { get; set; }
}
