using Microsoft.EntityFrameworkCore.Migrations;

namespace VectorIdentityAPI.Migrations
{
    public partial class AddProjectSetStateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "state_id",
                table: "projectset",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "project_set_state",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_set_state", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_projectset_state_id",
                table: "projectset",
                column: "state_id");

            migrationBuilder.AddForeignKey(
                name: "project_set_state_id_fkey",
                table: "projectset",
                column: "state_id",
                principalTable: "project_set_state",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "project_set_state_id_fkey",
                table: "projectset");

            migrationBuilder.DropTable(
                name: "project_set_state");

            migrationBuilder.DropIndex(
                name: "IX_projectset_state_id",
                table: "projectset");

            migrationBuilder.DropColumn(
                name: "state_id",
                table: "projectset");
        }
    }
}
