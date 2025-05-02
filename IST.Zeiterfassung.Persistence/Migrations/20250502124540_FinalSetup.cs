using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IST.Zeiterfassung.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FinalSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feiertage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    RegionCode = table.Column<string>(type: "TEXT", nullable: false),
                    IstArbeitsfrei = table.Column<bool>(type: "INTEGER", nullable: false),
                    Kommentar = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feiertage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    FontSize = table.Column<int>(type: "INTEGER", nullable: false),
                    BackgroundImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    AutoSendReports = table.Column<bool>(type: "INTEGER", nullable: false),
                    DownloadOnly = table.Column<bool>(type: "INTEGER", nullable: false),
                    SendOnDay = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportWithSignatureField = table.Column<bool>(type: "INTEGER", nullable: false),
                    TokenMaxLifetime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    QrTokenSingleUse = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableReminder = table.Column<bool>(type: "INTEGER", nullable: false),
                    RemindAfterDays = table.Column<int>(type: "INTEGER", nullable: false),
                    ErrorTypesToCheck = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    HolidaySource = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    HolidayRegionCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    AutoSyncHolidays = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "TokenConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginType = table.Column<int>(type: "INTEGER", nullable: false),
                    ValidityDuration = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    SystemSettingsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenConfigs_SystemSettings_SystemSettingsId",
                        column: x => x.SystemSettingsId,
                        principalTable: "SystemSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EmployeeNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    Aktiv = table.Column<bool>(type: "INTEGER", nullable: false),
                    ErstelltAm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FeiertagsRegion = table.Column<string>(type: "TEXT", nullable: false),
                    ZeitmodellId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LoginMethode = table.Column<int>(type: "INTEGER", nullable: false),
                    NfcId = table.Column<string>(type: "TEXT", nullable: true),
                    QrToken = table.Column<string>(type: "TEXT", nullable: true),
                    QrTokenExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LetzteErfassung = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LetzterLoginOrt = table.Column<string>(type: "TEXT", nullable: true),
                    Abteilung = table.Column<string>(type: "TEXT", nullable: true),
                    Telefon = table.Column<string>(type: "TEXT", nullable: true),
                    Standort = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Zeitmodelle_ZeitmodellId",
                        column: x => x.ZeitmodellId,
                        principalTable: "Zeitmodelle",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Absences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Typ = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Kommentar = table.Column<string>(type: "TEXT", nullable: true),
                    ErstelltAm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    UserId1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Absences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Absences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Absences_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ErfasstVonUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ErfasstFürUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ZeitmodellUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Start = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Ende = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Beschreibung = table.Column<string>(type: "TEXT", nullable: true),
                    ProjektName = table.Column<string>(type: "TEXT", nullable: true),
                    IstMontage = table.Column<bool>(type: "INTEGER", nullable: false),
                    ErfasstAm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Pausenzeit = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeEntries_Users_ErfasstFürUserId",
                        column: x => x.ErfasstFürUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeEntries_Users_ErfasstVonUserId",
                        column: x => x.ErfasstVonUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TimeEntries_Users_ZeitmodellUserId",
                        column: x => x.ZeitmodellUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "AutoSendReports", "AutoSyncHolidays", "BackgroundImageUrl", "DownloadOnly", "EnableReminder", "ErrorTypesToCheck", "FontSize", "HolidayRegionCode", "HolidaySource", "Language", "QrTokenSingleUse", "RemindAfterDays", "ReportWithSignatureField", "SendOnDay", "TokenMaxLifetime" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), false, true, null, false, false, "NurKommen,KeinePauseEnde", 1, "DE-BY", "API", "de", true, 3, false, 1, new TimeSpan(1, 0, 0, 0, 0) });

            migrationBuilder.InsertData(
                table: "TokenConfigs",
                columns: new[] { "Id", "LoginType", "SystemSettingsId", "ValidityDuration" },
                values: new object[,]
                {
                    { new Guid("22222222-1111-1111-1111-111111111111"), 1, new Guid("11111111-1111-1111-1111-111111111111"), new TimeSpan(0, 1, 0, 0, 0) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 4, new Guid("11111111-1111-1111-1111-111111111111"), new TimeSpan(0, 0, 10, 0, 0) },
                    { new Guid("33333333-aaaa-4444-8888-111111111111"), 1, new Guid("11111111-1111-1111-1111-111111111111"), new TimeSpan(0, 1, 0, 0, 0) },
                    { new Guid("44444444-bbbb-5555-9999-222222222222"), 4, new Guid("11111111-1111-1111-1111-111111111111"), new TimeSpan(0, 0, 10, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Absences_UserId",
                table: "Absences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Absences_UserId1",
                table: "Absences",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_LoginAudits_UserId",
                table: "LoginAudits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ErfasstFürUserId",
                table: "TimeEntries",
                column: "ErfasstFürUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ErfasstVonUserId",
                table: "TimeEntries",
                column: "ErfasstVonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_UserId",
                table: "TimeEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_ZeitmodellUserId",
                table: "TimeEntries",
                column: "ZeitmodellUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TokenConfigs_SystemSettingsId",
                table: "TokenConfigs",
                column: "SystemSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ZeitmodellId",
                table: "Users",
                column: "ZeitmodellId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Absences");

            migrationBuilder.DropTable(
                name: "Feiertage");

            migrationBuilder.DropTable(
                name: "LoginAudits");

            migrationBuilder.DropTable(
                name: "TimeEntries");

            migrationBuilder.DropTable(
                name: "TokenConfigs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Zeitmodelle");
        }
    }
}
