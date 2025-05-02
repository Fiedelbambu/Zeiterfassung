using IST.Zeiterfassung.Application.DTOs.Settings;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace IST.Zeiterfassung.Persistence.Repositories;

public class SystemSettingsRepository : ISystemSettingsRepository
{
    private readonly AppDbContext _context;

    public SystemSettingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SystemSettings?> LoadAsync()
    {
        return await _context.SystemSettings
            .Include(s => s.TokenConfigs)
            .FirstOrDefaultAsync();
    }

    public async Task SaveAsync(SystemSettings settings)
    {
        var existing = await _context.SystemSettings
            .Include(s => s.TokenConfigs)
            .FirstOrDefaultAsync();

        if (existing == null)
        {
            await _context.SystemSettings.AddAsync(settings);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(settings);

            existing.TokenConfigs.Clear();
            foreach (var token in settings.TokenConfigs)
            {
                existing.TokenConfigs.Add(token);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task<SystemSettingsDTO> GetSettingsAsync()
    {
        var settings = await LoadAsync() ?? new SystemSettings();

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
        var settings = await LoadAsync() ?? new SystemSettings();

        settings.Language = dto.Language;
        settings.FontSize = (int)dto.FontSize;
        settings.BackgroundImageUrl = dto.BackgroundImageUrl;
        settings.AutoSendReports = dto.AutoSendReports;
        settings.DownloadOnly = dto.DownloadOnly;
        settings.SendOnDay = dto.SendOnDay;
        settings.ReportWithSignatureField = dto.ReportWithSignatureField;
        settings.TokenMaxLifetime = dto.TokenMaxLifetime;
        settings.QrTokenSingleUse = dto.QrTokenSingleUse;
        settings.EnableReminder = dto.EnableReminder;
        settings.RemindAfterDays = dto.RemindAfterDays;
        settings.ErrorTypesToCheck = string.Join(",", dto.ErrorTypesToCheck);
        settings.HolidaySource = dto.HolidaySource;
        settings.HolidayRegionCode = dto.HolidayRegionCode;
        settings.AutoSyncHolidays = dto.AutoSyncHolidays;

        settings.TokenConfigs.Clear();
        foreach (var token in dto.TokenConfigs)
        {
            settings.TokenConfigs.Add(new TokenConfig
            {
                LoginType = token.LoginType,
                ValidityDuration = token.ValidityDuration
            });
        }

        await SaveAsync(settings);
    }
}
