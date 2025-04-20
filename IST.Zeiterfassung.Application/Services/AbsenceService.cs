using IST.Zeiterfassung.Application.DTOs.Absence;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Results;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.Services;

public class AbsenceService : IAbsenceService
{
    private readonly IAbsenceRepository _repository;

    public AbsenceService(IAbsenceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> CreateAsync(CreateAbsenceDTO dto, Guid userId)
    {
        if (dto.StartDate > dto.EndDate)
            return Result<Guid>.Fail("Das Startdatum darf nicht nach dem Enddatum liegen.");

        var entity = new Absence(userId, dto.Typ, dto.StartDate, dto.EndDate, dto.Reason);



        await _repository.AddAsync(entity);
        return Result<Guid>.Ok(entity.Id);
    }

    public async Task<Result<AbsenceResponseDTO>> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return Result<AbsenceResponseDTO>.Fail("Eintrag nicht gefunden.");

        var dto = MapToDTO(entity);
        return Result<AbsenceResponseDTO>.Ok(dto);
    }

    public async Task<Result<List<AbsenceResponseDTO>>> GetAllForUserAsync(Guid userId)
    {
        var entities = await _repository.GetAllByUserIdAsync(userId);
        var dtoList = entities.Select(MapToDTO).ToList();
        return Result<List<AbsenceResponseDTO>>.Ok(dtoList);
    }

    public async Task<Result<string>> DeleteAsync(Guid id, Guid userId)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return Result<string>.Fail("Eintrag nicht gefunden.");

        if (entity.UserId != userId)
            return Result<string>.Fail("Keine Berechtigung.");

        if (entity.Status != AbsenceStatus.Beantragt)
            return Result<string>.Fail("Nur beantragte Einträge können gelöscht werden.");

        await _repository.DeleteAsync(entity);
        return Result<string>.Ok("Eintrag gelöscht.");
    }

    private AbsenceResponseDTO MapToDTO(Absence entity)
    {
        return new AbsenceResponseDTO
        {
            Id = entity.Id,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Reason = entity.Reason,
            UserId = entity.UserId,
            Username = entity.User?.Username, // optional, wenn EF es geladen hat
            Duration = entity.EndDate - entity.StartDate
        };
    }

    public async Task<Result<string>> ApproveAsync(Guid id)
    {
        var entry = await _repository.GetByIdAsync(id);
        if (entry == null)
            return Result<string>.Fail("Eintrag nicht gefunden.");

        if (entry.Status != AbsenceStatus.Beantragt)
            return Result<string>.Fail("Nur beantragte Einträge können genehmigt werden.");

        entry.Status = AbsenceStatus.Genehmigt;
        await _repository.UpdateAsync(entry);

        return Result<string>.Ok("Eintrag genehmigt.");
    }

    public async Task<Result<string>> RejectAsync(Guid id)
    {
        var entry = await _repository.GetByIdAsync(id);
        if (entry == null)
            return Result<string>.Fail("Eintrag nicht gefunden.");

        if (entry.Status != AbsenceStatus.Beantragt)
            return Result<string>.Fail("Nur beantragte Einträge können abgelehnt werden.");

        entry.Status = AbsenceStatus.Abgelehnt;
        await _repository.UpdateAsync(entry);

        return Result<string>.Ok("Eintrag abgelehnt.");
    }

    public async Task<Result<List<AbsenceResponseDTO>>> GetAllAsync()
    {
        var entries = await _repository.GetAllAsync();
        var dtoList = entries.Select(MapToDTO).ToList();

        return Result<List<AbsenceResponseDTO>>.Ok(dtoList);
    }



}
