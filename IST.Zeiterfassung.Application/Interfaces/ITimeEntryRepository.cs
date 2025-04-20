using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface ITimeEntryRepository
    {
        Task AddAsync(TimeEntry entry);

        Task<TimeEntry?> GetByIdAsync(Guid id);

        Task<List<TimeEntry>> GetAllByUserIdAsync(Guid userId);

        Task UpdateAsync(TimeEntry entry);

        Task DeleteAsync(TimeEntry entry);
    }
}
