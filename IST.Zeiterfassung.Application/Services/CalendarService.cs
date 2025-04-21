using IST.Zeiterfassung.Application.DTOs.Absence;
using IST.Zeiterfassung.Application.DTOs.Calendar;
using IST.Zeiterfassung.Application.DTOs.TimeEntry;
using IST.Zeiterfassung.Application.Interfaces;
using System.Globalization;

namespace IST.Zeiterfassung.Application.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ITimeEntryRepository _timeRepo;
        private readonly IAbsenceRepository _absenceRepo;
        private readonly IFeiertagRepository _feiertagRepo;
        private readonly ITimeModelRepository _timeModelRepo;

        public CalendarService(
            ITimeEntryRepository timeRepo,
            IAbsenceRepository absenceRepo,
            IFeiertagRepository feiertagRepo,
            ITimeModelRepository timeModelRepo)
        {
            _timeRepo = timeRepo;
            _absenceRepo = absenceRepo;
            _feiertagRepo = feiertagRepo;
            _timeModelRepo = timeModelRepo;
        }

        public async Task<List<CalendarDayDTO>> GetMonthlyCalendarAsync(Guid userId, string monat)
        {
            var firstDay = DateOnly.Parse($"{monat}-01");
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            var timeEntries = await _timeRepo.GetByUserAndRangeAsync(userId, firstDay, lastDay);
            var absences = await _absenceRepo.GetAllByUserIdAndRangeAsync(userId, firstDay, lastDay);
            var feiertage = await _feiertagRepo.GetByRangeAsync(firstDay, lastDay);
            var timeModel = await _timeModelRepo.GetByUserIdAsync(userId);

            var list = new List<CalendarDayDTO>();

            for (var day = firstDay; day <= lastDay; day = day.AddDays(1))
            {
                var feiertag = feiertage.FirstOrDefault(f => DateOnly.FromDateTime(f.Datum) == day);
                var sollzeit = timeModel?.GetSollzeitInStundenFor(day);

                var tagesbuchungen = timeEntries.Where(t => DateOnly.FromDateTime(t.Date) == day).ToList();
                var tagesabsenzen = absences.Where(a =>
                    a.StartDate <= day.ToDateTime(TimeOnly.MinValue) &&
                    a.EndDate >= day.ToDateTime(TimeOnly.MinValue)).ToList();

                var istZeit = tagesbuchungen.Sum(t => t.NettoDauer.TotalHours);
                var abwesenheitZeit = tagesabsenzen.Sum(t => t.Dauer.TotalHours);

                list.Add(new CalendarDayDTO
                {
                    Date = day,
                    Wochentag = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(day.ToDateTime(TimeOnly.MinValue).DayOfWeek),
                    
                    TimeEntries = tagesbuchungen.Select(t => new TimeEntryResponseDTO  // Mapping
                    {
                        Id = t.Id,
                        Start = t.Start,
                        Ende = t.Ende,
                        Pausenzeit = t.Pausenzeit,
                        NettoDauer = t.NettoDauer,
                        ProjektName = t.ProjektName,
                        Beschreibung = t.Beschreibung
                    }).ToList(),

                   
                    
                    Absences = tagesabsenzen.Select(a => new AbsenceResponseDTO // Mapping
                    {
                        Id = a.Id,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        Reason = a.Kommentar,
                        Typ = a.Typ,
                        Status = a.Status
                    }).ToList(),     

                    IsFeiertag = feiertag != null,
                    Feiertagsname = feiertag?.Name,
                    Bemerkung = feiertag?.Kommentar,
                    SollzeitInStunden = sollzeit,
                    IstzeitInStunden = istZeit,
                    AbwesenheitszeitInStunden = abwesenheitZeit,
                    IstKernzeitErfüllt = (istZeit + abwesenheitZeit) >= (sollzeit ?? 0)
                });
            }

            return list;
        }
    }
}
