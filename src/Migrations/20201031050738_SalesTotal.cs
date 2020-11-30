using Microsoft.EntityFrameworkCore.Migrations;

namespace LaFlorida.Migrations
{
    public partial class SalesTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Sales",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Sales");
        }
    }
}
