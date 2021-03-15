using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VectorIdentityAPI.Migrations
{
    public partial class UpdateDBStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_FileData_FileDataId",
                table: "Match");

            migrationBuilder.DropTable(
                name: "FileData");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropColumn(
                name: "Pointer",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Match");

            migrationBuilder.RenameColumn(
                name: "FileDataId",
                table: "Match",
                newName: "LineBId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_FileDataId",
                table: "Match",
                newName: "IX_Match_LineBId");

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

            migrationBuilder.CreateTable(
                name: "ProjectSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSet_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Original = table.Column<bool>(type: "bit", nullable: false),
                    IdentityScore = table.Column<double>(type: "float", nullable: false),
                    CorrectnessScore = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    ProjectSetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectData_ProjectSet_ProjectSetId",
                        column: x => x.ProjectSetId,
                        principalTable: "ProjectSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectData_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Arc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Z = table.Column<double>(type: "float", nullable: false),
                    Radius = table.Column<double>(type: "float", nullable: false),
                    AngleStart = table.Column<double>(type: "float", nullable: false),
                    AngleEnd = table.Column<double>(type: "float", nullable: false),
                    ProjectDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arc_ProjectData_ProjectDataId",
                        column: x => x.ProjectDataId,
                        principalTable: "ProjectData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComparisonData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectAId = table.Column<int>(type: "int", nullable: true),
                    ProjectBId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComparisonData_ProjectData_ProjectAId",
                        column: x => x.ProjectAId,
                        principalTable: "ProjectData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComparisonData_ProjectData_ProjectBId",
                        column: x => x.ProjectBId,
                        principalTable: "ProjectData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Line",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X1 = table.Column<double>(type: "float", nullable: false),
                    Y1 = table.Column<double>(type: "float", nullable: false),
                    Z1 = table.Column<double>(type: "float", nullable: false),
                    X2 = table.Column<double>(type: "float", nullable: false),
                    Y2 = table.Column<double>(type: "float", nullable: false),
                    Z2 = table.Column<double>(type: "float", nullable: false),
                    Magnitude = table.Column<double>(type: "float", nullable: false),
                    Direction = table.Column<double>(type: "float", nullable: false),
                    ProjectDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Line", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Line_ProjectData_ProjectDataId",
                        column: x => x.ProjectDataId,
                        principalTable: "ProjectData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Arc_ProjectDataId",
                table: "Arc",
                column: "ProjectDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonData_ProjectAId",
                table: "ComparisonData",
                column: "ProjectAId");

            migrationBuilder.CreateIndex(
                name: "IX_ComparisonData_ProjectBId",
                table: "ComparisonData",
                column: "ProjectBId");

            migrationBuilder.CreateIndex(
                name: "IX_Line_ProjectDataId",
                table: "Line",
                column: "ProjectDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectData_OwnerId",
                table: "ProjectData",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectData_ProjectSetId",
                table: "ProjectData",
                column: "ProjectSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSet_OwnerId",
                table: "ProjectSet",
                column: "OwnerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Match_ComparisonData_ComparisonDataId",
                table: "Match",
                column: "ComparisonDataId",
                principalTable: "ComparisonData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Arc_ArcAId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Arc_ArcBId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_ComparisonData_ComparisonDataId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Line_LineAId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Line_LineBId",
                table: "Match");

            migrationBuilder.DropTable(
                name: "Arc");

            migrationBuilder.DropTable(
                name: "ComparisonData");

            migrationBuilder.DropTable(
                name: "Line");

            migrationBuilder.DropTable(
                name: "ProjectData");

            migrationBuilder.DropTable(
                name: "ProjectSet");

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

            migrationBuilder.RenameColumn(
                name: "LineBId",
                table: "Match",
                newName: "FileDataId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_LineBId",
                table: "Match",
                newName: "IX_Match_FileDataId");

            migrationBuilder.AddColumn<int>(
                name: "Pointer",
                table: "Match",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Match",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorrectnessScore = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdentityScore = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Original = table.Column<bool>(type: "bit", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileData_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileData_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileData_OwnerId",
                table: "FileData",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_FileData_ProjectId",
                table: "FileData",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_OwnerId",
                table: "Project",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_FileData_FileDataId",
                table: "Match",
                column: "FileDataId",
                principalTable: "FileData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
