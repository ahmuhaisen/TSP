using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameRankTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacultyMembers_Positions_RankId",
                table: "FacultyMembers");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Ranks",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Dean" },
                    { 2, "Dean Assistant" },
                    { 3, "Department Chair" },
                    { 4, "Professor" },
                    { 5, "Associate Professor" },
                    { 6, "Assistant Professor" },
                    { 7, "Teacher" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyMembers_Ranks_RankId",
                table: "FacultyMembers",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacultyMembers_Ranks_RankId",
                table: "FacultyMembers");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Dean" },
                    { 2, "Dean Assistant" },
                    { 3, "Department Chair" },
                    { 4, "Professor" },
                    { 5, "Associate Professor" },
                    { 6, "Assistant Professor" },
                    { 7, "Teacher" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyMembers_Positions_RankId",
                table: "FacultyMembers",
                column: "RankId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
