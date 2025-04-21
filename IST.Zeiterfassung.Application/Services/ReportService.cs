using IST.Zeiterfassung.Application.DTOs.Report;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Results;
using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Services;

public class ReportService : IReportService
{
    private readonly ITimeEntryRepository _timeEntryRepo;
    private readonly IAbsenceRepository _absenceRepo;
    private readonly IUserRepository _userRepository;

    public ReportService(
    ITimeEntryRepository timeEntryRepo,
    IAbsenceRepository absenceRepo,
    IUserRepository userRepository)
    {
        _timeEntryRepo = timeEntryRepo;
        _absenceRepo = absenceRepo;
        _userRepository = userRepository; // korrekt initialisiert
    }

    public async Task<Result<MonthlyReportDTO>> GetMonthlyReportAsync(Guid userId, int jahr, int monat)
    {
        var entries = await _timeEntryRepo.GetAllByUserIdAsync(userId);
        var absences = await _absenceRepo.GetAllByUserIdAsync(userId);

        var tage = new List<DailySummaryDTO>();
        var projekttage = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        var start = new DateTime(jahr, monat, 1);
        var end = start.AddMonths(1);

        // Gruppiere Zeiteinträge pro Tag
        var groupedEntries = entries
            .Where(e => e.Start >= start && e.Start < end)
            .GroupBy(e => e.Start.Date);

        foreach (var tag in groupedEntries)
        {
            var gearbeiteteZeit = tag.Sum(e => (e.Ende - e.Start).TotalMinutes);
            var pausen = tag.Sum(e => e.Pausenzeit.TotalMinutes);
            var beschreibungen = tag.Select(e => e.Beschreibung ?? "").Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            var projekte = tag.Select(e => e.ProjektName ?? "").Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            foreach (var projekt in projekte)
            {
                if (!projekttage.ContainsKey(projekt))
                    projekttage[projekt] = 0;

                projekttage[projekt]++;
            }

            tage.Add(new DailySummaryDTO
            {
                Datum = tag.Key,
                Gearbeitet = TimeSpan.FromMinutes(gearbeiteteZeit),
                Pause = TimeSpan.FromMinutes(pausen),
                IstMontage = tag.Any(e => e.IstMontage),
                Projekte = projekte,
                Status = "Geleistet",
                Beschreibungen = beschreibungen
            });
        }

        // Abwesenheiten zählen (pro Tag)
        var monatlicheAbsences = absences.Where(a =>
            a.StartDate < end && a.EndDate >= start).ToList();

        int urlaub = monatlicheAbsences.Count(a => a.Reason.Contains("Urlaub", StringComparison.OrdinalIgnoreCase));
        int krank = monatlicheAbsences.Count(a => a.Reason.Contains("Krank", StringComparison.OrdinalIgnoreCase));
        int home = monatlicheAbsences.Count(a => a.Reason.Contains("Home", StringComparison.OrdinalIgnoreCase));

        return Result<MonthlyReportDTO>.Ok(new MonthlyReportDTO
        {
            UserId = userId,
            Monat = monat,
            Jahr = jahr,
            Tage = tage.OrderBy(t => t.Datum).ToList(),
            Projekttage = projekttage,
            Urlaubstage = urlaub,
            Krankheitstage = krank,
            Status = "Geleistet",
            HomeOfficeTage = home
        });
    }


    public async Task<List<UserReportSummaryDTO>> GetMonthlySummaryAsync(int jahr, int monat)
    {
        var alleBenutzer = await _userRepository.GetAllAsync();

        var summaries = new List<UserReportSummaryDTO>();

        foreach (var user in alleBenutzer)
        {
            var reportResult = await GetMonthlyReportAsync(user.Id, jahr, monat);
            if (!reportResult.Success || reportResult.Value == null)
                continue;

            var report = reportResult.Value;

            // Gesamtarbeitszeit
            var netto = report.Tage.Aggregate(TimeSpan.Zero, (summe, t) => summe + t.Nettozeit);

            // Optional: feste 8h-Sollzeit je Werktag
            var start = new DateTime(jahr, monat, 1);
            var end = start.AddMonths(1);
            var werktage = Enumerable.Range(0, (end - start).Days)
                .Select(i => start.AddDays(i))
                .Count(d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday);

            var soll = TimeSpan.FromHours(werktage * 8); // Standardmodell 8h pro Werktag

            summaries.Add(new UserReportSummaryDTO
            {
                UserId = user.Id,
                Username = user.Username,
                Gesamtnettozeit = netto,
                Urlaubstage = report.Urlaubstage,
                Krankheitstage = report.Krankheitstage,
                HomeOfficeTage = report.HomeOfficeTage,
                Sollzeit = soll
            });
        }

        return summaries.OrderBy(s => s.Username).ToList();
    }



}
