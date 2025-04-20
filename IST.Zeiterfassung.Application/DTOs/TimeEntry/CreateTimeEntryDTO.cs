using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Application.DTOs.TimeEntry;

public class CreateTimeEntryDTO
{
    public Guid ErfasstFürUserId { get; set; }

    public DateTime Start { get; set; }
    public DateTime Ende { get; set; }

    public TimeSpan Pausenzeit { get; set; }

    public string? Beschreibung { get; set; }
    public string? ProjektName { get; set; }

    public bool IstMontage { get; set; }
}

