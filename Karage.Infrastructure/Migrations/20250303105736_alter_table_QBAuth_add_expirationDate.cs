using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karage.Infrastructure.Migrations
{
    public partial class alter_table_QBAuth_add_expirationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "QBAuths",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "QBAuths");
        }
    }
}
