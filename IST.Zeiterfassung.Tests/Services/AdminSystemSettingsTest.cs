using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IST.Zeiterfassung.Application.DTOs.Settings;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Domain.Enums;
using Moq;
using Xunit;

namespace IST.Zeiterfassung.Tests.Services
{
    public class SystemSettingsServiceTests
    {
        private readonly Mock<ISystemSettingsRepository> _mockRepo;
        private readonly SystemSettingsService _service;

        public SystemSettingsServiceTests()
        {
            _mockRepo = new Mock<ISystemSettingsRepository>();
            _service = new SystemSettingsService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetSettingsAsync_ShouldReturnDefaults_WhenNoSettingsExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.LoadAsync()).ReturnsAsync((SystemSettings)null!);

            // Act
            var result = await _service.GetSettingsAsync();

            // Assert
            result.Should().NotBeNull();
            result.TokenConfigs.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSettingsAsync_ShouldMapCorrectly_WhenSettingsExist()
        {
            // Arrange
            var settings = new SystemSettings
            {
                Language = "de",
                FontSize = (int)FontSizeOption.Normal,
                BackgroundImageUrl = "https://example.com/bg.jpg",
                AutoSendReports = true,
                DownloadOnly = false
            };
            _mockRepo.Setup(r => r.LoadAsync()).ReturnsAsync(settings);

            // Act
            var result = await _service.GetSettingsAsync();

            // Assert
            result.Language.Should().Be("de");
            result.FontSize.Should().Be(FontSizeOption.Normal);
            result.BackgroundImageUrl.Should().Be("https://example.com/bg.jpg");
            result.AutoSendReports.Should().BeTrue();
            result.DownloadOnly.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateSettingsAsync_ShouldPersistCorrectly()
        {
            // Arrange
            var existingSettings = new SystemSettings();
            _mockRepo.Setup(r => r.LoadAsync()).ReturnsAsync(existingSettings);

            var newDto = new SystemSettingsDTO
            {
                Language = "en",
                FontSize = FontSizeOption.Large,
                BackgroundImageUrl = "https://new.url/bg.jpg",
                AutoSendReports = true,
                DownloadOnly = true,
                TokenConfigs = new List<TokenConfigDTO>
             {
                 new TokenConfigDTO
             {
            LoginType = LoginMethod.QRCode,
            ValidityDuration = TimeSpan.FromMinutes(15) // korrekt
                    }
                 }
            };

            // Act
            await _service.UpdateSettingsAsync(newDto);

            // Assert
            _mockRepo.Verify(r => r.SaveAsync(It.Is<SystemSettings>(s =>
                s.Language == "en" &&
                s.FontSize == (int)FontSizeOption.Large &&
                s.BackgroundImageUrl == "https://new.url/bg.jpg" &&
                s.AutoSendReports == true &&
                s.DownloadOnly == true &&
                s.TokenConfigs.Count == 1
                )), Times.Once);
        }
    }
}
