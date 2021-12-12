using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LittleBlog.Core.Migrations
{
    public partial class Update_Article : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarkdownContent",
                table: "Articles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseMarkDown",
                table: "Articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkdownContent",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "UseMarkDown",
                table: "Articles");
        }
    }
}
