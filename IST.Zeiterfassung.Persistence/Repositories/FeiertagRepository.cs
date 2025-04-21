using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IST.Zeiterfassung.Persistence.Repositories;

public class FeiertagRepository : IFeiertagRepository
{
    private readonly AppDbContext _context;

    public FeiertagRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Feiertag>> GetAlleAsync(int jahr, string regionCode)
    {
        return await _context.Feiertage
            .Where(f => f.Datum.Year == jahr && f.RegionCode == regionCode)
            .ToListAsync();
    }

    public async Task AddAsync(Feiertag feiertag)
    {
        _context.Feiertage.Add(feiertag);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid id)
    {
        var entity = await _context.Feiertage.FindAsync(id);
        if (entity != null)
        {
            _context.Feiertage.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(DateTime datum, string regionCode)
    {
        return await _context.Feiertage
            .AnyAsync(f => f.Datum == datum.Date && f.RegionCode == regionCode);
    }

    public async Task<List<Feiertag>> GetByRangeAsync(DateOnly from, DateOnly to)
    {
        var fromDateTime = from.ToDateTime(TimeOnly.MinValue);
        var toDateTime = to.ToDateTime(TimeOnly.MaxValue);

        return await _context.Feiertage
            .Where(f => f.Datum >= fromDateTime && f.Datum <= toDateTime)
            .ToListAsync();
    }

    public async Task SaveAllAsync(List<Feiertag> feiertage)
    {
        if (!feiertage.Any())
            return;

        var jahr = feiertage.First().Datum.Year;

        var vorhandene = _context.Feiertage
            .Where(f => f.Datum.Year == jahr && f.RegionCode == feiertage.First().RegionCode);

        _context.Feiertage.RemoveRange(vorhandene);

        await _context.Feiertage.AddRangeAsync(feiertage);
        await _context.SaveChangesAsync();
    }


}
