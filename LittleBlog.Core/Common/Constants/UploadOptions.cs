using System;
using System.Collections.Generic;
using System.Text;

namespace LittleBlog.Core.Common
{
    public class UploadOptions
    {
        public static Dictionary<string, string> Types = new Dictionary<string, string>()
        {
            { nameof(UploadTypes.Default), UploadTypes.Default },
            { nameof(UploadTypes.Image), UploadTypes.Image },
            { nameof(UploadTypes.MarkdownTheme), UploadTypes.MarkdownTheme },
        };
    }

    public class UploadTypes
    {
        public const string Default = "default";
        public const string Image = "image";
        public const string Pdf = "pdf";
        public const string MarkdownTheme = "md_theme";
    }

    /// <summary>
    /// 上传帮助类
    /// </summary>
    public static class UploadHelper
    {
        private static Dictionary<string, string> _uploadTypesDict = null;

        /// <summary>
        /// 获取所有可以上传的类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllTypes()
        {
            if (_uploadTypesDict == null)
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
