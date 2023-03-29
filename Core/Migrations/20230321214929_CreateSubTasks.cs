using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class CreateSubTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MySubTaskId",
                table: "MyFiles",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MySubTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DateStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DateEnd = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MyTaskStatusId = table.Column<int>(type: "integer", nullable: true),
                    MyTaskLevelImportanceId = table.Column<int>(type: "integer", nullable: true),
                    MyTaskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MySubTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MySubTasks_LevelImportances_MyTaskLevelImportanceId",
                        column: x => x.MyTaskLevelImportanceId,
                        principalTable: "LevelImportances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MySubTasks_MyTaskStatuses_MyTaskStatusId",
                        column: x => x.MyTaskStatusId,
                        principalTable: "MyTaskStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MySubTasks_MyTasks_MyTaskId",
                        column: x => x.MyTaskId,
                        principalTable: "MyTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyFiles_MySubTaskId",
                table: "MyFiles",
                column: "MySubTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_MySubTasks_MyTaskId",
                table: "MySubTasks",
                column: "MyTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_MySubTasks_MyTaskLevelImportanceId",
                table: "MySubTasks",
                column: "MyTaskLevelImportanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MySubTasks_MyTaskStatusId",
                table: "MySubTasks",
                column: "MyTaskStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyFiles_MySubTasks_MySubTaskId",
                table: "MyFiles",
                column: "MySubTaskId",
                principalTable: "MySubTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyFiles_MySubTasks_MySubTaskId",
                table: "MyFiles");

            migrationBuilder.DropTable(
                name: "MySubTasks");

            migrationBuilder.DropIndex(
                name: "IX_MyFiles_MySubTaskId",
                table: "MyFiles");

            migrationBuilder.DropColumn(
                name: "MySubTaskId",
                table: "MyFiles");
        }
    }
}
