using Microsoft.EntityFrameworkCore.Migrations;

namespace LaFlorida.Migrations
{
    public partial class QuantityToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Sales",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "Sales",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Costs",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Sales",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "Sales",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "Costs",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

           
        }
    }
}
