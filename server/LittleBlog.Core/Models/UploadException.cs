using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Core.Models
{
    public class UploadException : Exception
    {
        public UploadException() :base(){}

        public UploadException(string message):base(message) { }
    }
}
