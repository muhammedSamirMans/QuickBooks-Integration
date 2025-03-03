using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karage.Infrastructure.Migrations
{
    public partial class altertable_QBAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "QBAuths",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "QBAuths",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QBAuths",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "QBAuths",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RealmId",
                table: "QBAuths",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "QBAuths");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "QBAuths");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QBAuths");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "QBAuths");

            migrationBuilder.DropColumn(
                name: "RealmId",
                table: "QBAuths");
        }
    }
}
