using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace uofi_itp_directory_data.Migrations
{
    /// <inheritdoc />
    public partial class InstructionChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructionsCv",
                table: "AreaSettings");

            migrationBuilder.DropColumn(
                name: "InstructionsEmployeeTags",
                table: "AreaSettings");

            migrationBuilder.DropColumn(
                name: "InstructionsHeadshot",
                table: "AreaSettings");

            migrationBuilder.RenameColumn(
                name: "InstructionsRefresh",
                table: "AreaSettings",
                newName: "InstructionsOffice");

            migrationBuilder.RenameColumn(
                name: "InstructionsProfile",
                table: "AreaSettings",
                newName: "InstructionsEmployeeSignature");

            migrationBuilder.RenameColumn(
                name: "InstructionsOfficeTags",
                table: "AreaSettings",
                newName: "InstructionsEmployeeHeadshot");

            migrationBuilder.RenameColumn(
                name: "InstructionsMain",
                table: "AreaSettings",
                newName: "InstructionsEmployeeCv");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InstructionsOffice",
                table: "AreaSettings",
                newName: "InstructionsRefresh");

            migrationBuilder.RenameColumn(
                name: "InstructionsEmployeeSignature",
                table: "AreaSettings",
                newName: "InstructionsProfile");

            migrationBuilder.RenameColumn(
                name: "InstructionsEmployeeHeadshot",
                table: "AreaSettings",
                newName: "InstructionsOfficeTags");

            migrationBuilder.RenameColumn(
                name: "InstructionsEmployeeCv",
                table: "AreaSettings",
                newName: "InstructionsMain");

            migrationBuilder.AddColumn<string>(
                name: "InstructionsCv",
                table: "AreaSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstructionsEmployeeTags",
                table: "AreaSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstructionsHeadshot",
                table: "AreaSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
