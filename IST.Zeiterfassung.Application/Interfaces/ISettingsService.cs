namespace IST.Zeiterfassung.Application.Interfaces
{
    using IST.Zeiterfassung.Application.DTOs.Settings;

    public interface ISettingsService
    {
        SystemSettingsDTO Load();
        void Save(SystemSettingsDTO dto);
    }
}
