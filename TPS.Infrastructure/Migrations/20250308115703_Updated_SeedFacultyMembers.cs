using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SeedFacultyMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DepartmentId", "Email", "EmailConfirmed", "FirstName", "Gender", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImageId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("75445a2d-ec45-4e52-8acc-08dd5e35e650"), 0, "822ba1ac-825b-4df6-b6ec-e85b960ba0e2", 4, "ruba@ju.edu.jo", false, "Ruba", "Female", "E'baidat", false, null, null, null, "AQAAAAIAAYagAAAAELIhT49xTHOXI0y72eDfBfyjO2rS+RgWK4USYS3KSxFT8aC2IR8o0MsLuc7n/o+Mxg==", "0799999999", false, null, "76ZUI2EVMGSBUF7NWRTDDMRYSD3P2OZA", false, "ruba" },
                    { new Guid("b6530abe-bfe5-4212-8acb-08dd5e35e650"), 0, "2ec4788c-5f4c-421e-8abb-4a704a9d5421", 2, "asaf@ju.edu.jo", false, "Abdelbast", "Male", "A'asaf", false, null, null, null, "AQAAAAIAAYagAAAAELIhT49xTHOXI0y72eDfBfyjO2rS+RgWK4USYS3KSxFT8aC2IR8o0MsLuc7n/o+Mxg==", "0799999999", false, null, "76ZUI2EVMGSBUF7NWRTDDMRYSD3P2OZA", false, "asaf" },
                    { new Guid("e67bb4a6-0eb9-498a-8acd-08dd5e35e650"), 0, "162f4cc2-930b-4a65-985b-69d59f9bcea0", 2, "musa@ju.edu.jo", false, "Musa", "Male", "Al Akhras", false, null, null, null, "AQAAAAIAAYagAAAAELIhT49xTHOXI0y72eDfBfyjO2rS+RgWK4USYS3KSxFT8aC2IR8o0MsLuc7n/o+Mxg==", "0799999999", false, null, "76ZUI2EVMGSBUF7NWRTDDMRYSD3P2OZA", false, "musa" }
                });

            migrationBuilder.InsertData(
                table: "FacultyMembers",
                columns: new[] { "Id", "EmployeeNumber", "RankId" },
                values: new object[,]
                {
                    { new Guid("75445a2d-ec45-4e52-8acc-08dd5e35e650"), "AIS01", 4 },
                    { new Guid("b6530abe-bfe5-4212-8acb-08dd5e35e650"), "CIS01", 2 },
                    { new Guid("e67bb4a6-0eb9-498a-8acd-08dd5e35e650"), "CIS02", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FacultyMembers",
                keyColumn: "Id",
                keyValue: new Guid("75445a2d-ec45-4e52-8acc-08dd5e35e650"));

            migrationBuilder.DeleteData(
                table: "FacultyMembers",
                keyColumn: "Id",
                keyValue: new Guid("b6530abe-bfe5-4212-8acb-08dd5e35e650"));

            migrationBuilder.DeleteData(
                table: "FacultyMembers",
                keyColumn: "Id",
                keyValue: new Guid("e67bb4a6-0eb9-498a-8acd-08dd5e35e650"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("75445a2d-ec45-4e52-8acc-08dd5e35e650"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b6530abe-bfe5-4212-8acb-08dd5e35e650"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e67bb4a6-0eb9-498a-8acd-08dd5e35e650"));
        }
    }
}
