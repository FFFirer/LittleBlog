using Microsoft.EntityFrameworkCore.Migrations;

namespace LittleBlog.DbMigrations.DevPgsql
{
    public partial class ChangeLogLevelDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LogLevel",
                table: "Logs",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LogLevel",
                table: "Logs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
