using Microsoft.EntityFrameworkCore.Migrations;

namespace VectorIdentityAPI.Migrations
{
    public partial class RemoveProjectsFromMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "match_originalprojectdata_id_fkey",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "match_testprojectdata_id_fkey",
                table: "match");

            migrationBuilder.DropIndex(
                name: "IX_match_original_project_id",
                table: "match");

            migrationBuilder.DropIndex(
                name: "IX_match_test_project_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "original_project_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "test_project_id",
                table: "match");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "original_project_id",
                table: "match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "test_project_id",
                table: "match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_match_original_project_id",
                table: "match",
                column: "original_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_test_project_id",
                table: "match",
                column: "test_project_id");

            migrationBuilder.AddForeignKey(
                name: "match_originalprojectdata_id_fkey",
                table: "match",
                column: "original_project_id",
                principalTable: "projectdata",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "match_testprojectdata_id_fkey",
                table: "match",
                column: "test_project_id",
                principalTable: "projectdata",
                principalColumn: "id");
        }
    }
}
