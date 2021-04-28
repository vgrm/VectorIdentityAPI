using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class UpdateArcDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arc_projectdata_ProjectDataId",
                table: "Arc");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Arc_ArcAId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Arc_ArcBId",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Arc",
                table: "Arc");

            migrationBuilder.DropIndex(
                name: "IX_Arc_ProjectDataId",
                table: "Arc");

            migrationBuilder.DropColumn(
                name: "ProjectDataId",
                table: "Arc");

            migrationBuilder.RenameTable(
                name: "Arc",
                newName: "arc");

            migrationBuilder.RenameColumn(
                name: "Z",
                table: "arc",
                newName: "z");

            migrationBuilder.RenameColumn(
                name: "Y",
                table: "arc",
                newName: "y");

            migrationBuilder.RenameColumn(
                name: "X",
                table: "arc",
                newName: "x");

            migrationBuilder.RenameColumn(
                name: "Radius",
                table: "arc",
                newName: "radius");

            migrationBuilder.RenameColumn(
                name: "Handle",
                table: "arc",
                newName: "handle");

            migrationBuilder.RenameColumn(
                name: "DZ",
                table: "arc",
                newName: "dz");

            migrationBuilder.RenameColumn(
                name: "DY",
                table: "arc",
                newName: "dy");

            migrationBuilder.RenameColumn(
                name: "DX",
                table: "arc",
                newName: "dx");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "arc",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "AngleStart",
                table: "arc",
                newName: "angle_start");

            migrationBuilder.RenameColumn(
                name: "AngleEnd",
                table: "arc",
                newName: "angle_end");

            migrationBuilder.AddColumn<string>(
                name: "layer",
                table: "arc",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "project_id",
                table: "arc",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_arc",
                table: "arc",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_arc_project_id",
                table: "arc",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "arc_projectdata_id_fkey",
                table: "arc",
                column: "project_id",
                principalTable: "projectdata",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "arc_projectdata_id_fkey",
                table: "arc");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_arc_ArcAId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_arc_ArcBId",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_arc",
                table: "arc");

            migrationBuilder.DropIndex(
                name: "IX_arc_project_id",
                table: "arc");

            migrationBuilder.DropColumn(
                name: "layer",
                table: "arc");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "arc");

            migrationBuilder.RenameTable(
                name: "arc",
                newName: "Arc");

            migrationBuilder.RenameColumn(
                name: "z",
                table: "Arc",
                newName: "Z");

            migrationBuilder.RenameColumn(
                name: "y",
                table: "Arc",
                newName: "Y");

            migrationBuilder.RenameColumn(
                name: "x",
                table: "Arc",
                newName: "X");

            migrationBuilder.RenameColumn(
                name: "radius",
                table: "Arc",
                newName: "Radius");

            migrationBuilder.RenameColumn(
                name: "handle",
                table: "Arc",
                newName: "Handle");

            migrationBuilder.RenameColumn(
                name: "dz",
                table: "Arc",
                newName: "DZ");

            migrationBuilder.RenameColumn(
                name: "dy",
                table: "Arc",
                newName: "DY");

            migrationBuilder.RenameColumn(
                name: "dx",
                table: "Arc",
                newName: "DX");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Arc",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "angle_start",
                table: "Arc",
                newName: "AngleStart");

            migrationBuilder.RenameColumn(
                name: "angle_end",
                table: "Arc",
                newName: "AngleEnd");

            migrationBuilder.AddColumn<int>(
                name: "ProjectDataId",
                table: "Arc",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Arc",
                table: "Arc",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Arc_ProjectDataId",
                table: "Arc",
                column: "ProjectDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arc_projectdata_ProjectDataId",
                table: "Arc",
                column: "ProjectDataId",
                principalTable: "projectdata",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Arc_ArcAId",
                table: "Match",
                column: "ArcAId",
                principalTable: "Arc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Arc_ArcBId",
                table: "Match",
                column: "ArcBId",
                principalTable: "Arc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
