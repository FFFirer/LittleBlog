﻿using LittleBlog.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LittleBlog.Web.Services;
using LittleBlog.Web.Services.Interfaces;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using LittleBlog.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using LittleBlog.Web.Authorization;
using NSwag;
using NSwag.Generation;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace LittleBlog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static string DefaultCorsPolicyName = "default";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var db = Configuration.GetValue<string>("DbType");

            // 数据库连接
            string connectionString = Configuration.GetConnectionString("LittleBlog");
            switch (db)
            {
                case "Mysql":
                    services.AddDbContext<LittleBlogContext>(options => options.UseMySql(connectionString));
                    break;
                case "Pgsql":
                    services.AddDbContext<LittleBlogContext>(options => options.UseNpgsql(connectionString));
                    break;
                default:
                    throw new BlogException("没有配置数据库连接！");
            }

            // Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 日志
            services.AddLogging(configLogger =>
            {
                configLogger.AddNLog("NLog.config");
            });
            //services.AddControllersWithViews();

            // Razor Pages
            services.AddRazorPages(options => { 
                options.Conventions.AllowAnonymousToPage("/");
                options.Conventions.AllowAnonymousToFolder("/");
            }).AddRazorRuntimeCompilation();

            // 依赖注入
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddSingleton<IAuthorizationHandler, ArticleAuthorizationHandler>();

            // Swagger OpenApi
            services.AddSwaggerDocument((settings)=> 
            {
                settings.Version = "v1.0.0";
                settings.Title = "LittleBlog Web API";
            });

            // 跨域配置
            var allowedOrigins = new List<string>();
            Configuration.GetSection("AllowedOrigins").Bind(allowedOrigins);
            services.AddCors(setup =>
            {
                setup.AddPolicy(DefaultCorsPolicyName, config =>
                {
                    config.WithOrigins(allowedOrigins.ToArray()).AllowAnyMethod().AllowAnyHeader();
                });
            });

            // AutoMapper
            services.AddAutoMapper(typeof(Models.MapProfile.ArticleProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var adminDirectory = Path.Join(env.ContentRootPath, "wwwroot", "admin");
            FileServerOptions options = new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(adminDirectory),
                RequestPath = "/admin"
            };

            options.DefaultFilesOptions.DefaultFileNames.Clear();
            options.DefaultFilesOptions.DefaultFileNames.Add("index.html");

            app.UseFileServer(options);

            if (!env.IsProduction())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

            app.UseRouting();

            app.UseCors(DefaultCorsPolicyName);

            app.UseStatusCodePagesWithRedirects("/Error/{0}");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
            });
            
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers(); 
            });
        }
    }
}
