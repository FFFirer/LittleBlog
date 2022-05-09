using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittleBlog.DbMigrations.DevPgsql
{
    public partial class Add_MarkdownTheme_Content : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "MarkdownThemes",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "MarkdownThemes");
        }
    }
}
