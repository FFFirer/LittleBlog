using LittleBlog.Core;
using Microsoft.EntityFrameworkCore;

namespace LittleBlog.DbMigrations
{
    public class DevPgsqlContext : LittleBlogContext
    {
        public DevPgsqlContext(DbContextOptions<LittleBlogContext> options):base(options)
        {

        }
    }
}
