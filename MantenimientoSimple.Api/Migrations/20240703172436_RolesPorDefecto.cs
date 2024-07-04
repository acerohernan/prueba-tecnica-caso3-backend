using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MantenimientoSimple.Api.Migrations
{
    public partial class RolesPorDefecto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "77b5c7e3-89f2-4122-9589-25c6ec643a1c", "3", "Viewer", "VIEWER" },
                    { "b4d7bdf1-1c55-49b6-b2f6-b04d1986b9c7", "1", "Admin", "ADMIN" },
                    { "cce4bc44-11f4-4143-8f0b-c7f7e5595138", "2", "Editor", "EDITOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "77b5c7e3-89f2-4122-9589-25c6ec643a1c");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "b4d7bdf1-1c55-49b6-b2f6-b04d1986b9c7");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "cce4bc44-11f4-4143-8f0b-c7f7e5595138");
        }
    }
}
