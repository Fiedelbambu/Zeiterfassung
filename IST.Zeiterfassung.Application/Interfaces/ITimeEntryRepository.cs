using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface ITimeEntryRepository
    {
        Task AddAsync(TimeEntry entry);

        Task<TimeEntry?> GetByIdAsync(Guid id);

        Task<List<TimeEntry>> GetAllByUserIdAsync(Guid userId);
        Task<List<TimeEntry>> GetFilteredAsync(DateTime? from, DateTime? to, Guid? userId);

        Task UpdateAsync(TimeEntry entry);
        Task<List<TimeEntry>> GetTodayEntriesAsync(Guid userId);
        Task DeleteAsync(TimeEntry entry);
        Task<List<TimeEntry>> GetByUserAndRangeAsync(Guid userId, DateOnly from, DateOnly to);
        Task<List<TimeEntry>> GetAllAsync();


    }
}
