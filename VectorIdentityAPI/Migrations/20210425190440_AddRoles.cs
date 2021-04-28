using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "role_id",
                table: "user_data",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_role", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "user_role",
                columns: new[] { "id", "name" },
                values: new object[] { -1, "Admin" });

            migrationBuilder.InsertData(
                table: "user_role",
                columns: new[] { "id", "name" },
                values: new object[] { -2, "User" });

            migrationBuilder.CreateIndex(
                name: "IX_user_data_role_id",
                table: "user_data",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "user_data_role_id_fkey",
                table: "user_data",
                column: "role_id",
                principalTable: "user_role",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "user_data_role_id_fkey",
                table: "user_data");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropIndex(
                name: "IX_user_data_role_id",
                table: "user_data");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "user_data");
        }
    }
}
