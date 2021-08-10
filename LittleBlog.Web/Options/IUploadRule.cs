using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Options
{
    public interface IUploadRule
    {
        public static string Key { get; } = UploadTypes.Default;
        public List<string> FileExtensions { get; }

        public long MaxFileSize { get; }

        public string GetUploadFileFolder();

        public string GetFileUrl();

        public void Validate(IFormFile file);
    }
}
