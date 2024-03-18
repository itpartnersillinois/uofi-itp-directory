using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uofi_itp_directory_data.Migrations
{
    /// <inheritdoc />
    public partial class dataload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UrlPeopleRefreshType",
                table: "AreaSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2024, 3, 18, 8, 13, 11, 715, DateTimeKind.Local).AddTicks(5955));

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2024, 3, 18, 8, 13, 11, 715, DateTimeKind.Local).AddTicks(5778));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlPeopleRefreshType",
                table: "AreaSettings");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
