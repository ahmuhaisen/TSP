using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacultyMembers_Positions_PositionId",
                table: "FacultyMembers");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "FacultyMembers",
                newName: "RankId");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyMembers_PositionId",
                table: "FacultyMembers",
                newName: "IX_FacultyMembers_RankId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyMembers_Positions_RankId",
                table: "FacultyMembers",
                column: "RankId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacultyMembers_Positions_RankId",
                table: "FacultyMembers");

            migrationBuilder.RenameColumn(
                name: "RankId",
                table: "FacultyMembers",
                newName: "PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_FacultyMembers_RankId",
                table: "FacultyMembers",
                newName: "IX_FacultyMembers_PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacultyMembers_Positions_PositionId",
                table: "FacultyMembers",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
