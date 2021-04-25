using Microsoft.EntityFrameworkCore.Migrations;

namespace VectorIdentityAPI.Migrations
{
    public partial class AddProjectStateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "state_id",
                table: "projectdata",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "state",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_state", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_projectdata_state_id",
                table: "projectdata",
                column: "state_id");

            migrationBuilder.AddForeignKey(
                name: "project_data_state_id_fkey",
                table: "projectdata",
                column: "state_id",
                principalTable: "state",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "project_data_state_id_fkey",
                table: "projectdata");

            migrationBuilder.DropTable(
                name: "state");

            migrationBuilder.DropIndex(
                name: "IX_projectdata_state_id",
                table: "projectdata");

            migrationBuilder.DropColumn(
                name: "state_id",
                table: "projectdata");
        }
    }
}
