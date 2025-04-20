using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IST.Zeiterfassung.Application.DTOs.Absence;
using IST.Zeiterfassung.Application.Results;

namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface IAbsenceService
    {
        Task<Result<Guid>> CreateAsync(CreateAbsenceDTO dto, Guid userId);

        Task<Result<AbsenceResponseDTO>> GetByIdAsync(Guid id);

        Task<Result<List<AbsenceResponseDTO>>> GetAllForUserAsync(Guid userId);

        Task<Result<string>> DeleteAsync(Guid id, Guid userId);
        Task<Result<string>> ApproveAsync(Guid id);
        Task<Result<string>> RejectAsync(Guid id);
        Task<Result<List<AbsenceResponseDTO>>> GetAllAsync(); // Admin-Übersicht


    }
}
