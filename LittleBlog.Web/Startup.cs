using LittleBlog.Web.Data;
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
            var connection = Configuration.GetConnectionString("MysqlConnection");
            
            services.AddDbContext<LittleBlogContext>(options => options.UseMySql(connection));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddLogging(configLogger =>
            {
                configLogger.AddNLog("NLog.config");
            });
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddSingleton<IAuthorizationHandler, ArticleAuthorizationHandler>();

            services.AddSwaggerDocument((settings)=> 
            {
                settings.Version = "v1.0.0";
                settings.Title = "LittleBlog Web API";
            });

            services.AddCors(setup =>
            {
                setup.AddPolicy(DefaultCorsPolicyName, config =>
                {
                    config.WithOrigins("http://127.0.0.1:3000", "http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
                });
            });
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
            app.UseOpenApi();
            app.UseSwaggerUi3();
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
                endpoints.MapControllerRoute(
                    
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
