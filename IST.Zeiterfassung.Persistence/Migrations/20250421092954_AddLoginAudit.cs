using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IST.Zeiterfassung.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLoginAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bis",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "ErfasstAm",
                table: "Absences");

            migrationBuilder.RenameColumn(
                name: "Von",
                table: "Absences",
                newName: "ErstelltAm");

            migrationBuilder.AddColumn<DateTime>(
                name: "LetzteErfassung",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LetzterLoginOrt",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoginMethode",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LoginMethoden",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NfcId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrToken",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QrTokenExpiresAt",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "TimeEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Kommentar",
                table: "Feiertage",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Kommentar",
                table: "Absences",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Absences",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "Absences",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginAudits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LoginMethod = table.Column<int>(type: "INTEGER", nullable: false),
                    Erfolgreich = table.Column<bool>(type: "INTEGER", nullable: false),
                    IPAdresse = table.Column<string>(type: "TEXT", nullable: true),
                    Ort = table.Column<string>(type: "TEXT", nullable: true),
                    Zeitpunkt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginAudits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absences_UserId1",
                table: "Absences",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAudits_UserId",
                table: "LoginAudits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Absences_Users_UserId1",
                table: "Absences",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Absences_Users_UserId1",
                table: "Absences");

            migrationBuilder.DropTable(
                name: "LoginAudits");

            migrationBuilder.DropIndex(
                name: "IX_Absences_UserId1",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "LetzteErfassung",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LetzterLoginOrt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LoginMethode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LoginMethoden",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NfcId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QrToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "QrTokenExpiresAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "Kommentar",
                table: "Feiertage");

            migrationBuilder.DropColumn(
                name: "Kommentar",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Absences");

            migrationBuilder.RenameColumn(
                name: "ErstelltAm",
                table: "Absences",
                newName: "Von");

            migrationBuilder.AddColumn<DateTime>(
                name: "Bis",
                table: "Absences",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ErfasstAm",
                table: "Absences",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
