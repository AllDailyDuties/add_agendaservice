using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllDailyDuties_AgendaService.Migrations
{
    public partial class NewMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskUser_UserId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskUser",
                table: "TaskUser");

            migrationBuilder.RenameTable(
                name: "TaskUser",
                newName: "TaskUsers");

            migrationBuilder.AddColumn<string>(
                name: "Activity",
                table: "Tasks",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskUsers",
                table: "TaskUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskUsers_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "TaskUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskUsers_UserId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskUsers",
                table: "TaskUsers");

            migrationBuilder.DropColumn(
                name: "Activity",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "TaskUsers",
                newName: "TaskUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskUser",
                table: "TaskUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskUser_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "TaskUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
