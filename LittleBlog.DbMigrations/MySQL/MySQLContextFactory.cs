using LittleBlog.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LittleBlog.DbMigrations.MySQL
{
    public class MySQLContextFactory : IDesignTimeDbContextFactory<MySQLContext>
    {
        public const string MySQLConnectionString = "Server=localhost;Port=3306;Database=db_web;Uid=root;Pwd=123456;";
        public MySQLContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LittleBlogContext>()
                                .UseMySql(MySQLConnectionString, ServerVersion.Parse("8.0"));
            return new MySQLContext(builder.Options);
        }
    }
}