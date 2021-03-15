using Microsoft.EntityFrameworkCore.Migrations;

namespace VectorIdentityAPI.Migrations
{
    public partial class UpdateFileData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Identity",
                table: "FileData",
                newName: "IdentityScore");

            migrationBuilder.RenameColumn(
                name: "Correctness",
                table: "FileData",
                newName: "CorrectnessScore");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdentityScore",
                table: "FileData",
                newName: "Identity");

            migrationBuilder.RenameColumn(
                name: "CorrectnessScore",
                table: "FileData",
                newName: "Correctness");
        }
    }
}
