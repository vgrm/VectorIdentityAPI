using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class UpdateMatches4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_match_line_line_original_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_line_line_test_id",
                table: "match");

            migrationBuilder.AddForeignKey(
                name: "match_line_original_id",
                table: "match",
                column: "line_original_id",
                principalTable: "line",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "match_line_test_id",
                table: "match",
                column: "line_test_id",
                principalTable: "line",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "match_line_original_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "match_line_test_id",
                table: "match");

            migrationBuilder.AddForeignKey(
                name: "FK_match_line_line_original_id",
                table: "match",
                column: "line_original_id",
                principalTable: "line",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_match_line_line_test_id",
                table: "match",
                column: "line_test_id",
                principalTable: "line",
                principalColumn: "id");
        }
    }
}
