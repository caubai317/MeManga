using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MeManga.Migrations
{
    public partial class createdatabasev3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "FilePath",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "FilePath",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedBy",
                table: "FilePath",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "FilePath",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RecordActive",
                table: "FilePath",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RecordDeleted",
                table: "FilePath",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RecordOrder",
                table: "FilePath",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "FilePath",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "FilePath",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "RecordActive",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "RecordDeleted",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "RecordOrder",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "FilePath");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "FilePath");
        }
    }
}
