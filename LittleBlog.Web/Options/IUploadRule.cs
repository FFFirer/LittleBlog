using LittleBlog.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Options
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
