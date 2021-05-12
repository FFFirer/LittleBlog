using Microsoft.EntityFrameworkCore.Design;
using LittleBlog.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace LittleBlog.DbMigrations.MySQL
{
    public class MySQLContextFactory : IDesignTimeDbContextFactory<MySQLContext>
    {
        public const string MySQLConnectionString = "Server=localhost;Port=3306;Database=db_web;Uid=root;Pwd=123456;";
        public MySQLContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LittleBlogContext>()
                                .UseMySql(MySQLConnectionString);
            return new MySQLContext(builder.Options);
        }
    }
}