using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IST.Zeiterfassung.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplyInitialSystemSettingsSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TokenConfigs",
                keyColumn: "Id",
                keyValue: new Guid("33333333-aaaa-4444-8888-111111111111"));

            migrationBuilder.DeleteData(
                table: "TokenConfigs",
                keyColumn: "Id",
                keyValue: new Guid("44444444-bbbb-5555-9999-222222222222"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TokenConfigs",
                columns: new[] { "Id", "LoginType", "SystemSettingsId", "ValidityDuration" },
                values: new object[,]
                {
                    { new Guid("33333333-aaaa-4444-8888-111111111111"), 1, new Guid("11111111-1111-1111-1111-111111111111"), new TimeSpan(0, 1, 0, 0, 0) },
                    { new Guid("44444444-bbbb-5555-9999-222222222222"), 4, new Guid("11111111-1111-1111-1111-111111111111"), new TimeSpan(0, 0, 10, 0, 0) }
                });
        }
    }
}
