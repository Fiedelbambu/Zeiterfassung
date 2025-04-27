using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IST.Zeiterfassung.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FinalUserFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ZeitmodellId",
                table: "Users",
                column: "ZeitmodellId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Zeitmodelle_ZeitmodellId",
                table: "Users",
                column: "ZeitmodellId",
                principalTable: "Zeitmodelle",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Zeitmodelle_ZeitmodellId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ZeitmodellId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");
        }
    }
}
