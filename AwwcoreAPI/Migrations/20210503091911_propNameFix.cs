using Microsoft.EntityFrameworkCore.Migrations;

namespace AwwcoreAPI.Migrations
{
    public partial class propNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyProperty",
                table: "PhotoLinks",
                newName: "Link");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "PhotoLinks",
                newName: "MyProperty");
        }
    }
}
