using Xunit;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using IST.Zeiterfassung.Application.Settings;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using System.Text;

namespace IST.Zeiterfassung.Tests.Services;

public class TokenServiceTests
{
    [Fact]
    public void CreateToken_ShouldContainExpectedClaims()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Email = "admin@example.com",
            Role = Role.Admin
        };

        var jwtSettings = new JwtSettings
        {
            Secret = Convert.ToBase64String(Encoding.UTF8.GetBytes("my_very_secret_key_2025")),
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiresInMinutes = 60
        };

        var service = new TokenService(Options.Create(jwtSettings));

        // Act
        var token = service.CreateToken(user);

        // Decode token
        var handler = new JwtSecurityTokenHandler();
        var parsedToken = handler.ReadJwtToken(token);

        // Assert
        parsedToken.Issuer.Should().Be(jwtSettings.Issuer);
        parsedToken.Audiences.Should().Contain(jwtSettings.Audience);

        parsedToken.Claims.Should().Contain(c =>
            c.Type == "nameid" && c.Value == user.Id.ToString());

        parsedToken.Claims.Should().Contain(c =>
            c.Type == "role" && c.Value == user.Role.ToString());

        parsedToken.Claims.Should().Contain(c =>
            c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);

        parsedToken.Claims.Should().Contain(c =>
            c.Type == JwtRegisteredClaimNames.UniqueName && c.Value == user.Username);

        parsedToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }
}
