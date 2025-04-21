using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface IAbsenceRepository
{
    Task AddAsync(Absence entity);
    Task<Absence?> GetByIdAsync(Guid id);
    Task<List<Absence>> GetAllByUserIdAsync(Guid userId);
    Task DeleteAsync(Absence entity);
    Task<List<Absence>> GetAllByUserIdAndRangeAsync(Guid userId, DateOnly from, DateOnly to);

    Task UpdateAsync(Absence entity);
    Task<List<Absence>> GetAllAsync();
}
