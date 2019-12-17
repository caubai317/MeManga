using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeManga.Migrations
{
    public partial class CreateDatabasev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TranslatorId",
                table: "Book",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_TranslatorId",
                table: "Book",
                column: "TranslatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_User_TranslatorId",
                table: "Book",
                column: "TranslatorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_User_TranslatorId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_TranslatorId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "TranslatorId",
                table: "Book");
        }
    }
}
