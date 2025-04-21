using System.Text.Json;
using FluentAssertions;
using IST.Zeiterfassung.Application.DTOs.Feiertage;
using IST.Zeiterfassung.Application.Services;
using IST.Zeiterfassung.Domain.Entities;
using IST.Zeiterfassung.Application.Interfaces;
using Moq;
using Xunit;

namespace IST.Zeiterfassung.Tests.Services
{
    public class FeiertagsImportServiceTests
    {
        [Fact]
        public async Task LadeUndSpeichereFeiertageAsync_Should_Call_Repository_And_Return_Result()
        {
            // Arrange
            var repoMock = new Mock<IFeiertagRepository>();
            var service = new FeiertagsImportService(repoMock.Object);

            var jahr = 2025;
            var bundeslandCode = "AT";

            // Act
            var result = await service.LadeUndSpeichereFeiertageAsync(jahr, bundeslandCode);

            // Assert
            result.Should().NotBeEmpty();
            result.All(f => f.Datum.Year == jahr).Should().BeTrue();
            repoMock.Verify(r => r.SaveAllAsync(It.IsAny<List<Feiertag>>()), Times.Once);
        }
    }
}
