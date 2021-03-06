﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace vector_control_system_api.Migrations
{
    public partial class UpdateMatches3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_match_arc_arc_test_id",
                table: "match");

            migrationBuilder.DropForeignKey(
                name: "FK_match_arc_ArcId",
                table: "match");

            migrationBuilder.DropIndex(
                name: "IX_match_ArcId",
                table: "match");

            migrationBuilder.DropColumn(
                name: "ArcId",
                table: "match");

            migrationBuilder.AddForeignKey(
                name: "match_arc_test_id",
                table: "match",
                column: "arc_test_id",
                principalTable: "arc",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "match_arc_test_id",
                table: "match");

            migrationBuilder.AddColumn<int>(
                name: "ArcId",
                table: "match",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_match_ArcId",
                table: "match",
                column: "ArcId");

            migrationBuilder.AddForeignKey(
                name: "FK_match_arc_arc_test_id",
                table: "match",
                column: "arc_test_id",
                principalTable: "arc",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_match_arc_ArcId",
                table: "match",
                column: "ArcId",
                principalTable: "arc",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
