namespace IST.Zeiterfassung.Application.DTOs.TimeEntry;

public class UpdateTimeEntryDTO
{
    public DateTime Start { get; set; }
    public DateTime Ende { get; set; }

    public TimeSpan Pausenzeit { get; set; }

    public string? Beschreibung { get; set; }
    public string? ProjektName { get; set; }

    public bool IstMontage { get; set; }
}

