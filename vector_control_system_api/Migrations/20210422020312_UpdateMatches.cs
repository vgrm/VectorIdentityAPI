using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class UpdateMatches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComparisonData_projectdata_ProjectAId",
                table: "ComparisonData");

            migrationBuilder.DropForeignKey(
                name: "FK_ComparisonData_projectdata_ProjectBId",
                table: "ComparisonData");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_arc_ArcAId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_arc_ArcBId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_ComparisonData_ComparisonDataId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_line_LineAId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_line_LineBId",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_ArcAId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_ArcBId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_ComparisonDataId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_LineAId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_LineBId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_ComparisonData_ProjectAId",
                table: "ComparisonData");

            migrationBuilder.DropIndex(
                name: "IX_ComparisonData_ProjectBId",
                table: "ComparisonData");

            migrationBuilder.DropColumn(
                name: "ArcAId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "ArcBId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "ComparisonDataId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "LineAId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "LineBId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "ProjectAId",
                table: "ComparisonData");

            migrationBuilder.DropColumn(
                name: "ProjectBId",
                table: "ComparisonData");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "match");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "match",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "arc_original_id",
                table: "match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "arc_test_id",
                table: "match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "info",
                table: "match",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "line_original_id",
                table: "match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "line_test_id",
                table: "match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "match",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "match",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_match_original_project_id",
                table: "match",
                column: "original_project_id");

            migrationBuilder.CreateIndex(
                name: "IX_match_test_project_id",
                table: "match",
                column: "test_project_id");

            migrationBuilder.AddForeignKey(
                name: "FK_match_arc_arc_original_id",
                table: "match",
                column: "arc_original_id",
                principalTable: "arc",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_match_arc_arc_test_id",
                table: "match",
                column: "arc_test_id",
                principalTable: "arc",
                principalColumn: "id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_match_arc_arc_original_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_arc_arc_test_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_line_line_original_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_line_line_test_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "match_originalprojectdata_id_fkey",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "match_testprojectdata_id_fkey",
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

            migrationBuilder.DropIndex(
                name: "IX_match_original_project_id",
                table: "match");

            migrationBuilder.DropIndex(
                name: "IX_match_test_project_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "arc_original_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "arc_test_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "info",
                table: "match");

            migrationBuilder.DropColumn(
                name: "line_original_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "line_test_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "name",
                table: "match");

            migrationBuilder.DropColumn(
                name: "original_project_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "test_project_id",
                table: "match");

            migrationBuilder.DropColumn(
                name: "type",
                table: "match");

            migrationBuilder.RenameTable(
                name: "match",
                newName: "Match");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Match",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "ArcAId",
                table: "Match",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArcBId",
                table: "Match",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ComparisonDataId",
                table: "Match",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LineAId",
                table: "Match",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LineBId",
                table: "Match",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectAId",
                table: "ComparisonData",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectBId",
                table: "ComparisonData",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_ArcAId",
                table: "Match",
                column: "ArcAId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_ArcBId",
                table: "Match",
                column: "ArcBId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_ComparisonDataId",
                table: "Match",
                column: "ComparisonDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_LineAId",
                table: "Match",
                column: "LineAId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_LineBId",
                table: "Match",
                column: "LineBId");

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonData_ProjectAId",
                table: "ComparisonData",
                column: "ProjectAId");

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonData_ProjectBId",
                table: "ComparisonData",
                column: "ProjectBId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComparisonData_projectdata_ProjectAId",
                table: "ComparisonData",
                column: "ProjectAId",
                principalTable: "projectdata",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComparisonData_projectdata_ProjectBId",
                table: "ComparisonData",
                column: "ProjectBId",
                principalTable: "projectdata",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_arc_ArcAId",
                table: "Match",
                column: "ArcAId",
                principalTable: "arc",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_arc_ArcBId",
                table: "Match",
                column: "ArcBId",
                principalTable: "arc",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_ComparisonData_ComparisonDataId",
                table: "Match",
                column: "ComparisonDataId",
                principalTable: "ComparisonData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_line_LineAId",
                table: "Match",
                column: "LineAId",
                principalTable: "line",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_line_LineBId",
                table: "Match",
                column: "LineBId",
                principalTable: "line",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
