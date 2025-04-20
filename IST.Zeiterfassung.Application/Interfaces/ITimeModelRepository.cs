using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface ITimeModelRepository
{
    /// <summary>
    /// Holt das Zeitmodell für einen Benutzer.
    /// </summary>
    Task<Zeitmodell?> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Erstellt oder aktualisiert ein Zeitmodell.
    /// </summary>
    Task SetOrUpdateAsync(Guid userId, Zeitmodell modell);
}
