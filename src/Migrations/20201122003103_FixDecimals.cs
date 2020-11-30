using Microsoft.EntityFrameworkCore.Migrations;

namespace LaFlorida.Migrations
{
    public partial class FixDecimals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Sales",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Costs",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Costs",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Sales",
                type: "decimal(18, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Total",
                table: "Costs",
                type: "decimal(18, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Costs",
                type: "decimal(18, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);
        }
    }
}
