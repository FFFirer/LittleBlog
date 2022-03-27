using LittleBlog.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LittleBlog.Web.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ExceptionTestController : BaseApiController
    {
        // GET: api/<ExceptionTestController>
        [HttpGet]
        public IActionResult TestGet()
        {
            throw new System.Exception("Test Get Exception");
        }
    }
}
