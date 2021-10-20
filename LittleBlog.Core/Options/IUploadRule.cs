using LittleBlog.Core.Common;
using LittleBlog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LittleBlog.Core.Options
{
    public interface IUploadRule
    {
        public static string Key { get; } = UploadTypes.Default;
        public List<string> folders { get; }

        public string RequestPath { get; }
        public string UploadFolder { get; }

        public List<string> FileExtensions { get; }

        public long MaxFileSize { get; }

        public string GetUploadFileFolder(UploadInfo uploadInfo);

        public string GetFileUrl(IUrlHelper urlHelper, UploadInfo uploadInfo);

        public void Validate(IFormFile file, UploadInfo uploadInfo);
    }
}
