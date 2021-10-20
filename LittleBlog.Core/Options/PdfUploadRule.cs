using LittleBlog.Core.Common;
using LittleBlog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace LittleBlog.Core.Options
{
    public class PdfUploadRule : IUploadRule
    {

        public List<string> folders => new List<string>() { "wwwroot", "upload", "pdfs" };

        public List<string> FileExtensions => new List<string>() { 
            ".pdf"
        };

        public long MaxFileSize => 1024 * 1024 * 10;

        public static string Key => UploadTypes.Pdf;

        public string RequestPath => "/resoures/pdf";

        public string UploadFolder => throw new NotImplementedException();

        public void Validate(IFormFile file)
        {
            if(file.Length > MaxFileSize)
            {
                throw new Exception($"PDF文件大小不超过{MaxFileSize / 1024}Kb");
            }

            if (!FileExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                throw new Exception($"PDF文件后缀名仅限于[{string.Join(",", FileExtensions)}]");
            }
        }

        public string GetUploadFileFolder(UploadInfo uploadInfo)
        {
            throw new NotImplementedException();
        }

        public string GetFileUrl(IUrlHelper urlHelper, UploadInfo uploadInfo)
        {
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Join(Path.DirectorySeparatorChar, folders));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }

        public void Validate(IFormFile file, UploadInfo uploadInfo)
        {
            if (string.IsNullOrEmpty(uploadInfo.FileName))
            {
                //return urlHelper.Content(string.Join("/", folders));
            }
            else
            {
                //return urlHelper.Content($"{string.Join("/", folders)}/{filename}");
            }
        }
    }
}
