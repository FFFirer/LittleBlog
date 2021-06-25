using System;
using System.Collections.Generic;
using System.Text;
using LittleBlog.Web.Data;
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
