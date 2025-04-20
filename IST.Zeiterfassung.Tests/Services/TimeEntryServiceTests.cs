using Xunit;
using Moq;
using FluentAssertions;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Application.DTOs.TimeEntry;

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
}
