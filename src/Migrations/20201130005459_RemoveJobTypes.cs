using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LaFlorida.Migrations
{
    public partial class RemoveJobTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_JobType",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "JobTypes");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobTypeId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobTypeId",
                table: "Jobs");           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobTypeId",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JobTypes",
                columns: table => new
                {
                    JobTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobType", x => x.JobTypeId);
                });          

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobTypeId",
                table: "Jobs",
                column: "JobTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_JobType",
                table: "Jobs",
                column: "JobTypeId",
                principalTable: "JobTypes",
                principalColumn: "JobTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
