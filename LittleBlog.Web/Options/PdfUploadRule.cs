using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Options
{
    public class PdfUploadRule : IUploadRule
    {
        public List<string> FileExtensions => new List<string>() { 
            ".pdf"
        };

        public long MaxFileSize => throw new NotImplementedException();

        public static string Key => UploadTypes.Pdf;

        public string GetFileUrl()
        {
            throw new NotImplementedException();
        }

        public string GetUploadFileFolder()
        {
            throw new NotImplementedException();
        }

        public void Validate(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
