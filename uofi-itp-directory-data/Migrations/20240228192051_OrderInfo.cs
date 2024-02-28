using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uofi_itp_directory_data.Migrations
{
    /// <inheritdoc />
    public partial class OrderInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalOrder",
                table: "OfficeSettings");

            migrationBuilder.DropColumn(
                name: "InternalOrder",
                table: "AreaSettings");

            migrationBuilder.AddColumn<int>(
                name: "InternalOrder",
                table: "Offices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternalOrder",
                table: "Areas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalOrder",
                table: "Offices");

            migrationBuilder.DropColumn(
                name: "InternalOrder",
                table: "Areas");

            migrationBuilder.AddColumn<int>(
                name: "InternalOrder",
                table: "OfficeSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InternalOrder",
                table: "AreaSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
