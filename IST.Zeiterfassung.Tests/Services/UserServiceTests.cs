using Xunit;
using Moq;
using FluentAssertions;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _service = new UserService(_repoMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnUserResponseDTO_WhenUserIsValid()
    {
        // Arrange
        var dto = new RegisterUserDTO
        {
            Username = "max",
            Email = "max@example.com",
            Passwort = "Geheim123"
        };

        _repoMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                 .Returns(Task.CompletedTask);

        // Act
        var result = await _service.RegisterAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Username.Should().Be(dto.Username);
        result.Value!.Email.Should().Be(dto.Email);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task ChangePassword_Should_UpdatePasswordHash()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, PasswordHash = "alt" };
        var dto = new ChangePasswordDTO { NeuesPasswort = "Neu123!" };

        _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var result = await _service.ChangePasswordAsync(userId, dto);

        result.Success.Should().BeTrue();
        _repoMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            BCrypt.Net.BCrypt.Verify("Neu123!", u.PasswordHash)
        )), Times.Once);
    }

    [Fact]
    public async Task ChangeRole_Should_UpdateUserRole()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Role = Role.Employee };
        var dto = new ChangeUserRoleDTO { NeueRolle = Role.Admin };

        _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var result = await _service.ChangeRoleAsync(userId, dto);

        result.Success.Should().BeTrue();
        user.Role.Should().Be(Role.Admin);
        _repoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ToggleAktiv_Should_SwitchStatus()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Aktiv = false };

        _repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var result = await _service.ToggleAktivAsync(userId);

        result.Success.Should().BeTrue();
        user.Aktiv.Should().BeTrue();
        _repoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task LoginByNfcAsync_Should_ReturnUser_WhenUidValid()
    {
        var uid = "04:A3:12:FE";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "nfcUser",
            NfcId = uid,
            Aktiv = true,
            Role = Role.Admin
        };

        _repoMock.Setup(r => r.GetByNfcUidAsync(uid)).ReturnsAsync(user);

        var result = await _service.LoginByNfcAsync(uid);

        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Username.Should().Be("nfcUser");
    }

    [Fact]
    public async Task LoginByQrAsync_Should_Fail_WhenTokenNotFound()
    {
        var token = "unknown-token";

        _repoMock.Setup(r => r.GetByQrTokenAsync(token)).ReturnsAsync((User?)null);

        var result = await _service.LoginByQrAsync(token);

        result.Success.Should().BeFalse();
        result.ErrorMessage!.ToLowerInvariant().Should().Contain("kein benutzer");
    }
}
