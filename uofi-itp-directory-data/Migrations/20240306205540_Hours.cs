using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uofi_itp_directory_data.Migrations
{
    /// <inheritdoc />
    public partial class Hours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NetId",
                table: "SecurityEntries",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "OfficeHourText",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeHourText",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficeHourText",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "EmployeeHourText",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "SecurityEntries",
                newName: "NetId");
        }
    }
}
