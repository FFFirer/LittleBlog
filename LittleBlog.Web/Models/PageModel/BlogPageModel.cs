using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models
{
    public class BlogPageModel : PageModel
    {
        private string _siteName { get; set; }
        public string SiteName
        {
            get
            {
                if (string.IsNullOrEmpty(_siteName))
                {
                    return "LittleBlog";
                }
                else
                {
                    return _siteName;
                }
            }
        }

        public string _filingNumber { get; set; }
        public string FilingNumber { get; set; }
    }
}
