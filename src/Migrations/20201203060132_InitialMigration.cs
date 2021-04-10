using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace LaFlorida.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 85, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 128, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 85, nullable: false),
                    UserName = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 128, nullable: true),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 128, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobId);
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 85, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(maxLength: 85, nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(maxLength: 85, nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(maxLength: 85, nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 85, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 85, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 85, nullable: false),
                    RoleId = table.Column<string>(maxLength: 85, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 85, nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 85, nullable: false),
                    Name = table.Column<string>(maxLength: 85, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    LotId = table.Column<int>(nullable: false),
                    IsComplete = table.Column<bool>(nullable: false, defaultValueSql: "0"),
                    IsRent = table.Column<bool>(nullable: false, defaultValueSql: "0")
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
                    Quantity = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
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
                    Quantity = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Total = table.Column<decimal>(nullable: true),
                    Buyer = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    InvoiceId = table.Column<string>(nullable: true),
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
                    CycleId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Withdraw", x => x.WithdrawId);
                    table.ForeignKey(
                        name: "FK_Withdraw_ApplicationUser",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Withdraw_Cycle",
                        column: x => x.CycleId,
                        principalTable: "Cycles",
                        principalColumn: "CycleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0b75fd57-659d-4ec5-9864-3be915e49a5c", "4802a8e2-27c8-47c8-8191-95c4d6cc2af2", "Admin", "ADMIN" },
                    { "cd074895-bf44-40d6-b511-61848932ad64", "986ea838-1075-42df-810b-ebc718dc5d76", "Manager", "MANAGER" },
                    { "e0219917-9bb9-4433-8f1b-123246352e99", "bbfe7e82-fb9e-4373-bed8-3479590445e1", "Client", "CLIENT" },
                    { "f23e58df-3f4a-49c6-9b28-a9043cfe0557", "16cbc98b-21e1-4916-a9ae-046170094786", "Machinist", "MACHINIST" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1030b74b-96fd-46e0-959c-4d71f99b74c7", 0, "d019e076-083c-4483-89d3-b9405e625f47", "m.canizares@outlook.com", true, "Miguel", "Canizares", false, null, "M.CANIZARES@OUTLOOK.COM", "M.CANIZARES@OUTLOOK.COM", "AQAAAAEAACcQAAAAEDcbeMwG8bsOZOyaxR4nV4z6Eu+VVWuLPPZMJkowGv90YJjYv/4WxCV8KzoCKwWDpA==", "0400157444", true, "c26b1c00-dcd8-4295-9b98-a8be698b08c7", false, "m.canizares@outlook.com" },
                    { "0e548f75-adda-4431-9462-f113ab1adc37", 0, "b4e2009b-05f2-4ba5-b380-5f5dfb6758e2", "jorlcm@hotmail.com", true, "Jorge", "Canizares", false, null, "JORLCM@HOTMAIL.COM", "JORLCM@HOTMAIL.COM", "AQAAAAEAACcQAAAAEFgrlvFSTFVkAf1WmDLIT7yWQubRnUrnfYdCAf0RpomCkxw8CYq2mcKjR2swoWKVmA==", "0998229186", true, "7cc4260f-0a02-4a70-bf80-4d67f4245791", false, "jorlcm@hotmail.com" }
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "JobId", "Name" },
                values: new object[,]
                {
                    { 1, "Maquinaria" },
                    { 2, "Arriendo" },
                    { 3, "Semilla" },
                    { 4, "Fertilizante" },
                    { 5, "Mano de Obra" },
                    { 6, "Quimicos" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "1030b74b-96fd-46e0-959c-4d71f99b74c7", "0b75fd57-659d-4ec5-9864-3be915e49a5c" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "0e548f75-adda-4431-9462-f113ab1adc37", "0b75fd57-659d-4ec5-9864-3be915e49a5c" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

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
                name: "IX_Sales_CycleId",
                table: "Sales",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Withdraws_ApplicationUserId",
                table: "Withdraws",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Withdraws_CycleId",
                table: "Withdraws",
                column: "CycleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Costs");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Withdraws");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Cycles");

            migrationBuilder.DropTable(
                name: "Crops");

            migrationBuilder.DropTable(
                name: "Lots");
        }
    }
}
