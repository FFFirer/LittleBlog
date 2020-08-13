﻿using System;
using LittleBlog.Web.Areas.Identity.Data;
using LittleBlog.Web.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
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
                services.AddDbContext<LittleBlogContext>(options =>
                    options.UseMySql(
                        context.Configuration.GetConnectionString("MysqlConnection")));

                services.AddDefaultIdentity<LittleBlogIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<LittleBlogContext>();
            });
        }
    }
}