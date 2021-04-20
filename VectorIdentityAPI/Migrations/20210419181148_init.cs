using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VectorIdentityAPI.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_data",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password_salt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "projectset",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    owner_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectset", x => x.id);
                    table.ForeignKey(
                        name: "projectset_owner_id_fkey",
                        column: x => x.owner_id,
                        principalTable: "user_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "projectdata",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    file_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    file_data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    date_created = table.Column<DateTime>(type: "date", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    original = table.Column<bool>(type: "bit", nullable: false),
                    score_identity = table.Column<double>(type: "float", nullable: false),
                    score_correctness = table.Column<double>(type: "float", nullable: false),
                    date_uploaded = table.Column<DateTime>(type: "date", nullable: false),
                    date_updated = table.Column<DateTime>(type: "date", nullable: false),
                    owner_id = table.Column<int>(type: "int", nullable: true),
                    projectset_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projectdata", x => x.id);
                    table.ForeignKey(
                        name: "projectdata_owner_id_fkey",
                        column: x => x.owner_id,
                        principalTable: "user_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "projectdata_projectset_id_fkey",
                        column: x => x.projectset_id,
                        principalTable: "projectset",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Arc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Handle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    X = table.Column<double>(type: "float", nullable: false),
                    Y = table.Column<double>(type: "float", nullable: false),
                    Z = table.Column<double>(type: "float", nullable: false),
                    Radius = table.Column<double>(type: "float", nullable: false),
                    AngleStart = table.Column<double>(type: "float", nullable: false),
                    AngleEnd = table.Column<double>(type: "float", nullable: false),
                    DX = table.Column<double>(type: "float", nullable: false),
                    DY = table.Column<double>(type: "float", nullable: false),
                    DZ = table.Column<double>(type: "float", nullable: false),
                    ProjectDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arc", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arc_projectdata_ProjectDataId",
                        column: x => x.ProjectDataId,
                        principalTable: "projectdata",
                        principalColumn: "id",
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
                        name: "FK_ComparisonData_projectdata_ProjectAId",
                        column: x => x.ProjectAId,
                        principalTable: "projectdata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComparisonData_projectdata_ProjectBId",
                        column: x => x.ProjectBId,
                        principalTable: "projectdata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Line",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Handle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Layer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    X1 = table.Column<double>(type: "float", nullable: false),
                    Y1 = table.Column<double>(type: "float", nullable: false),
                    Z1 = table.Column<double>(type: "float", nullable: false),
                    X2 = table.Column<double>(type: "float", nullable: false),
                    Y2 = table.Column<double>(type: "float", nullable: false),
                    Z2 = table.Column<double>(type: "float", nullable: false),
                    Magnitude = table.Column<double>(type: "float", nullable: false),
                    DX = table.Column<double>(type: "float", nullable: false),
                    DY = table.Column<double>(type: "float", nullable: false),
                    DZ = table.Column<double>(type: "float", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Line", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Line_projectdata_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projectdata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineAId = table.Column<int>(type: "int", nullable: true),
                    LineBId = table.Column<int>(type: "int", nullable: true),
                    ArcAId = table.Column<int>(type: "int", nullable: true),
                    ArcBId = table.Column<int>(type: "int", nullable: true),
                    ComparisonDataId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_Arc_ArcAId",
                        column: x => x.ArcAId,
                        principalTable: "Arc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Arc_ArcBId",
                        column: x => x.ArcBId,
                        principalTable: "Arc",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_ComparisonData_ComparisonDataId",
                        column: x => x.ComparisonDataId,
                        principalTable: "ComparisonData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Line_LineAId",
                        column: x => x.LineAId,
                        principalTable: "Line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Line_LineBId",
                        column: x => x.LineBId,
                        principalTable: "Line",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_Line_ProjectId",
                table: "Line",
                column: "ProjectId");

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
                name: "IX_projectdata_owner_id",
                table: "projectdata",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_projectdata_projectset_id",
                table: "projectdata",
                column: "projectset_id");

            migrationBuilder.CreateIndex(
                name: "IX_projectset_owner_id",
                table: "projectset",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "user_data_email_key",
                table: "user_data",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_data_username_key",
                table: "user_data",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Arc");

            migrationBuilder.DropTable(
                name: "ComparisonData");

            migrationBuilder.DropTable(
                name: "Line");

            migrationBuilder.DropTable(
                name: "projectdata");

            migrationBuilder.DropTable(
                name: "projectset");

            migrationBuilder.DropTable(
                name: "user_data");
        }
    }
}
