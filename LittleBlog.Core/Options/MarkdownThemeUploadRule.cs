using LittleBlog.Core.Common;
using LittleBlog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Options
{
    public class MarkdownThemeUploadRule : IUploadRule
    {
        public MarkdownThemeUploadRule(IHostEnvironment host)
        {
            UploadFolder = Path.Combine(host.ContentRootPath, string.Join(Path.DirectorySeparatorChar, folders));
        }

        public static string Key => UploadTypes.MarkdownTheme;

        public List<string> folders { get; } = new List<string>() { "wwwroot", "upload", "md_themes" };

        public string RequestPath { get; } = "/themes/markdown";

        public string UploadFolder { get; }

        public List<string> FileExtensions { get; } = new List<string>()
        {
            ".css"
        };

        public long MaxFileSize { get; } = 1024 * 1024 * 10;

        public string GetFileUrl(IUrlHelper urlHelper, UploadInfo uploadInfo)
        {
            var url = urlHelper.Content($"~{RequestPath}/{uploadInfo.Group}/{uploadInfo.FileName}");

            return url;
        }

        public string GetUploadFileFolder(UploadInfo uploadInfo)
        {
            var folder  = Path.Combine(UploadFolder, uploadInfo.Group);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        public void Validate(IFormFile file, UploadInfo uploadInfo)
        {
            if (file.Length > MaxFileSize)
            {
                throw new UploadException($"文件大小不超过{MaxFileSize / 1024}Kb");
            }

            if (!FileExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new UploadException($"文件后缀名仅限于[{string.Join(",", FileExtensions)}]");
            }

            if (string.IsNullOrEmpty(uploadInfo.Group))
            {
                uploadInfo.Group = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrEmpty(uploadInfo.FileName))
            {
                uploadInfo.FileName = $"TMP_{Guid.NewGuid().ToString("N")}{Path.GetExtension(file.FileName)}";
            }
        }
    }
}
