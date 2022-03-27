using LittleBlog.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LittleBlog.Web.Pages
{
    [TypeFilter(typeof(PageExceptionFilterAttribute))]
    public class BasePageModel : PageModel
    {

    }
}
