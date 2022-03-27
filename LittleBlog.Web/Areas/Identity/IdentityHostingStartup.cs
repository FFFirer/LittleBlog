using LittleBlog.Core;
using LittleBlog.Core.Models;
using LittleBlog.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(LittleBlog.Web.Areas.Identity.IdentityHostingStartup))]
namespace LittleBlog.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                var DbType = context.Configuration.GetValue<string>("DbType");

                string connectionString = context.Configuration.GetConnectionString("LittleBlog");
                switch (DbType)
                {
                    //case "Mysql":       
                    //    services.AddDbContext<LittleBlogContext>(options => 
                    //        options.UseMySql(connectionString));
                    //    break;
                    case "Pgsql":
                        services.AddDbContext<LittleBlogContext>(options => 
                            options.UseNpgsql(connectionString));
                        break;
                    default:
                        throw new BlogException($"没有配置数据库连接！当前配置[{DbType}]");
                }

                services.AddDefaultIdentity<LittleBlogIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<LittleBlogContext>();

                services.AddAuthorization(options =>
                {
                    // 后备身份验证策略，所有未显示认证的操作都需要登录操作。
                    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                });
            });
        }
    }
}