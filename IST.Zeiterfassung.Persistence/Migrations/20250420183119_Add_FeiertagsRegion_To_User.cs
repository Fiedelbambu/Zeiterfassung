using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IST.Zeiterfassung.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_FeiertagsRegion_To_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Users_UserId",
                table: "TimeEntries");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "TimeEntries",
                newName: "ZeitmodellUserId");

            migrationBuilder.RenameColumn(
                name: "EndTime",
                table: "TimeEntries",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TimeEntries",
                newName: "Pausenzeit");

            migrationBuilder.AddColumn<string>(
                name: "FeiertagsRegion",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TimeEntries",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Beschreibung",
                table: "TimeEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Ende",
                table: "TimeEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ErfasstAm",
                table: "TimeEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ErfasstFürUserId",
                table: "TimeEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ErfasstVonUserId",
                table: "TimeEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IstMontage",
                table: "TimeEntries",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProjektName",
                table: "TimeEntries",
                type: "TEXT",
                nullable: true);

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

            migrationBuilder.AddColumn<int>(
                name: "Typ",
                table: "Absences",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Von",
                table: "Absences",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Feiertage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RegionCode = table.Column<string>(type: "TEXT", nullable: false),
                    IstArbeitsfrei = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feiertage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zeitmodelle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Bezeichnung = table.Column<string>(type: "TEXT", nullable: false),
                    WochenSollzeit = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    IstGleitzeit = table.Column<bool>(type: "INTEGER", nullable: false),
                    GleitzeitkontoAktiv = table.Column<bool>(type: "INTEGER", nullable: false),
                    GleitzeitMonatslimit = table.Column<TimeSpan>(type: "TEXT", nullable: true),
                    SaldoÜbertragAktiv = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zeitmodelle", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ErfasstFürUserId",
                table: "TimeEntries",
                column: "ErfasstFürUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ErfasstVonUserId",
                table: "TimeEntries",
                column: "ErfasstVonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ZeitmodellUserId",
                table: "TimeEntries",
                column: "ZeitmodellUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Users_ErfasstFürUserId",
                table: "TimeEntries",
                column: "ErfasstFürUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Users_ErfasstVonUserId",
                table: "TimeEntries",
                column: "ErfasstVonUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Users_UserId",
                table: "TimeEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Users_ZeitmodellUserId",
                table: "TimeEntries",
                column: "ZeitmodellUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Users_ErfasstFürUserId",
                table: "TimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Users_ErfasstVonUserId",
                table: "TimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Users_UserId",
                table: "TimeEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeEntries_Users_ZeitmodellUserId",
                table: "TimeEntries");

            migrationBuilder.DropTable(
                name: "Feiertage");

            migrationBuilder.DropTable(
                name: "Zeitmodelle");

            migrationBuilder.DropIndex(
                name: "IX_TimeEntries_ErfasstFürUserId",
                table: "TimeEntries");

            migrationBuilder.DropIndex(
                name: "IX_TimeEntries_ErfasstVonUserId",
                table: "TimeEntries");

            migrationBuilder.DropIndex(
                name: "IX_TimeEntries_ZeitmodellUserId",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "FeiertagsRegion",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Beschreibung",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "Ende",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "ErfasstAm",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "ErfasstFürUserId",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "ErfasstVonUserId",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "IstMontage",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "ProjektName",
                table: "TimeEntries");

            migrationBuilder.DropColumn(
                name: "Bis",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "ErfasstAm",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "Typ",
                table: "Absences");

            migrationBuilder.DropColumn(
                name: "Von",
                table: "Absences");

            migrationBuilder.RenameColumn(
                name: "ZeitmodellUserId",
                table: "TimeEntries",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "TimeEntries",
                newName: "EndTime");

            migrationBuilder.RenameColumn(
                name: "Pausenzeit",
                table: "TimeEntries",
                newName: "Description");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TimeEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeEntries_Users_UserId",
                table: "TimeEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
