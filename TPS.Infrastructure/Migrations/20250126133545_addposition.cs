using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addposition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "SocietiesMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "SocietiesMembers");
        }
    }
}
