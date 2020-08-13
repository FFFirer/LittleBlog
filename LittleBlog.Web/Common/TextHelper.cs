using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LittleBlog.Web.Common
{
    public class TextHelper
    {
        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <returns></returns>
        public static string GetAbstract(string htmlCode)
        {
            string plainPattern = @"<\/?.+?\/?>";
            string outText = Regex.Replace(htmlCode, plainPattern, "");

            

            return outText.Substring(0, outText.Length > 255 ? 255 : outText.Length);
        }
    }
}
