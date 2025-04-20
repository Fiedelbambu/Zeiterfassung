using IST.Zeiterfassung.Application.DTOs.Report;
using IST.Zeiterfassung.Application.DTOs.Zeitmodell;
using IST.Zeiterfassung.Application.Results;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface ITimeModelService
{

    // Zeitmodell pro Benutzer verwalten
    Task<Result<ZeitmodellDTO>> GetByUserIdAsync(Guid userId);
    Task<Result<string>> SetOrUpdateAsync(Guid userId, ZeitmodellDTO dto);


    /// <summary>
    /// Ermittelt den Saldo eines Monats (Über-/Unterstunden) eines Benutzers.
    /// </summary>
    Task<Result<SaldoDTO>> BerechneMonatssaldoAsync(Guid userId, int jahr, int monat);
}

