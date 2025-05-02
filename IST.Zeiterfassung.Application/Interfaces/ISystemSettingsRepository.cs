using System.Threading.Tasks;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Application.DTOs.Settings;

namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface ISystemSettingsRepository
    {
        Task<SystemSettings?> LoadAsync();
        Task SaveAsync(SystemSettings settings);
        Task<SystemSettingsDTO> GetSettingsAsync();
        Task UpdateSettingsAsync(SystemSettingsDTO dto);
    }
}
