using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class UpdateUserStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "user_data_email_key",
                table: "user_data");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user_data",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user_data",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "user_data_email_key",
                table: "user_data",
                column: "email",
                unique: true);
        }
    }
}
