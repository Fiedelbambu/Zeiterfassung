using Xunit;
using Moq;
using FluentAssertions;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Application.DTOs.TimeEntry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TimeEntryServiceTests
{
    private readonly Mock<ITimeEntryRepository> _repoMock;
    private readonly TimeEntryService _service;

    public TimeEntryServiceTests()
    {
        _repoMock = new Mock<ITimeEntryRepository>();
        _service = new TimeEntryService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnId_WhenValid()
    {
        // Arrange
        var dto = new CreateTimeEntryDTO
        {
            ErfasstFürUserId = Guid.NewGuid(),
            Start = DateTime.UtcNow,
            Ende = DateTime.UtcNow.AddHours(2),
            Pausenzeit = TimeSpan.FromMinutes(30),
            Beschreibung = "Montagearbeiten",
            ProjektName = "Projekt A",
            IstMontage = true
        };

        // Act
        var result = await _service.CreateAsync(dto, Guid.NewGuid());

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<TimeEntry>()), Times.Once);
    }

    [Fact]
    public async Task GetFilteredAsync_Should_Return_Filtered_List()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var from = new DateTime(2025, 4, 1);
        var to = new DateTime(2025, 4, 30);

        var timeEntries = new List<TimeEntry>
        {
            new TimeEntry
            {
                Id = Guid.NewGuid(),
                ErfasstFürUserId = userId,
                Start = new DateTime(2025, 4, 10, 8, 0, 0),
                Ende = new DateTime(2025, 4, 10, 16, 0, 0),
                Pausenzeit = TimeSpan.FromMinutes(45),
                ProjektName = "Testprojekt"
            }
        };

        _repoMock.Setup(r => r.GetFilteredAsync(from, to, userId)).ReturnsAsync(timeEntries);

        // Act
        var result = await _service.GetFilteredAsync(from, to, userId);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().HaveCount(1);
        result.Value![0].ProjektName.Should().Be("Testprojekt");
    }
}
