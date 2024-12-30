using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addMoreValuesInRankTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 8, "Secretary / Typist" },
                    { 9, "Computer Lab Supervisor" },
                    { 10, "Computer Engineer" },
                    { 11, "Monitoring and Controlling" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Ranks",
                keyColumn: "Id",
                keyValue: 11);
        }
    }
}
