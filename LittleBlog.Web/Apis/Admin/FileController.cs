using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using LittleBlog.Web.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class FileController : BaseApiController
    {
        private IFileService _fileService;

        public FileController(IFileService fileService, ILogger<FileController> logger)
        {
            _logger = logger;
            _fileService = fileService;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ResultModel> Upload(IFormFile data, [FromForm]UploadInfo upload)
        {
            try
            {
                var result = await _fileService.SaveFileAsync(upload, data);
                
                return Success(result);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"上传失败: {upload.FileName}");
                return Fail(ex, "上传失败");
            }
        }
    }
}
