using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IST.Zeiterfassung.Persistence.Repositories;

public class AbsenceRepository : IAbsenceRepository
{
    private readonly AppDbContext _context;

    public AbsenceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Absence entity)
    {
        _context.Absences.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Absence>> GetAllByUserIdAndRangeAsync(Guid userId, DateOnly from, DateOnly to)
    {
        var fromDateTime = from.ToDateTime(TimeOnly.MinValue);
        var toDateTime = to.ToDateTime(TimeOnly.MaxValue);

        return await _context.Absences
            .Where(a => a.UserId == userId &&
                        a.StartDate <= toDateTime &&
                        a.EndDate >= fromDateTime)
            .ToListAsync();
    }

    public async Task<Absence?> GetByIdAsync(Guid id)
    {
        return await _context.Absences
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Absence>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Absences
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.StartDate)
            .ToListAsync();
    }

    public async Task DeleteAsync(Absence entity)
    {
        _context.Absences.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Absence entity)
    {
        _context.Absences.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Absence>> GetAllAsync()
    {
        return await _context.Absences
            .Include(a => a.User) // falls Username benötigt wird
            .ToListAsync();
    }

}
