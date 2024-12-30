using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MinorChangesOnTheSeededDataOfSchools : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "School",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "School",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "School of Sport Science");

            migrationBuilder.UpdateData(
                table: "School",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "School of Political Science and International Studies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "School",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.UpdateData(
                table: "School",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "School of Educational Sciences");

            migrationBuilder.UpdateData(
                table: "School",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "School of International Studies");
        }
    }
}
