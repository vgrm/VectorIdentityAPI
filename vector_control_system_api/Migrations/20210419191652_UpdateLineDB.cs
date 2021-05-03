using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class UpdateLineDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Line_projectdata_ProjectId",
                table: "Line");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Line_LineAId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Line_LineBId",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Line",
                table: "Line");

            migrationBuilder.RenameTable(
                name: "Line",
                newName: "line");

            migrationBuilder.RenameColumn(
                name: "Z2",
                table: "line",
                newName: "z2");

            migrationBuilder.RenameColumn(
                name: "Z1",
                table: "line",
                newName: "z1");

            migrationBuilder.RenameColumn(
                name: "Y2",
                table: "line",
                newName: "y2");

            migrationBuilder.RenameColumn(
                name: "Y1",
                table: "line",
                newName: "y1");

            migrationBuilder.RenameColumn(
                name: "X2",
                table: "line",
                newName: "x2");

            migrationBuilder.RenameColumn(
                name: "X1",
                table: "line",
                newName: "x1");

            migrationBuilder.RenameColumn(
                name: "Magnitude",
                table: "line",
                newName: "magnitude");

            migrationBuilder.RenameColumn(
                name: "Layer",
                table: "line",
                newName: "layer");

            migrationBuilder.RenameColumn(
                name: "Handle",
                table: "line",
                newName: "handle");

            migrationBuilder.RenameColumn(
                name: "DZ",
                table: "line",
                newName: "dz");

            migrationBuilder.RenameColumn(
                name: "DY",
                table: "line",
                newName: "dy");

            migrationBuilder.RenameColumn(
                name: "DX",
                table: "line",
                newName: "dx");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "line",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "line",
                newName: "project_id");

            migrationBuilder.RenameIndex(
                name: "IX_Line_ProjectId",
                table: "line",
                newName: "IX_line_project_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_line",
                table: "line",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "line_projectdata_id_fkey",
                table: "line",
                column: "project_id",
                principalTable: "projectdata",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "line_projectdata_id_fkey",
                table: "line");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_line_LineAId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_line_LineBId",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_line",
                table: "line");

            migrationBuilder.RenameTable(
                name: "line",
                newName: "Line");

            migrationBuilder.RenameColumn(
                name: "z2",
                table: "Line",
                newName: "Z2");

            migrationBuilder.RenameColumn(
                name: "z1",
                table: "Line",
                newName: "Z1");

            migrationBuilder.RenameColumn(
                name: "y2",
                table: "Line",
                newName: "Y2");

            migrationBuilder.RenameColumn(
                name: "y1",
                table: "Line",
                newName: "Y1");

            migrationBuilder.RenameColumn(
                name: "x2",
                table: "Line",
                newName: "X2");

            migrationBuilder.RenameColumn(
                name: "x1",
                table: "Line",
                newName: "X1");

            migrationBuilder.RenameColumn(
                name: "magnitude",
                table: "Line",
                newName: "Magnitude");

            migrationBuilder.RenameColumn(
                name: "layer",
                table: "Line",
                newName: "Layer");

            migrationBuilder.RenameColumn(
                name: "handle",
                table: "Line",
                newName: "Handle");

            migrationBuilder.RenameColumn(
                name: "dz",
                table: "Line",
                newName: "DZ");

            migrationBuilder.RenameColumn(
                name: "dy",
                table: "Line",
                newName: "DY");

            migrationBuilder.RenameColumn(
                name: "dx",
                table: "Line",
                newName: "DX");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Line",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "project_id",
                table: "Line",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_line_project_id",
                table: "Line",
                newName: "IX_Line_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Line",
                table: "Line",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Line_projectdata_ProjectId",
                table: "Line",
                column: "ProjectId",
                principalTable: "projectdata",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Line_LineAId",
                table: "Match",
                column: "LineAId",
                principalTable: "Line",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Line_LineBId",
                table: "Match",
                column: "LineBId",
                principalTable: "Line",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
