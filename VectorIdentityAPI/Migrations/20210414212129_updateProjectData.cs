using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VectorIdentityAPI.Migrations
{
    public partial class updateProjectData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ProjectData",
                newName: "DateUploaded");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "ProjectData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "ProjectData",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "ProjectData",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "ProjectData",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ProjectData");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ProjectData");

            migrationBuilder.DropColumn(
                name: "FileData",
                table: "ProjectData");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "ProjectData");

            migrationBuilder.RenameColumn(
                name: "DateUploaded",
                table: "ProjectData",
                newName: "Date");
        }
    }
}
