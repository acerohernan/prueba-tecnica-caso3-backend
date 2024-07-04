using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MantenimientoSimple.Api.Migrations
{
    public partial class AutenticacionImplementada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
