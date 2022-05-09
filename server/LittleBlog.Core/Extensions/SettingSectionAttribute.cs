using System;

namespace LittleBlog.Core.Extensions
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
