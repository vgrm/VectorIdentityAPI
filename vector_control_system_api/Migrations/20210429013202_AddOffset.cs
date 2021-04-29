using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class AddOffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "offset_x",
                table: "projectdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "offset_y",
                table: "projectdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "offset_z",
                table: "projectdata",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "offset_x",
                table: "projectdata");

            migrationBuilder.DropColumn(
                name: "offset_y",
                table: "projectdata");

            migrationBuilder.DropColumn(
                name: "offset_z",
                table: "projectdata");
        }
    }
}
