using IST.Zeiterfassung.Application.DTOs.Settings;
using System.Threading.Tasks;


namespace IST.Zeiterfassung.Application.Interfaces
{
    public interface ISystemSettingsService
    {
        Task<SystemSettingsDTO> GetSettingsAsync();
        Task UpdateSettingsAsync(SystemSettingsDTO dto);
        Task<string> UploadBackgroundImageAsync(IFormFile file);
    }
}
