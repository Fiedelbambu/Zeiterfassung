using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net;

using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Application.DTOs.User;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;

namespace IST.Zeiterfassung.Tests.Services;

public class UserServiceAdminTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IZeitmodellRepository> _zeitmodellRepoMock;
    private readonly Mock<ILoginAuditRepository> _auditRepoMock;
    private readonly UserService _service;

    public UserServiceAdminTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _zeitmodellRepoMock = new Mock<IZeitmodellRepository>();
        _auditRepoMock = new Mock<ILoginAuditRepository>();

        _service = new UserService(
            _userRepoMock.Object,
            _zeitmodellRepoMock.Object,
            _auditRepoMock.Object
        );
    }

    [Fact]
    public async Task ChangeRoleAsync_Should_UpdateRole()
    {
        var user = new User { Id = Guid.NewGuid(), Role = Role.Employee };
        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var dto = new ChangeUserRoleDTO { NeueRolle = Role.Admin };
        var result = await _service.ChangeRoleAsync(user.Id, dto);

        result.Success.Should().BeTrue();
        user.Role.Should().Be(Role.Admin);
        _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ToggleAktivAsync_Should_SwitchStatus()
    {
        var user = new User { Id = Guid.NewGuid(), Aktiv = false };
        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var result = await _service.ToggleAktivAsync(user.Id);

        result.Success.Should().BeTrue();
        user.Aktiv.Should().BeTrue();
        _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task SetNfcIdAsync_Should_SetValue()
    {
        var user = new User { Id = Guid.NewGuid() };
        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var dto = new SetNfcIdDTO { NfcId = "04:AB:CD:12" };
        var result = await _service.SetNfcIdAsync(user.Id, dto);

        result.Success.Should().BeTrue();
        user.NfcId.Should().Be("04:AB:CD:12");
        _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task SetQrTokenAsync_Should_GenerateToken_WhenTrue()
    {
        var user = new User { Id = Guid.NewGuid() };
        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var dto = new SetQrTokenDTO { NeuGenerieren = true };
        var result = await _service.SetQrTokenAsync(user.Id, dto);

        result.Success.Should().BeTrue();
        user.QrToken.Should().NotBeNullOrEmpty();
        user.QrTokenExpiresAt.Should().NotBeNull();
        _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task SetQrTokenAsync_Should_ClearToken_WhenFalse()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            QrToken = "abc123",
            QrTokenExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };
        _userRepoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

        var dto = new SetQrTokenDTO { NeuGenerieren = false };
        var result = await _service.SetQrTokenAsync(user.Id, dto);

        result.Success.Should().BeTrue();
        user.QrToken.Should().BeNull();
        user.QrTokenExpiresAt.Should().BeNull();
        _userRepoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task LoginByNfcAsync_Should_LogAttempt_And_Fail_IfUserNotFound()
    {
        var uid = "00:11:22:33";
        _userRepoMock.Setup(r => r.GetByNfcUidAsync(uid)).ReturnsAsync((User?)null);

        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = IPAddress.Loopback;

        var result = await _service.LoginByNfcAsync(uid, context);

        result.Success.Should().BeFalse();
        _auditRepoMock.Verify(a => a.AddAsync(It.IsAny<LoginAudit>()), Times.Once);
    }
}
