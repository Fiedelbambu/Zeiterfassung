using IST.Zeiterfassung.Application.DTOs.TimeEntry;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Results;
using IST.Zeiterfassung.Domain.Entities;


namespace IST.Zeiterfassung.Application.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _repository;

    public TimeEntryService(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> CreateAsync(CreateTimeEntryDTO dto, Guid erfasserUserId)
    {
        var entity = new TimeEntry
        {
            Id = Guid.NewGuid(),
            ErfasstVonUserId = erfasserUserId,
            ErfasstFürUserId = dto.ErfasstFürUserId,
            Start = dto.Start,
            Ende = dto.Ende,
            Pausenzeit = dto.Pausenzeit,
            Beschreibung = dto.Beschreibung,
            ProjektName = dto.ProjektName,
            IstMontage = dto.IstMontage,
            ErfasstAm = DateTime.UtcNow
        };

        await _repository.AddAsync(entity);

        return Result<Guid>.Ok(entity.Id);
    }

    public async Task<Result<TimeEntryResponseDTO>> GetByIdAsync(Guid id)
    {
        var entry = await _repository.GetByIdAsync(id);
        if (entry is null)
            return Result<TimeEntryResponseDTO>.Fail("Eintrag nicht gefunden.");

        return Result<TimeEntryResponseDTO>.Ok(MapToDTO(entry));
    }

    public async Task<Result<List<TimeEntryResponseDTO>>> GetAllForUserAsync(Guid userId)
    {
        var entries = await _repository.GetAllByUserIdAsync(userId);

        var list = entries.Select(MapToDTO).ToList();
        return Result<List<TimeEntryResponseDTO>>.Ok(list);
    }

    public async Task<Result<string>> UpdateAsync(Guid id, UpdateTimeEntryDTO dto, Guid bearbeiterUserId)
    {
        var entry = await _repository.GetByIdAsync(id);
        if (entry is null)
            return Result<string>.Fail("Eintrag nicht gefunden.");

        entry.Start = dto.Start;
        entry.Ende = dto.Ende;
        entry.Pausenzeit = dto.Pausenzeit;
        entry.Beschreibung = dto.Beschreibung;
        entry.ProjektName = dto.ProjektName;
        entry.IstMontage = dto.IstMontage;

        await _repository.UpdateAsync(entry);

        return Result<string>.Ok("Eintrag aktualisiert.");
    }

    public async Task<Result<string>> DeleteAsync(Guid id, Guid bearbeiterUserId)
    {
        var entry = await _repository.GetByIdAsync(id);
        if (entry is null)
            return Result<string>.Fail("Eintrag nicht gefunden.");

        await _repository.DeleteAsync(entry);
        return Result<string>.Ok("Eintrag gelöscht.");
    }

    private TimeEntryResponseDTO MapToDTO(TimeEntry entry)
    {
        return new TimeEntryResponseDTO
        {
            Id = entry.Id,
            ErfasstFürUserId = entry.ErfasstFürUserId,
            Benutzername = entry.Betroffener?.Username,
            Start = entry.Start,
            Ende = entry.Ende,
            Pausenzeit = entry.Pausenzeit,
            Beschreibung = entry.Beschreibung,
            ProjektName = entry.ProjektName,
            IstMontage = entry.IstMontage,
            ErfasstAm = entry.ErfasstAm,
            NettoDauer = entry.NettoDauer
        };
    }
}
