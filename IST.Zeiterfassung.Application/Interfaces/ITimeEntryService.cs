using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IST.Zeiterfassung.Application.DTOs.TimeEntry;
using IST.Zeiterfassung.Application.Results;
using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Interfaces;

public interface ITimeEntryService
{
    Task<Result<Guid>> CreateAsync(CreateTimeEntryDTO dto, Guid erfasserUserId);
    Task<Result<TimeEntryResponseDTO>> GetByIdAsync(Guid id);
    Task<Result<List<TimeEntryResponseDTO>>> GetAllForUserAsync(Guid userId);
    Task<Result<string>> UpdateAsync(Guid id, UpdateTimeEntryDTO dto, Guid bearbeiterUserId);
    Task<List<TimeEntry>> GetAllAsync(DateOnly? from = null, DateOnly? to = null);
    Task<Result<List<TimeEntryResponseDTO>>> GetFilteredAsync(DateTime? from, DateTime? to, Guid? userId);
    Task<Result<string>> DeleteAsync(Guid id, Guid bearbeiterUserId);
}

