using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uofi_itp_directory_data.Migrations
{
    /// <inheritdoc />
    public partial class PhaseTwoJp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowEmployeeToEdit",
                table: "JobProfileTags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2024, 5, 3, 15, 27, 31, 347, DateTimeKind.Local).AddTicks(443));

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2024, 5, 3, 15, 27, 31, 347, DateTimeKind.Local).AddTicks(318));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowEmployeeToEdit",
                table: "JobProfileTags");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2024, 5, 3, 13, 20, 2, 900, DateTimeKind.Local).AddTicks(9383));

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2024, 5, 3, 13, 20, 2, 900, DateTimeKind.Local).AddTicks(9265));
        }
    }
}
