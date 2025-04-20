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
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
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

        _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                           .Returns(Task.CompletedTask);

        var service = new UserService(_userRepositoryMock.Object);

        // Act
        var result = await service.RegisterAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Value.Should().NotBeNull();

        result.Value!.Username.Should().Be(dto.Username);
        result.Value!.Email.Should().Be(dto.Email);

        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task ChangePassword_Should_UpdatePasswordHash()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, PasswordHash = "alt" };
        var dto = new ChangePasswordDTO { NeuesPasswort = "Neu123!" };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var service = new UserService(_userRepositoryMock.Object);

        // Act
        var result = await service.ChangePasswordAsync(userId, dto);

        // Assert
        result.Success.Should().BeTrue();
        _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(u =>
            BCrypt.Net.BCrypt.Verify("Neu123!", u.PasswordHash)
        )), Times.Once);
    }

    [Fact]
    public async Task ChangeRole_Should_UpdateUserRole()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Role = Role.Employee };
        var dto = new ChangeUserRoleDTO { NeueRolle = Role.Admin };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var service = new UserService(_userRepositoryMock.Object);

        // Act
        var result = await service.ChangeRoleAsync(userId, dto);

        // Assert
        result.Success.Should().BeTrue();
        user.Role.Should().Be(Role.Admin);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ToggleAktiv_Should_SwitchStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Aktiv = false };

        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var service = new UserService(_userRepositoryMock.Object);

        // Act
        var result = await service.ToggleAktivAsync(userId);

        // Assert
        result.Success.Should().BeTrue();
        user.Aktiv.Should().BeTrue();
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }
}
