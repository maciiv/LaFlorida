using Microsoft.EntityFrameworkCore.Migrations;

namespace LaFlorida.Migrations
{
    public partial class ProductionReady : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b75fd57-659d-4ec5-9864-3be915e49a5c",
                column: "ConcurrencyStamp",
                value: "4153e16e-c7f3-4f15-94a1-a9adab24e86c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd074895-bf44-40d6-b511-61848932ad64",
                column: "ConcurrencyStamp",
                value: "1f191e0d-f154-430d-b753-dd6b0725eaa0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0219917-9bb9-4433-8f1b-123246352e99",
                column: "ConcurrencyStamp",
                value: "415b2b8f-940f-4f1f-84a1-cb55ebb90189");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f23e58df-3f4a-49c6-9b28-a9043cfe0557",
                column: "ConcurrencyStamp",
                value: "5604e4f1-63ff-4cf9-8277-f7b76de782f6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e548f75-adda-4431-9462-f113ab1adc37",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "LockoutEnabled", "PasswordHash", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled" },
                values: new object[] { "6425ebb9-bc08-4ec1-bc0d-156732194758", true, false, "AQAAAAEAACcQAAAAELKLzPpmIdfwLh/V9I5vdEiOk3PDXgBxtaFLyWOa8RFiINhr8xy57W88li3JdMxMVw==", true, "f501e345-3a61-4b4c-9ef9-7a7bbb912660", false });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1030b74b-96fd-46e0-959c-4d71f99b74c7",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "LockoutEnabled", "PasswordHash", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled" },
                values: new object[] { "b22555dc-19f2-4d4d-88e9-9f7d22ef91a1", true, false, "AQAAAAEAACcQAAAAEBnlopdCdiGOibJiwoWk4wzb9jD2/kajMRUiDiZ9XqVNXgaTNIUZvHs7RyBVtSJ9dg==", true, "a7b5fedd-1851-47fe-b142-cd14f523dad3", false });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "JobId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "JobId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "JobId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "JobId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "JobId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Jobs",
                keyColumn: "JobId",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b75fd57-659d-4ec5-9864-3be915e49a5c",
                column: "ConcurrencyStamp",
                value: "9f2d68af-2099-4162-b7af-0280285989bf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd074895-bf44-40d6-b511-61848932ad64",
                column: "ConcurrencyStamp",
                value: "4146dfee-3f96-44ef-af7c-a958a1edef12");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0219917-9bb9-4433-8f1b-123246352e99",
                column: "ConcurrencyStamp",
                value: "3aa1d042-46fd-4f43-90b7-0441fadc5bc5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f23e58df-3f4a-49c6-9b28-a9043cfe0557",
                column: "ConcurrencyStamp",
                value: "2afb220d-be84-4834-84af-a18c35c95d85");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0e548f75-adda-4431-9462-f113ab1adc37",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "LockoutEnabled", "PasswordHash", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled" },
                values: new object[] { "a99ed97d-aef3-4347-a3df-9da58aa0093b", (short)1, (short)0, "AQAAAAEAACcQAAAAELVZAG1GH97LquwDnAoBj3zjY3z9lIgudypAWAJTivYAAoNjRuZgHHNwBoId+fTT4g==", (short)1, "30e48fb7-2734-4b0f-9518-6238ffcd93d3", (short)0 });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1030b74b-96fd-46e0-959c-4d71f99b74c7",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "LockoutEnabled", "PasswordHash", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled" },
                values: new object[] { "46130b9b-d7e6-4e27-88ef-046c01eeb9e6", (short)1, (short)0, "AQAAAAEAACcQAAAAEAiLAUWtT/b+KpBaS80a7Djw+fORRHWA/XoEJA9UUaWjZ+8KrQjzeh8PfFKJPjr73w==", (short)1, "c1607468-0acf-4c48-9755-87b655d0087f", (short)0 });
        }
    }
}
