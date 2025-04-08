using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClassUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRequest_Societies_SocietyId",
                table: "MembershipRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRequest_Students_StudentId",
                table: "MembershipRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipRequest",
                table: "MembershipRequest");

            migrationBuilder.RenameTable(
                name: "MembershipRequest",
                newName: "MembershipsRequests");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipRequest_StudentId",
                table: "MembershipsRequests",
                newName: "IX_MembershipsRequests_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipRequest_SocietyId",
                table: "MembershipsRequests",
                newName: "IX_MembershipsRequests_SocietyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DecisionDate",
                table: "EventsApproval",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipsRequests",
                table: "MembershipsRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipsRequests_Societies_SocietyId",
                table: "MembershipsRequests",
                column: "SocietyId",
                principalTable: "Societies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipsRequests_Students_StudentId",
                table: "MembershipsRequests",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipsRequests_Societies_SocietyId",
                table: "MembershipsRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MembershipsRequests_Students_StudentId",
                table: "MembershipsRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MembershipsRequests",
                table: "MembershipsRequests");

            migrationBuilder.RenameTable(
                name: "MembershipsRequests",
                newName: "MembershipRequest");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipsRequests_StudentId",
                table: "MembershipRequest",
                newName: "IX_MembershipRequest_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_MembershipsRequests_SocietyId",
                table: "MembershipRequest",
                newName: "IX_MembershipRequest_SocietyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DecisionDate",
                table: "EventsApproval",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MembershipRequest",
                table: "MembershipRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRequest_Societies_SocietyId",
                table: "MembershipRequest",
                column: "SocietyId",
                principalTable: "Societies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRequest_Students_StudentId",
                table: "MembershipRequest",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }
    }
}
