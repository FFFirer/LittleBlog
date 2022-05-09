using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleBlog.Web.Pages
{
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] 
    public class ErrorCodeModel : PageModel
    {
        public int? HttpStatusCode { get; set; }

        public bool HasHttpStatusCode => HttpStatusCode != null;

        public void OnGet(int? code)
        {
            HttpStatusCode = code;
        }
    }
}
