using LittleBlog.Core;
using Microsoft.EntityFrameworkCore;

namespace LittleBlog.DbMigrations.MySQL
{
    public class MySQLContext : LittleBlogContext
    {
        public MySQLContext(DbContextOptions<LittleBlogContext> options) : base(options)
        {
            
        }
    }
}