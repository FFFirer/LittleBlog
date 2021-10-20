using System.Text.RegularExpressions;

namespace LittleBlog.Core
{
    public static class HtmlTextHelper
    {
        /// <summary>
        /// 去除html标签
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <returns></returns>
        public static string GetAbstract(string htmlCode)
        {
            string plainPattern = @"<\/?.+?\/?>";

            // 替换掉所有Html标签
            string outText = Regex.Replace(htmlCode, plainPattern, "");

            return outText.Substring(0, outText.Length > 255 ? 255 : outText.Length);
        }
    }
}
