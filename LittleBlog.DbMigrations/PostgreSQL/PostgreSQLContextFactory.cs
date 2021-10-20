using LittleBlog.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LittleBlog.DbMigrations.PostgreSQL
{
    public class PostgreSQLContextFactory :IDesignTimeDbContextFactory<PostgreSQLContext>
    {
        public const string PostgreSQLConnectionString = "Host=127.0.0.1;Database=littleblog;Username=postgres;Password=123456;";

        public PostgreSQLContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LittleBlogContext>()
                                .UseNpgsql(PostgreSQLConnectionString);
            return new PostgreSQLContext(builder.Options);
        }
    }
}