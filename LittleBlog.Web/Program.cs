using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Areas.Identity.Data;
using LittleBlog.Web.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LittleBlog.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<Program>>();
                
                try
                {
                    var context = services.GetRequiredService<LittleBlogContext>();
                    var env = services.GetRequiredService<IWebHostEnvironment>();
                    if (env.IsProduction())
                    {
                        context.Database.Migrate();
                        logger.LogInformation("Database migrated");
                    }
                    else
                    {
                        if (context.Database.EnsureCreated())
                        {
                            logger.LogInformation("Database ensure created");

                        }
                    }
                    
                    var config = host.Services.GetRequiredService<IConfiguration>();

                    SeedData.Initialze(services, config["adminPwd"], config["adminName"]).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((WebHostBuilderContext, config) =>
                {
                    config.AddEnvironmentVariables(prefix: "LittleBlog_");
                })
                .UseStartup<Startup>();
    }
}
