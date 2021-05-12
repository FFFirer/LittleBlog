using System;
using LittleBlog.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace LittleBlog.DbMigrations.PostgreSQL
{
    public class PostgreSQLContext : LittleBlogContext
    {
        public PostgreSQLContext(DbContextOptions<LittleBlogContext> options) : base(options)
        {

        }
    }
}