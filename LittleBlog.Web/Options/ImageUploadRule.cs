using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Options
{
    public class ImageUploadRule : IUploadRule
    {
        public static string Key => UploadTypes.Image;

        public List<string> FileExtensions => new List<string>() {
            ".jpg",
            ".jpeg"
        };

        public long MaxFileSize => 1024 * 1024 * 10;

        public string GetFileUrl()
        {
            return string.Empty;
        }

        public string GetUploadFileFolder()
        {
            return string.Empty;
        }

        public void Validate(IFormFile file)
        {
            
        }
    }
}
