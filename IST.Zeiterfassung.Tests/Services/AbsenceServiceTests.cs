using Xunit;
using Moq;
using FluentAssertions;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.DTOs.Absence;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IST.Zeiterfassung.Tests.Services;

public class AbsenceServiceTests
{
    private readonly Mock<IAbsenceRepository> _repoMock;
    private readonly AbsenceService _service;

    public AbsenceServiceTests()
    {
        _repoMock = new Mock<IAbsenceRepository>();
        _service = new AbsenceService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnId_WhenValid()
    {
        // Arrange
        var dto = new CreateAbsenceDTO
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(2),
            Reason = "Urlaub",
            Typ = AbsenceType.Urlaub
        };

        var userId = Guid.NewGuid();

        // Act
        var result = await _service.CreateAsync(dto, userId);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Absence>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnDto_WhenFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new Absence
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Typ = AbsenceType.Krankheit,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Kommentar = "Test",
            Status = AbsenceStatus.Beantragt,
            ErstelltAm = DateTime.UtcNow,
            User = new User { Username = "testuser" }
        };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Reason.Should().Be("Krankheit");
        result.Value!.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnFail_WhenUserIdMismatch()
    {
        // Arrange
        var id = Guid.NewGuid();
        var wrongUser = Guid.NewGuid();
        var entry = new Absence
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Typ = AbsenceType.Krankheit,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Kommentar = "Test",
            Status = AbsenceStatus.Beantragt,
            ErstelltAm = DateTime.UtcNow,
            User = new User { Username = "testuser" }
        };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entry);

        // Act
        var result = await _service.DeleteAsync(id, wrongUser);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Keine Berechtigung");
    }

    [Fact]
    public async Task DeleteAsync_Should_Succeed_WhenUserMatches()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var entry = new Absence
        {
            Id = id,
            UserId = userId,
            Kommentar = "Test",
            Typ = AbsenceType.Krankheit,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(1),
            Status = AbsenceStatus.Beantragt
        };

        _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entry);

        // Act
        var result = await _service.DeleteAsync(id, userId);

        // Assert
        result.Success.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(entry), Times.Once);
    }
}
