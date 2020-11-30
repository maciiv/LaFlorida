using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LaFlorida.Migrations
{
    public partial class Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Crops",
                columns: table => new
                {
                    CropId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Lenght = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PF_Crop", x => x.CropId);
                });

            migrationBuilder.CreateTable(
                name: "JobTypes",
                columns: table => new
                {
                    JobTypeId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobType", x => x.JobTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    LotId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lot", x => x.LotId);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    JobTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Job_JobType",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "JobTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cycles",
                columns: table => new
                {
                    CycleId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    HarvestDate = table.Column<DateTime>(nullable: true),
                    CropId = table.Column<int>(nullable: false),
                    LotId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cycle", x => x.CycleId);
                    table.ForeignKey(
                        name: "FK_Cycle_Crop",
                        column: x => x.CropId,
                        principalTable: "Crops",
                        principalColumn: "CropId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cycle_Lot",
                        column: x => x.LotId,
                        principalTable: "Lots",
                        principalColumn: "LotId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Costs",
                columns: table => new
                {
                    CostId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    Details = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    CycleId = table.Column<int>(nullable: false),
                    JobId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cost", x => x.CostId);
                    table.ForeignKey(
                        name: "FK_Cost_ApplicationUser",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cost_Cycle",
                        column: x => x.CycleId,
                        principalTable: "Cycles",
                        principalColumn: "CycleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cost_Job",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: true),
                    Buyer = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    InvoiceId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    CycleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sale_Cycle",
                        column: x => x.CycleId,
                        principalTable: "Cycles",
                        principalColumn: "CycleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Withdraws",
                columns: table => new
                {
                    WithdrawId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Quantity = table.Column<decimal>(nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "now()"),
                    CostId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withdraw", x => x.WithdrawId);
                    table.ForeignKey(
                        name: "FK_Withdraw_Cost",
                        column: x => x.CostId,
                        principalTable: "Costs",
                        principalColumn: "CostId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Costs_ApplicationUserId",
                table: "Costs",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_CycleId",
                table: "Costs",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_JobId",
                table: "Costs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_CropId",
                table: "Cycles",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_LotId",
                table: "Cycles",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobTypeId",
                table: "Jobs",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CycleId",
                table: "Sales",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Withdraws_CostId",
                table: "Withdraws",
                column: "CostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Withdraws");

            migrationBuilder.DropTable(
                name: "Costs");

            migrationBuilder.DropTable(
                name: "Cycles");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropTable(
                name: "JobTypes");
        }
    }
}
