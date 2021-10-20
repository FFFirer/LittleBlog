using System;

namespace LittleBlog.Core.Models
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
