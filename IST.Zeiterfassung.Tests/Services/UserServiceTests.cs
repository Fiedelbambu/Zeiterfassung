using Xunit;
using Moq;
using FluentAssertions;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using IST.Zeiterfassung.Application.Results;

namespace IST.Zeiterfassung.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IZeitmodellRepository> _zeitmodellRepositoryMock;
    private readonly Mock<ILoginAuditRepository> _loginAuditRepositoryMock;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _zeitmodellRepositoryMock = new Mock<IZeitmodellRepository>();
        _loginAuditRepositoryMock = new Mock<ILoginAuditRepository>();

        _service = new UserService(
            _userRepositoryMock.Object,
            _zeitmodellRepositoryMock.Object,
            _loginAuditRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Create_New_User()
    {
        // Arrange
        var dto = new CreateUserDTO
        {
            Name = "Max",
            LastName = "Mustermann",
            BirthDate = new DateTime(1990, 1, 1),
            Email = "max@example.com",
            Password = "Test1234!",
            EmployeeNumber = "12345",
            Role = "Employee",
            IsActive = true
        };

        _userRepositoryMock.Setup(r => r.GetByUsernameAsync(dto.Name)).ReturnsAsync((User?)null);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        result.Success.Should().BeTrue();
        result.Value.Username.Should().Be(dto.Name);
        result.Value.LastName.Should().Be(dto.LastName);
        result.Value.BirthDate.Should().Be(dto.BirthDate);
        result.Value.Email.Should().Be(dto.Email);
        result.Value.EmployeeNumber.Should().Be(dto.EmployeeNumber);
        result.Value.Role.Should().Be("Employee");
        result.Value.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task ChangeRoleAsync_Should_UpdateRole()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Role = Role.Employee };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var dto = new ChangeUserRoleDTO { NeueRolle = Role.Admin };

        // Act
        var result = await _service.ChangeRoleAsync(user.Id, dto);

        // Assert
        result.Success.Should().BeTrue();
        user.Role.Should().Be(Role.Admin);
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ToggleAktivAsync_Should_SwitchStatus()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), Aktiv = false };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        // Act
        var result = await _service.ToggleAktivAsync(user.Id);

        // Assert
        result.Success.Should().BeTrue();
        user.Aktiv.Should().BeTrue();
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task SetNfcIdAsync_Should_SetValue()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var dto = new SetNfcIdDTO { NfcId = "04:AB:CD:12" };

        // Act
        var result = await _service.SetNfcIdAsync(user.Id, dto);

        // Assert
        result.Success.Should().BeTrue();
        user.NfcId.Should().Be("04:AB:CD:12");
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task SetQrTokenAsync_Should_GenerateToken_WhenTrue()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var dto = new SetQrTokenDTO { NeuGenerieren = true };

        // Act
        var result = await _service.SetQrTokenAsync(user.Id, dto);

        // Assert
        result.Success.Should().BeTrue();
        user.QrToken.Should().NotBeNullOrEmpty();
        user.QrTokenExpiresAt.Should().NotBeNull();
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task SetQrTokenAsync_Should_ClearToken_WhenFalse()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            QrToken = "abc123",
            QrTokenExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };
        _userRepositoryMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var dto = new SetQrTokenDTO { NeuGenerieren = false };

        // Act
        var result = await _service.SetQrTokenAsync(user.Id, dto);

        // Assert
        result.Success.Should().BeTrue();
        user.QrToken.Should().BeNull();
        user.QrTokenExpiresAt.Should().BeNull();
        _userRepositoryMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }
}
