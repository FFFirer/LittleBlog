using LittleBlog.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LittleBlog.DbMigrations
{
    public class DevPgsqlContextFactory : IDesignTimeDbContextFactory<DevPgsqlContext>
    {
        public const string ConnectionString = "Host=127.0.0.1;Database=littleblog;Username=postgres;Password=123456";

        public DevPgsqlContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LittleBlogContext>();

            builder.UseNpgsql(ConnectionString);

            return new DevPgsqlContext(builder.Options);
        }
    }
}
