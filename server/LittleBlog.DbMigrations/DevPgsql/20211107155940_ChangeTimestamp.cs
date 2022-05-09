using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace LittleBlog.DbMigrations.DevPgsql
{
    public partial class ChangeTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Logged",
                table: "Logs",
                nullable: false,
                type: "timestamp USING \"Logged\"::timestamp",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogLevel",
                table: "Logs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<DateTime>(
                name: "Logged",
                table: "Logs",
                nullable: false,
                type: "timestamp without time zone USING \"Logged\"::timestamp without time zone",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogLevel",
                table: "Logs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
