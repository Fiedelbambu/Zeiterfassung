using IST.Zeiterfassung.Application.DTOs.Settings;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Application.Services;

public class SystemSettingsService : ISystemSettingsService
{
    private readonly ISystemSettingsRepository _systemSettingsRepository;

    public SystemSettingsService(ISystemSettingsRepository systemSettingsRepository)
    {
        _systemSettingsRepository = systemSettingsRepository;
    }

    public async Task<SystemSettingsDTO> GetSettingsAsync()
    {
        var settings = await _systemSettingsRepository.LoadAsync() ?? new SystemSettings();

        return new SystemSettingsDTO
        {
            Language = settings.Language,
            FontSize = (FontSizeOption)settings.FontSize,
            BackgroundImageUrl = settings.BackgroundImageUrl,
            AutoSendReports = settings.AutoSendReports,
            DownloadOnly = settings.DownloadOnly,
            TokenConfigs = settings.TokenConfigs.Select(tc => new TokenConfigDTO
            {
                LoginType = tc.LoginType,
                ValidityDuration = tc.ValidityDuration
            }).ToList()
        };
    }

    public async Task UpdateSettingsAsync(SystemSettingsDTO dto)
    {
        var settings = await _systemSettingsRepository.LoadAsync() ?? new SystemSettings();

        settings.Language = dto.Language;
        settings.FontSize = (int)dto.FontSize;
        settings.BackgroundImageUrl = dto.BackgroundImageUrl;
        settings.AutoSendReports = dto.AutoSendReports;
        settings.DownloadOnly = dto.DownloadOnly;

        settings.TokenConfigs.Clear();
        foreach (var token in dto.TokenConfigs)
        {
            settings.TokenConfigs.Add(new TokenConfig
            {
                LoginType = token.LoginType,
                ValidityDuration = token.ValidityDuration
            });
        }

        await _systemSettingsRepository.SaveAsync(settings);
    }
}
