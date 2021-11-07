using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LittleBlog.DbMigrations.DevPgsql
{
    public partial class ChangeBackLoggedDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 手动添加要转换的类型
            migrationBuilder.AlterColumn<DateTime>(
                name: "Logged",
                table: "Logs",
                nullable: false,
                type: "timestamp without time zone USING \"Logged\"::timestamp without time zone",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Logged",
                table: "Logs",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
