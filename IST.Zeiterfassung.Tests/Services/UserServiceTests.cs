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
    [Fact]
    public async Task RegisterAsync_ShouldReturnUserResponseDTO_WhenUserIsValid()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        var dto = new RegisterUserDTO
        {
            Username = "max",
            Email = "max@example.com",
            Passwort = "Geheim123"
        };

        // Fake Repo-Verhalten: AddAsync speichert den User
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

        var service = new UserService(mockRepo.Object);

        // Act
        var result = await service.RegisterAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be(dto.Username);
        result.Email.Should().Be(dto.Email);

        mockRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }
}
