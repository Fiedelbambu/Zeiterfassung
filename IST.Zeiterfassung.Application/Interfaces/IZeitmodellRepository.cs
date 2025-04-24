using IST.Zeiterfassung.Domain.Entities;

public interface IZeitmodellRepository
{
    Task<Zeitmodell?> GetByIdAsync(Guid id);
}
