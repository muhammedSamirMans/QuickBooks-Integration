using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karage.Infrastructure.Migrations
{
    public partial class altertable_QBAuth_addIsExpired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                table: "QBAuths",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpired",
                table: "QBAuths");
        }
    }
}
