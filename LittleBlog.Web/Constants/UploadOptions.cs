using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web
{
    public class UploadOptions
    {
        public static Dictionary<string, string> Types = new Dictionary<string, string>()
        {
            { nameof(UploadTypes.Default), UploadTypes.Default },
            { nameof(UploadTypes.Image), UploadTypes.Image },
        };
    }

    public class UploadTypes
    {
        public const string Default = "Default";
        public const string Image = "Image";
        public const string Pdf = "pdf";
    }

    /// <summary>
    /// 写入数据库，系统默认值
    /// </summary>
    public static class UploadHelper
    {
        private static Dictionary<string, string> _uploadTypesDict = null;
        public static Dictionary<string, string> GetAllTypes()
        {
            if(_uploadTypesDict == null)
            {
                _uploadTypesDict = new Dictionary<string, string>();

                Type type = typeof(UploadTypes);

                var instance = new UploadTypes();

                foreach (var field in type.GetFields(System.Reflection.BindingFlags.Static))
                {
                    _uploadTypesDict.Add(field.Name, field.GetValue(instance).ToString());
                }
            }

            return _uploadTypesDict;
        }
    }
}
