using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class AddLayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "project_data_state_id_fkey",
                table: "projectdata");

            migrationBuilder.DropForeignKey(
                name: "project_set_state_id_fkey",
                table: "projectset");

            migrationBuilder.DropTable(
                name: "ComparisonData");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_state",
                table: "state");

            migrationBuilder.DropPrimaryKey(
                name: "PK_project_set_state",
                table: "project_set_state");

            migrationBuilder.RenameTable(
                name: "state",
                newName: "projectdata_state");

            migrationBuilder.RenameTable(
                name: "project_set_state",
                newName: "projectset_state");

            migrationBuilder.AddPrimaryKey(
                name: "PK_projectdata_state",
                table: "projectdata_state",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_projectset_state",
                table: "projectset_state",
                column: "id");

            migrationBuilder.CreateTable(
                name: "layer",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    linetype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    project_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_layer", x => x.id);
                    table.ForeignKey(
                        name: "layer_projectdata_id_fkey",
                        column: x => x.project_id,
                        principalTable: "projectdata",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_layer_project_id",
                table: "layer",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "projectdata_state_id_fkey",
                table: "projectdata",
                column: "state_id",
                principalTable: "projectdata_state",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "projectset_state_id_fkey",
                table: "projectset",
                column: "state_id",
                principalTable: "projectset_state",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "projectdata_state_id_fkey",
                table: "projectdata");

            migrationBuilder.DropForeignKey(
                name: "projectset_state_id_fkey",
                table: "projectset");

            migrationBuilder.DropTable(
                name: "layer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_projectset_state",
                table: "projectset_state");

            migrationBuilder.DropPrimaryKey(
                name: "PK_projectdata_state",
                table: "projectdata_state");

            migrationBuilder.RenameTable(
                name: "projectset_state",
                newName: "project_set_state");

            migrationBuilder.RenameTable(
                name: "projectdata_state",
                newName: "state");

            migrationBuilder.AddPrimaryKey(
                name: "PK_project_set_state",
                table: "project_set_state",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_state",
                table: "state",
                column: "id");

            migrationBuilder.CreateTable(
                name: "ComparisonData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComparisonData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArcOriginalId = table.Column<int>(type: "int", nullable: false),
                    ArcTestId = table.Column<int>(type: "int", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineOriginalId = table.Column<int>(type: "int", nullable: false),
                    LineTestId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "project_data_state_id_fkey",
                table: "projectdata",
                column: "state_id",
                principalTable: "state",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "project_set_state_id_fkey",
                table: "projectset",
                column: "state_id",
                principalTable: "project_set_state",
                principalColumn: "id");
        }
    }
}
