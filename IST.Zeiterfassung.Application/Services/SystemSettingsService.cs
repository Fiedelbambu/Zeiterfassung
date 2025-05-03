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
            SendOnDay = settings.SendOnDay,
            ReportWithSignatureField = settings.ReportWithSignatureField,
            TokenMaxLifetime = settings.TokenMaxLifetime,
            QrTokenSingleUse = settings.QrTokenSingleUse,
            EnableReminder = settings.EnableReminder,
            RemindAfterDays = settings.RemindAfterDays,
            ErrorTypesToCheck = settings.ErrorTypesToCheck,
            HolidaySource = settings.HolidaySource,
            HolidayRegionCode = settings.HolidayRegionCode,
            AutoSyncHolidays = settings.AutoSyncHolidays,

            //diese sind schon in Verwendung


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

        settings.SendOnDay = dto.SendOnDay;
        settings.ReportWithSignatureField = dto.ReportWithSignatureField;
        settings.TokenMaxLifetime = dto.TokenMaxLifetime;
        settings.QrTokenSingleUse = dto.QrTokenSingleUse;
        settings.EnableReminder = dto.EnableReminder;
        settings.RemindAfterDays = dto.RemindAfterDays;
        settings.ErrorTypesToCheck = dto.ErrorTypesToCheck;
        settings.HolidaySource = dto.HolidaySource;
        settings.HolidayRegionCode = dto.HolidayRegionCode;
        settings.AutoSyncHolidays = dto.AutoSyncHolidays;

        //diese sind schon in Verwendung

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

    public async Task<string> UploadBackgroundImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Keine Datei übergeben");

        var fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!new[] { ".jpg", ".jpeg", ".png", ".webp" }.Contains(fileExt))
            throw new ArgumentException("Nur JPG, PNG oder WEBP erlaubt");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pics", "background");
        Directory.CreateDirectory(uploadsFolder);

        foreach (var old in Directory.GetFiles(uploadsFolder))
        {
            try { File.Delete(old); } catch { /* ignorieren */ }
        }

        var uniqueName = $"bg_{DateTime.UtcNow.Ticks}{fileExt}";
        var filePath = Path.Combine(uploadsFolder, uniqueName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var publicUrl = $"/pics/background/{uniqueName}";

        var settings = await _systemSettingsRepository.LoadAsync() ?? new SystemSettings();
        settings.BackgroundImageUrl = publicUrl;
        await _systemSettingsRepository.SaveAsync(settings);

        return publicUrl;
    }

}
