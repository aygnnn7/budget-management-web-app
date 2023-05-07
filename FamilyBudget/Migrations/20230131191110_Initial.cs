using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FamilyBudget.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sex = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepeatDay = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "Email", "Name", "Password", "RegisteredTime", "Sex" },
                values: new object[] { 1, "A@1", "Aygyun", "Fitness124", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5057), true });

            migrationBuilder.InsertData(
                table: "transactions",
                columns: new[] { "Id", "Category", "CreatedTime", "Description", "ModifiedTime", "OwnerId", "RepeatDay", "Value" },
                values: new object[,]
                {
                    { 1, "Salary", new DateTime(2022, 12, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5122), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5125), 1, 0, 821m },
                    { 2, "General", new DateTime(2022, 12, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5136), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5138), 1, 0, -163m },
                    { 3, "MonthlyRepetition", new DateTime(2022, 12, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5142), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5144), 1, 0, -55m },
                    { 4, "Salary", new DateTime(2022, 11, 30, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5148), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5150), 1, 0, 524m },
                    { 5, "General", new DateTime(2022, 11, 30, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5153), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5155), 1, 0, -459m },
                    { 6, "MonthlyRepetition", new DateTime(2022, 11, 30, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5159), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5161), 1, 0, -55m },
                    { 7, "Salary", new DateTime(2022, 10, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5163), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5165), 1, 0, 696m },
                    { 8, "General", new DateTime(2022, 10, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5168), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5170), 1, 0, -668m },
                    { 9, "MonthlyRepetition", new DateTime(2022, 10, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5172), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5174), 1, 0, -55m },
                    { 10, "Salary", new DateTime(2022, 9, 30, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5177), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5179), 1, 0, 736m },
                    { 11, "General", new DateTime(2022, 9, 30, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5182), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5184), 1, 0, -886m },
                    { 12, "MonthlyRepetition", new DateTime(2022, 9, 30, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5186), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5188), 1, 0, -55m },
                    { 13, "Salary", new DateTime(2022, 8, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5191), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5193), 1, 0, 449m },
                    { 14, "General", new DateTime(2022, 8, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5195), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5197), 1, 0, -303m },
                    { 15, "MonthlyRepetition", new DateTime(2022, 8, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5200), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5202), 1, 0, -55m },
                    { 16, "Salary", new DateTime(2022, 7, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5204), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5206), 1, 0, 512m },
                    { 17, "General", new DateTime(2022, 7, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5208), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5210), 1, 0, -303m },
                    { 18, "MonthlyRepetition", new DateTime(2022, 7, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5213), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5215), 1, 0, -55m },
                    { 19, "Health", new DateTime(2023, 5, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5218), "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since", new DateTime(2023, 1, 31, 21, 11, 9, 971, DateTimeKind.Local).AddTicks(5220), 1, 14, -55m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_transactions_OwnerId",
                table: "transactions",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
