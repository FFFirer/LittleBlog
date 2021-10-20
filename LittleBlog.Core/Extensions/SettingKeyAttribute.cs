using System;

namespace LittleBlog.Core.Extensions
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SettingKeyAttribute : Attribute
    {
        public string KeyName { get; set; }

        public SettingKeyAttribute(string keyName)
        {
            this.KeyName = keyName;
        }
    }
}
