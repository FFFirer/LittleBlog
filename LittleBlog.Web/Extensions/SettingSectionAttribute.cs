using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Extensions
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class SettingSectionAttribute : Attribute
    {
        public string SectionName { get; set; }

        public SettingSectionAttribute(string sectionName)
        {
            this.SectionName = sectionName;
        }
    }
}
