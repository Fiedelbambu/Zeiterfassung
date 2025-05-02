namespace IST.Zeiterfassung.Application.Services
{
    using IST.Zeiterfassung.Application.DTOs.Settings;
    using IST.Zeiterfassung.Application.Interfaces;

    public class SettingsService : ISettingsService
    {
        private SystemSettingsDTO _cached = new(); // in echter App: DB oder Datei

        public SystemSettingsDTO Load() => _cached;

        public void Save(SystemSettingsDTO dto)
        {
            _cached = dto; // speichern (z. B. in DB/File)
        }
    }
}