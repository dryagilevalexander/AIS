using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class modLetters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LetterTypeId",
                table: "Letters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LetterTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LetterTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LetterTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Входящее" },
                    { 2, "Исходящее" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Letters_LetterTypeId",
                table: "Letters",
                column: "LetterTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Letters_LetterTypes_LetterTypeId",
                table: "Letters",
                column: "LetterTypeId",
                principalTable: "LetterTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Letters_LetterTypes_LetterTypeId",
                table: "Letters");

            migrationBuilder.DropTable(
                name: "LetterTypes");

            migrationBuilder.DropIndex(
                name: "IX_Letters_LetterTypeId",
                table: "Letters");

            migrationBuilder.DropColumn(
                name: "LetterTypeId",
                table: "Letters");
        }
    }
}
