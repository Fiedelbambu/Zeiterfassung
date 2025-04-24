using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Tests.Services;

public class UserServiceTests_Login
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IZeitmodellRepository> _zeitmodellRepoMock;
    private readonly Mock<ILoginAuditRepository> _auditRepoMock;
    private readonly UserService _service;
    private readonly DefaultHttpContext _httpContext;

    public UserServiceTests_Login()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _zeitmodellRepoMock = new Mock<IZeitmodellRepository>();
        _auditRepoMock = new Mock<ILoginAuditRepository>();

        _service = new UserService(_userRepoMock.Object, _zeitmodellRepoMock.Object, _auditRepoMock.Object);
        _httpContext = new DefaultHttpContext();
        _httpContext.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");
    }

    [Fact]
    public async Task LoginAsync_Should_Fail_When_User_Not_Found()
    {
        _userRepoMock.Setup(r => r.GetByUsernameAsync("unknown")).ReturnsAsync((User?)null);

        var result = await _service.LoginAsync(new LoginUserDTO { Username = "unknown", Passwort = "test" }, _httpContext);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Benutzer");
    }

    [Fact]
    public async Task LoginAsync_Should_Fail_When_Password_Invalid()
    {
        var user = new User { Username = "max", PasswordHash = BCrypt.Net.BCrypt.HashPassword("richtig") };
        _userRepoMock.Setup(r => r.GetByUsernameAsync("max")).ReturnsAsync(user);

        var result = await _service.LoginAsync(new LoginUserDTO { Username = "max", Passwort = "falsch" }, _httpContext);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Passwort");
    }

    [Fact]
    public async Task LoginByNfcAsync_Should_Return_User_When_Valid()
    {
        var user = new User { Id = Guid.NewGuid(), NfcId = "ABC123", Aktiv = true };
        _userRepoMock.Setup(r => r.GetByNfcUidAsync("ABC123")).ReturnsAsync(user);

        var result = await _service.LoginByNfcAsync("ABC123", _httpContext);

        result.Success.Should().BeTrue();
        result.Value.Should().Be(user);
    }

    [Fact]
    public async Task LoginByNfcAsync_Should_Fail_When_User_Deactivated()
    {
        var user = new User { Id = Guid.NewGuid(), NfcId = "XYZ", Aktiv = false };
        _userRepoMock.Setup(r => r.GetByNfcUidAsync("XYZ")).ReturnsAsync(user);

        var result = await _service.LoginByNfcAsync("XYZ", _httpContext);

        result.Success.Should().BeFalse();
        result.ErrorMessage.Should().Contain("nicht möglich");
    }


    [Fact]
    public async Task LoginByQrAsync_Should_Fail_When_Token_Unknown()
    {
        // Arrange
        var token = "ungültig";
        _userRepoMock.Setup(r => r.GetByQrTokenAsync(token)).ReturnsAsync((User?)null);

        // Act
        var result = await _service.LoginByQrAsync(token, _httpContext);

        // Assert
        result.Success.Should().BeFalse();
        result.ErrorMessage!.ToLowerInvariant().Should().Contain("qr"); // statt "kein benutzer"
    }

    [Fact]
    public async Task LoginByQrAsync_Should_Succeed_When_Valid()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            QrToken = "valid",
            QrTokenExpiresAt = DateTime.UtcNow.AddMinutes(5),
            Aktiv = true
        };

        _userRepoMock.Setup(r => r.GetByQrTokenAsync("valid")).ReturnsAsync(user);

        var result = await _service.LoginByQrAsync("valid", _httpContext);

        result.Success.Should().BeTrue();
        result.Value.Should().Be(user);
    }
}
