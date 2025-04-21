using IST.Zeiterfassung.Application.DTOs.Calendar;

namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface ICalendarService
    {
        /// <summary>
        /// Liefert alle Kalendertage eines Monats mit Arbeitszeit, Abwesenheit und Feiertag
        /// </summary>
        /// <param name="userId">Benutzer-ID</param>
        /// <param name="monat">Format: YYYY-MM (z. B. 2025-05)</param>
        Task<List<CalendarDayDTO>> GetMonthlyCalendarAsync(Guid userId, string monat);
    }
}
