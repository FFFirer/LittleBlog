using Microsoft.EntityFrameworkCore.Migrations;

namespace LittleBlog.Web.Migrations
{
    public partial class UpdateArticle_AddAbstract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abstract",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abstract",
                table: "Articles");
        }
    }
}
