using Microsoft.EntityFrameworkCore.Design;
using System;
using LittleBlog.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace LittleBlog.DbMigrations.PostgreSQL
{
    public class PostgreSQLContextFactory :IDesignTimeDbContextFactory<PostgreSQLContext>
    {
        public const string PostgreSQLConnectionString = "";

        public PostgreSQLContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<LittleBlogContext>()
                                .UseNpgsql(PostgreSQLConnectionString);
            return new PostgreSQLContext(builder.Options);
        }
    }
}