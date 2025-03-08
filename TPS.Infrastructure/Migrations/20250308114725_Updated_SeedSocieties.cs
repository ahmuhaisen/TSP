using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SeedSocieties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Societies",
                columns: new[] { "Id", "AdvisorId", "CreationDate", "Description", "LogoId", "Name", "ThemeColor" },
                values: new object[,]
                {
                    { new Guid("2a077a71-972d-4b6f-80e5-f2103dafd753"), new Guid("75445a2d-ec45-4e52-8acc-08dd5e35e650"), new DateOnly(2024, 1, 1), "A society for Robotics.", "", "Waves JU", "#FF0000" },
                    { new Guid("6f5fbae1-d89a-4dbd-96cd-7e7929cde69a"), new Guid("e67bb4a6-0eb9-498a-8acd-08dd5e35e650"), new DateOnly(2019, 1, 1), "A society for Computer Science students.", "", "IEEE CS JU", "#FF0000" },
                    { new Guid("7981a758-5274-4349-ba71-6b8e689e9ea9"), new Guid("b6530abe-bfe5-4212-8acb-08dd5e35e650"), new DateOnly(2017, 1, 1), "A society for Problem Solving.", "", "ACM JU Student Chapter", "#FF0000" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Societies",
                keyColumn: "Id",
                keyValue: new Guid("2a077a71-972d-4b6f-80e5-f2103dafd753"));

            migrationBuilder.DeleteData(
                table: "Societies",
                keyColumn: "Id",
                keyValue: new Guid("6f5fbae1-d89a-4dbd-96cd-7e7929cde69a"));

            migrationBuilder.DeleteData(
                table: "Societies",
                keyColumn: "Id",
                keyValue: new Guid("7981a758-5274-4349-ba71-6b8e689e9ea9"));
        }
    }
}
