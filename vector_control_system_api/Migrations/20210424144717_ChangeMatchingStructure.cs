using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class ChangeMatchingStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "match_arc_original_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "match_arc_test_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "match_line_original_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "match_line_test_id",
                table: "match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_match",
                table: "match");

            migrationBuilder.DropIndex(
                name: "IX_match_arc_original_id",
                table: "match");

            migrationBuilder.DropIndex(
                name: "IX_match_arc_test_id",
                table: "match");

            migrationBuilder.DropIndex(
                name: "IX_match_line_original_id",
                table: "match");

            migrationBuilder.DropIndex(
                name: "IX_match_line_test_id",
                table: "match");

            migrationBuilder.RenameTable(
                name: "match",
                newName: "Match");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Match",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Match",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "info",
                table: "Match",
                newName: "Info");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Match",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "line_test_id",
                table: "Match",
                newName: "LineTestId");

            migrationBuilder.RenameColumn(
                name: "line_original_id",
                table: "Match",
                newName: "LineOriginalId");

            migrationBuilder.RenameColumn(
                name: "arc_test_id",
                table: "Match",
                newName: "ArcTestId");

            migrationBuilder.RenameColumn(
                name: "arc_original_id",
                table: "Match",
                newName: "ArcOriginalId");

            migrationBuilder.AddColumn<int>(
                name: "original_project_id",
                table: "projectdata",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "correct",
                table: "line",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "correct",
                table: "arc",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "original_project_id",
                table: "projectdata");

            migrationBuilder.DropColumn(
                name: "correct",
                table: "line");

            migrationBuilder.DropColumn(
                name: "correct",
                table: "arc");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "match");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "match",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "match",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Info",
                table: "match",
                newName: "info");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "match",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LineTestId",
                table: "match",
                newName: "line_test_id");

            migrationBuilder.RenameColumn(
                name: "LineOriginalId",
                table: "match",
                newName: "line_original_id");

            migrationBuilder.RenameColumn(
                name: "ArcTestId",
                table: "match",
                newName: "arc_test_id");

            migrationBuilder.RenameColumn(
                name: "ArcOriginalId",
                table: "match",
                newName: "arc_original_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_match",
                table: "match",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_match_arc_original_id",
                table: "match",
                column: "arc_original_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_arc_test_id",
                table: "match",
                column: "arc_test_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_line_original_id",
                table: "match",
                column: "line_original_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_line_test_id",
                table: "match",
                column: "line_test_id");

            migrationBuilder.AddForeignKey(
                name: "match_arc_original_id",
                table: "match",
                column: "arc_original_id",
                principalTable: "arc",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "match_arc_test_id",
                table: "match",
                column: "arc_test_id",
                principalTable: "arc",
                principalColumn: "id");

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
    }
}
