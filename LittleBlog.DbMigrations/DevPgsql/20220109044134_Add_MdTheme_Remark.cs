using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittleBlog.DbMigrations.DevPgsql
{
    public partial class Add_MdTheme_Remark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "MarkdownThemes",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark",
                table: "MarkdownThemes");
        }
    }
}
