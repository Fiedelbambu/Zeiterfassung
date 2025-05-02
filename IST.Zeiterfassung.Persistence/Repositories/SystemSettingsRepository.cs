using IST.Zeiterfassung.Application.DTOs.Settings;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using IST.Zeiterfassung.Persistence;

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
        try
        {
            var existing = await _context.SystemSettings
                .Include(s => s.TokenConfigs)
                .FirstOrDefaultAsync(s => s.Id == settings.Id);

            if (existing == null)
            {
                Console.WriteLine("Kein SystemSettings-Eintrag mit ID gefunden: " + settings.Id);
                return;
            }

            // Hauptdaten aktualisieren
            _context.Entry(existing).CurrentValues.SetValues(settings);

            // TokenConfigs aktualisieren (vorher löschen, dann neu hinzufügen)
            _context.TokenConfigs.RemoveRange(existing.TokenConfigs);

            foreach (var token in settings.TokenConfigs)
            {
                if (token.LoginType == 0 || token.ValidityDuration == TimeSpan.Zero)
                {
                    Console.WriteLine("Überspringe fehlerhaften TokenConfig-Eintrag: " + token);
                    continue;
                }

                token.SystemSettingsId = settings.Id;
                _context.TokenConfigs.Add(token);
            }

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Console.WriteLine("DbUpdateConcurrencyException bei SaveAsync:");
            Console.WriteLine("   SystemSettings-ID: " + settings.Id);
            Console.WriteLine("   Ursache: " + ex.Message);
            throw;
        }
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
        settings.ErrorTypesToCheck = dto.ErrorTypesToCheck?.ToList() ?? new List<string>();
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
