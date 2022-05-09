using LittleBlog.Core.Common;
using LittleBlog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LittleBlog.Core.Options
{
    public class ImageUploadRule : IUploadRule
    {
        public ImageUploadRule(IHostEnvironment host)
        {
            UploadFolder = Path.Combine(host.ContentRootPath, string.Join(Path.DirectorySeparatorChar, folders));
        }

        public static string Key => UploadTypes.Image;

        public List<string> folders => new List<string>() { "wwwroot", "upload", "images" };

        public List<string> FileExtensions => new List<string>() {
            ".jpg",
            ".jpeg"
        };

        public long MaxFileSize => 1024 * 1024 * 10;

        public string RequestPath => "/resources/image";

        public string UploadFolder { get; }

        public string GetUploadFileFolder(UploadInfo uploadInfo)
        {
            var folder = Path.Combine(UploadFolder, uploadInfo.Group);

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
                throw new UploadException($"图片文件大小不超过{MaxFileSize / 1024}Kb");
            }

            if (!FileExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new UploadException($"图片文件后缀名仅限于[{string.Join(",", FileExtensions)}]");
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

        public string GetFileUrl(IUrlHelper urlHelper, UploadInfo uploadInfo)
        {
            var url = urlHelper.Content($"~{RequestPath}/{uploadInfo.Group}/{uploadInfo.FileName}");

            return url;
        }
    }
}
