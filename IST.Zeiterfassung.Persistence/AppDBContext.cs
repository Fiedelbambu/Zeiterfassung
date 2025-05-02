using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using IST.Zeiterfassung.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IST.Zeiterfassung.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<SystemSettings> SystemSettings => Set<SystemSettings>();
        public DbSet<TokenConfig> TokenConfigs => Set<TokenConfig>();
        public DbSet<Feiertag> Feiertage => Set<Feiertag>();
        public DbSet<User> Users => Set<User>();
        public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
        public DbSet<Absence> Absences => Set<Absence>();
        public DbSet<Zeitmodell> Zeitmodelle => Set<Zeitmodell>();
        public DbSet<LoginAudit> LoginAudits => Set<LoginAudit>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguration TimeEntry
            modelBuilder.Entity<TimeEntry>()
                .HasOne(te => te.Erfasser)
                .WithMany()
                .HasForeignKey(te => te.ErfasstVonUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimeEntry>()
                .HasOne(te => te.Betroffener)
                .WithMany()
                .HasForeignKey(te => te.ErfasstFürUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TimeEntry>()
                .HasOne(te => te.ZeitmodellUser)
                .WithMany()
                .HasForeignKey(te => te.ZeitmodellUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Absence>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Konverter für ErrorTypesToCheck (List<string>)
            var errorTypesConverter = new ValueConverter<List<string>, string>(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            );

            modelBuilder.Entity<SystemSettings>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Language).HasMaxLength(10).IsRequired();
                entity.Property(e => e.FontSize).IsRequired();
                entity.Property(e => e.BackgroundImageUrl).HasMaxLength(500);
                entity.Property(e => e.AutoSendReports).IsRequired();
                entity.Property(e => e.DownloadOnly).IsRequired();
                entity.Property(e => e.SendOnDay).IsRequired();
                entity.Property(e => e.ReportWithSignatureField).IsRequired();
                entity.Property(e => e.TokenMaxLifetime).IsRequired();
                entity.Property(e => e.QrTokenSingleUse).IsRequired();
                entity.Property(e => e.EnableReminder).IsRequired();
                entity.Property(e => e.RemindAfterDays).IsRequired();

                entity.Property(e => e.ErrorTypesToCheck)
                      .HasConversion(errorTypesConverter)
                      .HasMaxLength(500);

                entity.Property(e => e.HolidaySource).HasMaxLength(20).IsRequired();
                entity.Property(e => e.HolidayRegionCode).HasMaxLength(20).IsRequired();
                entity.Property(e => e.AutoSyncHolidays).IsRequired();

                entity.HasMany(e => e.TokenConfigs)
                      .WithOne()
                      .HasForeignKey(tc => tc.SystemSettingsId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasData(new SystemSettings
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Language = "de",
                    FontSize = 1,
                    BackgroundImageUrl = null,
                    AutoSendReports = false,
                    DownloadOnly = false,
                    SendOnDay = 1,
                    ReportWithSignatureField = false,
                    TokenMaxLifetime = TimeSpan.FromHours(24),
                    QrTokenSingleUse = true,
                    EnableReminder = false,
                    RemindAfterDays = 3,
                    ErrorTypesToCheck = new List<string> { "NurKommen", "KeinePauseEnde" },
                    HolidaySource = "API",
                    HolidayRegionCode = "DE-BY",
                    AutoSyncHolidays = true
                });
            });

            modelBuilder.Entity<TokenConfig>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.LoginType).IsRequired();
                entity.Property(t => t.ValidityDuration).IsRequired();
                entity.Property(t => t.SystemSettingsId).IsRequired();

                entity.HasData(
                    new TokenConfig
                    {
                        Id = Guid.Parse("22222222-1111-1111-1111-111111111111"),
                        LoginType = Domain.Enums.LoginMethod.Passwort,
                        ValidityDuration = TimeSpan.FromMinutes(60),
                        SystemSettingsId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                    },
                    new TokenConfig
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        LoginType = Domain.Enums.LoginMethod.QRCode,
                        ValidityDuration = TimeSpan.FromMinutes(10),
                        SystemSettingsId = Guid.Parse("11111111-1111-1111-1111-111111111111")
                    }
                );
            });
        }
    }
}
