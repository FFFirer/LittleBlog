using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/Tags")]
    [ApiController]
    [Description("标签管理相关端口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [OpenApiTags("Admin Tags")]
    public class TagsManageController : ControllerBase
    {
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public Task<IActionResult>List()
        {
            throw new  NotImplementedException();
        }
    }
}
