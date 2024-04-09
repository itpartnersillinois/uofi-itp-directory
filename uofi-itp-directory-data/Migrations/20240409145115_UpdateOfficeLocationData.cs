using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uofi_itp_directory_data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOfficeLocationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OfficeInformation",
                table: "Employees",
                newName: "ZipCode");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UsePrimaryOfficeAsAddress",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2024, 4, 9, 9, 51, 14, 379, DateTimeKind.Local).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2024, 4, 9, 9, 51, 14, 379, DateTimeKind.Local).AddTicks(9660));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UsePrimaryOfficeAsAddress",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Employees",
                newName: "OfficeInformation");

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -2,
                column: "LastUpdated",
                value: new DateTime(2024, 4, 2, 10, 45, 9, 282, DateTimeKind.Local).AddTicks(3294));

            migrationBuilder.UpdateData(
                table: "SecurityEntries",
                keyColumn: "Id",
                keyValue: -1,
                column: "LastUpdated",
                value: new DateTime(2024, 4, 2, 10, 45, 9, 282, DateTimeKind.Local).AddTicks(2418));
        }
    }
}
