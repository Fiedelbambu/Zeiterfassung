using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.DTOs.Absence
{
    public class CreateAbsenceRequestDTO
    {
        public AbsenceType Typ { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? Kommentar { get; set; }
    }
}
