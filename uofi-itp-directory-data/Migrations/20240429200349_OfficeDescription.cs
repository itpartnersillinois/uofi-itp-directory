using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uofi_itp_directory_data.Migrations
{
    /// <inheritdoc />
    public partial class OfficeDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2024, 4, 29, 15, 3, 48, 715, DateTimeKind.Local).AddTicks(4980));

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2024, 4, 29, 15, 3, 48, 715, DateTimeKind.Local).AddTicks(4838));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Offices");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2024, 4, 9, 16, 14, 53, 302, DateTimeKind.Local).AddTicks(3504));

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2024, 4, 9, 16, 14, 53, 302, DateTimeKind.Local).AddTicks(3346));
        }
    }
}
