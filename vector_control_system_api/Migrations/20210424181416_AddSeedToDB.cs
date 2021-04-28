using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class AddSeedToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "project_set_state",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { -1, "Open" },
                    { -2, "Closed" },
                    { -3, "Private" }
                });

            migrationBuilder.InsertData(
                table: "state",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { -1, "New" },
                    { -2, "Processing" },
                    { -3, "Analyzing" },
                    { -4, "Analyzed" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "project_set_state",
                keyColumn: "id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "project_set_state",
                keyColumn: "id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "project_set_state",
                keyColumn: "id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "state",
                keyColumn: "id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "state",
                keyColumn: "id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "state",
                keyColumn: "id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "state",
                keyColumn: "id",
                keyValue: -1);
        }
    }
}
