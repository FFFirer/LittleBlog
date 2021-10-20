using LittleBlog.Core;
using LittleBlog.Core.Common;
using LittleBlog.Core.Extensions;
using LittleBlog.Core.Models;
using LittleBlog.Core.Options;
using LittleBlog.Core.Repositories;
using LittleBlog.Core.Services;
using LittleBlog.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LittleBlog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            var logProvider = new NLogLoggerProvider();
            Logger = logProvider.CreateLogger(nameof(Startup));

            HostEnvironment = hostEnvironment;
        }

        private static string DefaultCorsPolicyName = "default";
        public IConfiguration Configuration { get; }
        public ILogger Logger { get; }
        public IHostEnvironment HostEnvironment { get; }

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
                    services.AddDbContext<LittleBlogContext>(options => options.UseNpgsql(connectionString, x=> {
                        x.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                    }));
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

            // 文件上传
            var imageRule = new ImageUploadRule(HostEnvironment);

            // Razor Pages
            services.AddRazorPages(options =>
            {
                options.Conventions.AllowAnonymousToPage("/");
                options.Conventions.AllowAnonymousToFolder("/");
                options.Conventions.AllowAnonymousToFolder(imageRule.RequestPath);
                options.Conventions.AllowAnonymousToPage("/Error/");
            }).AddRazorRuntimeCompilation();

            // 依赖注入
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddSingleton<IAuthorizationHandler, ArticleAuthorizationHandler>();
            services.AddScoped<IFileService, FileService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ISettingRepo, SettingRepo>();

            // Swagger OpenApi
            services.AddSwaggerDocument((settings) =>
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
                    config.WithOrigins(allowedOrigins.ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            // AutoMapper
            services.AddAutoMapper(typeof(ArticleProfile));

            // 配置上传文件验证
            services.AddOptions<UploadOption>(UploadTypes.Image)
                .Configure((o) =>
                {
                    o.Rule = imageRule;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
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

            if (Directory.Exists(adminDirectory))
            {
                Directory.CreateDirectory(adminDirectory);
                FileServerOptions options = new FileServerOptions()
                {
                    FileProvider = new PhysicalFileProvider(adminDirectory),
                    RequestPath = "/admin"
                };
                options.DefaultFilesOptions.DefaultFileNames.Clear();
                options.DefaultFilesOptions.DefaultFileNames.Add("index.html");
                app.UseFileServer(options);
            }
            else
            {
                Logger.LogError($"缺少目录[{adminDirectory}]，后台管理功能将无法使用。");
            }
            app.UseFileServerForUpload(UploadOptions.Types.Select(a=>a.Value).ToList());

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
