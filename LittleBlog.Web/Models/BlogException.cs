using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web
{
    public class BlogException : Exception
    {
        public BlogException() : base()
        {

        }

        public BlogException(string message) : base(message)
        {

        }
    }
}
