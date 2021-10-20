using LittleBlog.Core.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;

namespace LittleBlog.Core.Extensions
{
    public static class UploadFileServerExtension
    {
        public static IApplicationBuilder UseFileServerForUpload(this IApplicationBuilder app, List<string> uploadTypes)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var namedOptions = scope.ServiceProvider.GetService<IOptionsSnapshot<UploadOption>>();
                var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

                if (namedOptions == null) return app;

                foreach (var uploadType in uploadTypes)
                {
                    var uploadOption = namedOptions.Get(uploadType);
                    if (uploadOption == null) continue;

                    var rule = uploadOption.Rule;
                    if (rule == null) continue;


                    var ruleDirectory = Path.Combine(env.ContentRootPath, string.Join(Path.DirectorySeparatorChar, rule.folders));

                    if (!Directory.Exists(ruleDirectory))
                    {
                        Directory.CreateDirectory(ruleDirectory);
                    }

                    FileServerOptions options = new FileServerOptions()
                    {
                        FileProvider = new PhysicalFileProvider(ruleDirectory),
                        RequestPath = rule.RequestPath,
                    };
                    options.DefaultFilesOptions.DefaultFileNames.Clear();
                    app.UseFileServer(options);
                }
            }
            
            return app;
        }
     }
}
