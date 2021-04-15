using Microsoft.EntityFrameworkCore.Migrations;

namespace VectorIdentityAPI.Migrations
{
    public partial class updateLineArc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Direction",
                table: "Line",
                newName: "DZ");

            migrationBuilder.AddColumn<double>(
                name: "DX",
                table: "Line",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DY",
                table: "Line",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Handle",
                table: "Line",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DX",
                table: "Arc",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DY",
                table: "Arc",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DZ",
                table: "Arc",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Handle",
                table: "Arc",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DX",
                table: "Line");

            migrationBuilder.DropColumn(
                name: "DY",
                table: "Line");

            migrationBuilder.DropColumn(
                name: "Handle",
                table: "Line");

            migrationBuilder.DropColumn(
                name: "DX",
                table: "Arc");

            migrationBuilder.DropColumn(
                name: "DY",
                table: "Arc");

            migrationBuilder.DropColumn(
                name: "DZ",
                table: "Arc");

            migrationBuilder.DropColumn(
                name: "Handle",
                table: "Arc");

            migrationBuilder.RenameColumn(
                name: "DZ",
                table: "Line",
                newName: "Direction");
        }
    }
}
