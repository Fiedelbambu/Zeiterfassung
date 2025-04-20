using Xunit;
using Moq;
using FluentAssertions;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using System;

namespace IST.Zeiterfassung.Tests.Services;

public class TimeModelServiceTests
{
    [Fact]
    public async Task BerechneMonatssaldo_Should_IgnoreFeiertag()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var feiertag = new DateTime(2025, 12, 25); // z. B. Weihnachten

        var user = new User { Id = userId, FeiertagsRegion = "AT" };

        // korrekt gesetzte Start/Ende zur NettoBerechnung
        var entries = new List<TimeEntry>
        {
            new TimeEntry
            {
                Start = feiertag.AddHours(8),
                Ende = feiertag.AddHours(16),
                Pausenzeit = TimeSpan.FromMinutes(30),
                ErfasstFürUserId = userId
            }
        };

        var zeitmodell = new Zeitmodell
        {
            Id = userId,
            WochenSollzeit = TimeSpan.FromHours(40),
            IstGleitzeit = false,
            GleitzeitkontoAktiv = false,
            SollzeitProTag = new Dictionary<DayOfWeek, TimeSpan>
            {
                { DayOfWeek.Monday, TimeSpan.FromHours(8) },
                { DayOfWeek.Tuesday, TimeSpan.FromHours(8) },
                { DayOfWeek.Wednesday, TimeSpan.FromHours(8) },
                { DayOfWeek.Thursday, TimeSpan.FromHours(8) },
                { DayOfWeek.Friday, TimeSpan.FromHours(8) },
            }
        };

        var entryRepo = new Mock<ITimeEntryRepository>();
        entryRepo.Setup(r => r.GetAllByUserIdAsync(userId)).ReturnsAsync(entries);

        var modelRepo = new Mock<ITimeModelRepository>();
        modelRepo.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(zeitmodell);

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var feiertagsService = new Mock<IFeiertagsService>();
        feiertagsService.Setup(f => f.IstFeiertagAsync(feiertag, "AT")).ReturnsAsync(true);

        var service = new TimeModelService(entryRepo.Object, modelRepo.Object, feiertagsService.Object, userRepo.Object);

        // Act
        var result = await service.BerechneMonatssaldoAsync(userId, 2025, 12);

        // Assert
        result.Success.Should().BeTrue();
        result.Value!.SollzeitGesamt.Should().Be(TimeSpan.Zero); // Feiertag → 0h Soll
        result.Value.Gesamtnettozeit.Should().Be(TimeSpan.FromHours(7.5)); // ✅ 8h - 30min Pause
        result.Value!.Saldo.Should().Be(TimeSpan.FromHours(7.5));
    }
}
