using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IST.Zeiterfassung.Persistence.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly AppDbContext _context;

    public TimeEntryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TimeEntry entry)
    {
        _context.TimeEntries.Add(entry);
        await _context.SaveChangesAsync();
    }

    public async Task<TimeEntry?> GetByIdAsync(Guid id)
    {
        return await _context.TimeEntries
            .Include(e => e.Betroffener)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<TimeEntry>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.TimeEntries
            .Where(e => e.ErfasstFürUserId == userId)
            .OrderByDescending(e => e.Start)
            .ToListAsync();
    }

    public async Task UpdateAsync(TimeEntry entry)
    {
        _context.TimeEntries.Update(entry);
        await _context.SaveChangesAsync();
    }



    public async Task DeleteAsync(TimeEntry entry)
    {
        _context.TimeEntries.Remove(entry);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TimeEntry>> GetByUserAndRangeAsync(Guid userId, DateOnly from, DateOnly to)
    {
        var fromDateTime = from.ToDateTime(TimeOnly.MinValue);
        var toDateTime = to.ToDateTime(TimeOnly.MaxValue);

        return await _context.TimeEntries
            .Where(t => t.ErfasstFürUserId == userId &&
                        t.Date >= fromDateTime && t.Date <= toDateTime)
            .ToListAsync();
    }

    public async Task<List<TimeEntry>> GetTodayEntriesAsync(Guid userId)
    {
        var heute = DateTime.UtcNow.Date;

        return await _context.TimeEntries
            .Where(t => t.ErfasstFürUserId == userId && t.Date.Date == heute)
            .OrderBy(t => t.Start)
            .ToListAsync();
    }


}
