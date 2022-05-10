using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittleBlog.DbMigrations.DevPgSql
{
    public partial class Add_UseOuterLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseOuterLink",
                table: "MarkdownThemes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseOuterLink",
                table: "MarkdownThemes");
        }
    }
}
