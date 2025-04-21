using IST.Zeiterfassung.Application.DTOs.Absence;
using IST.Zeiterfassung.Application.DTOs.TimeEntry;

namespace IST.Zeiterfassung.Application.DTOs.Calendar
{
    public class CalendarDayDTO
    {
        /// <summary>
        /// Datum des Kalendertags (z. B. 2025-05-01)
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Lokalisierter Wochentag (z. B. Montag)
        /// </summary>
        public string Wochentag { get; set; } = string.Empty;

        /// <summary>
        /// Arbeitszeitbuchungen für den Tag
        /// </summary>
        public List<TimeEntryResponseDTO> TimeEntries { get; set; } = new();

        /// <summary>
        /// Abwesenheiten (Urlaub, Krankheit etc.)
        /// </summary>
        public List<AbsenceResponseDTO> Absences { get; set; } = new();

        /// <summary>
        /// Gibt an, ob der Tag ein Feiertag ist
        /// </summary>
        public bool IsFeiertag { get; set; }

        /// <summary>
        /// Name des Feiertags, falls vorhanden
        /// </summary>
        public string? Feiertagsname { get; set; }

        /// <summary>
        /// Optionaler Kommentar, z. B. „gesetzlicher Feiertag“
        /// </summary>
        public string? Bemerkung { get; set; }

        /// <summary>
        /// Sollarbeitszeit laut Zeitmodell (in Stunden)
        /// </summary>
        public double? SollzeitInStunden { get; set; }

        /// <summary>
        /// Tatsächlich gebuchte Arbeitszeit (in Stunden)
        /// </summary>
        public double? IstzeitInStunden { get; set; }

        /// <summary>
        /// Summe der Abwesenheitszeiten (z. B. 8h Urlaub)
        /// </summary>
        public double? AbwesenheitszeitInStunden { get; set; }

        /// <summary>
        /// Gibt an, ob die Kernzeit (Pflichtzeit) erfüllt wurde
        /// </summary>
        public bool IstKernzeitErfüllt { get; set; }
    }
}
