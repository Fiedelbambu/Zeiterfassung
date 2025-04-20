using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface IFeiertagRepository
{
    Task<List<Feiertag>> GetAlleAsync(int jahr, string regionCode);
    Task AddAsync(Feiertag feiertag);
    Task RemoveAsync(Guid id);
    Task<bool> ExistsAsync(DateTime datum, string regionCode);
}
