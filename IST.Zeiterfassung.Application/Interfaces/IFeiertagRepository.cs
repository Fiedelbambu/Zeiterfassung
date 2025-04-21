using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface IFeiertagRepository
{
    Task<List<Feiertag>> GetAlleAsync(int jahr, string regionCode);
    /// <summary>
    /// Fügt einen einzelnen Feiertag hinzu.
    /// </summary>
    Task AddAsync(Feiertag feiertag);
    Task RemoveAsync(Guid id);
    Task<bool> ExistsAsync(DateTime datum, string regionCode);
    /// <summary>
    /// Liefert alle Feiertage in einem bestimmten Datumsbereich.
    /// </summary>
    Task<List<Feiertag>> GetByRangeAsync(DateOnly from, DateOnly to);
    /// <summary>
    /// Entfernt bestehende Feiertage für das Jahr und speichert die neuen.
    /// </summary>   
    Task SaveAllAsync(List<Feiertag> feiertage);

}
