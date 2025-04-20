using IST.Zeiterfassung.Application.DTOs.Zeitmodell;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Results;
using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Services;

public class TimeModelService : ITimeModelService
{
    private readonly ITimeEntryRepository _timeEntryRepo;
    private readonly ITimeModelRepository _modelRepo;
    private readonly IFeiertagsService _feiertagsService;
    private readonly IUserRepository _userRepository;

    
    public TimeModelService(
        ITimeEntryRepository timeEntryRepo,
        ITimeModelRepository modelRepo,
        IFeiertagsService feiertagsService,
        IUserRepository userRepository)
    {
        _timeEntryRepo = timeEntryRepo;
        _modelRepo = modelRepo;
        _feiertagsService = feiertagsService;
        _userRepository = userRepository;
    }




    public async Task<Result<SaldoDTO>> BerechneMonatssaldoAsync(Guid userId, int jahr, int monat)
    {
        var zeitmodell = await _modelRepo.GetByUserIdAsync(userId);
        if (zeitmodell is null)
            return Result<SaldoDTO>.Fail("Kein Zeitmodell vorhanden.");

        var einträge = await _timeEntryRepo.GetAllByUserIdAsync(userId);
        var start = new DateTime(jahr, monat, 1);
        var ende = start.AddMonths(1);

        var relevanteEinträge = einträge
            .Where(e => e.Start >= start && e.Start < ende)
            .ToList();

        var gruppiert = relevanteEinträge
            .GroupBy(e => e.Start.Date)
            .ToList();

        var gesamtnetto = TimeSpan.Zero;
        var sollgesamt = TimeSpan.Zero;

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return Result<SaldoDTO>.Fail("Benutzer nicht gefunden.");

        foreach (var tag in gruppiert)
        {
            var datum = tag.Key;
            var wochentag = datum.DayOfWeek;

            // Feiertagsprüfung
            var istFeiertag = await _feiertagsService.IstFeiertagAsync(datum, user.FeiertagsRegion);

            // Nur wenn kein Feiertag: Sollzeit berücksichtigen
            if (!istFeiertag && zeitmodell.SollzeitProTag.TryGetValue(wochentag, out var sollzeit))
            {
                sollgesamt += sollzeit;
            }

            // Nettozeit wird **immer** gerechnet, egal ob Feiertag oder nicht
            var netto = tag.Sum(e => (e.Ende - e.Start - e.Pausenzeit).Ticks);
            gesamtnetto += new TimeSpan(netto);
        }


        var dto = new SaldoDTO
        {
            UserId = userId,
            Jahr = jahr,
            Monat = monat,
            Gesamtnettozeit = gesamtnetto,
            SollzeitGesamt = sollgesamt,
            Monatslimit = zeitmodell.GleitzeitMonatslimit,
            LimitÜberschritten = zeitmodell.GleitzeitMonatslimit.HasValue &&
                                 Math.Abs((gesamtnetto - sollgesamt).TotalHours) > zeitmodell.GleitzeitMonatslimit.Value.TotalHours
        };

        return Result<SaldoDTO>.Ok(dto);
    }

    public async Task<Result<ZeitmodellDTO>> GetByUserIdAsync(Guid userId)
    {
        var modell = await _modelRepo.GetByUserIdAsync(userId);
        if (modell == null)
            return Result<ZeitmodellDTO>.Fail("Kein Zeitmodell vorhanden.");

        return Result<ZeitmodellDTO>.Ok(new ZeitmodellDTO
        {
            Id = modell.Id,
            Bezeichnung = modell.Bezeichnung,
            WochenSollzeit = modell.WochenSollzeit,
            IstGleitzeit = modell.IstGleitzeit,
            SollzeitProTag = modell.SollzeitProTag,
            GleitzeitkontoAktiv = modell.GleitzeitkontoAktiv,
            GleitzeitMonatslimit = modell.GleitzeitMonatslimit,
            SaldoÜbertragAktiv = modell.SaldoÜbertragAktiv
        });
    }

    public async Task<Result<string>> SetOrUpdateAsync(Guid userId, ZeitmodellDTO dto)
    {
        var modell = new Zeitmodell
        {
            Id = userId, // alternativ: separate UserId, falls in Entität vorhanden
            Bezeichnung = dto.Bezeichnung,
            WochenSollzeit = dto.WochenSollzeit,
            IstGleitzeit = dto.IstGleitzeit,
            SollzeitProTag = dto.SollzeitProTag,
            GleitzeitkontoAktiv = dto.GleitzeitkontoAktiv,
            GleitzeitMonatslimit = dto.GleitzeitMonatslimit,
            SaldoÜbertragAktiv = dto.SaldoÜbertragAktiv
        };

        await _modelRepo.SetOrUpdateAsync(userId, modell);
        return Result<string>.Ok("Zeitmodell gespeichert.");
    }


}
