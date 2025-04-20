using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface IFeiertagsService
{
    /// <summary>
    /// Holt Feiertage für ein Jahr und eine Region (lokal oder extern).
    /// </summary>
    Task<List<Feiertag>> GetFeiertageAsync(int jahr, string region = "AT");

    /// <summary>
    /// Prüft, ob ein bestimmter Tag ein Feiertag ist.
    /// </summary>
    Task<bool> IstFeiertagAsync(DateTime datum, string region = "AT");
}
