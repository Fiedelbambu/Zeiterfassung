using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IST.Zeiterfassung.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAktivUndErstelltAmZuUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktiv",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ErstelltAm",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktiv",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ErstelltAm",
                table: "Users");
        }
    }
}
