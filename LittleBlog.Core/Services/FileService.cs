using LittleBlog.Core.Models;
using LittleBlog.Core.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public class FileService : IFileService
    {
        private const string _uploadFolder = "upload";
        private readonly IActionContextAccessor _actionContextAccessor;

        private readonly IOptionsSnapshot<UploadOption> _namedOptionsAccesser;
        public FileService(IOptionsSnapshot<UploadOption> namedOptionsAccessor, IActionContextAccessor actionContextAccessor)
        {
            _namedOptionsAccesser = namedOptionsAccessor;
            _actionContextAccessor = actionContextAccessor;
        }

        /// <summary>
        /// 保存文件，暂不实现分片
        /// </summary>
        /// <param name="info"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task<UploadResult> SaveFileAsync(UploadInfo info, IFormFile file)
        {
            var uploadOption = _namedOptionsAccesser.Get(info.Type);

            if(uploadOption == null || uploadOption.Rule == null)
            {
                throw new UploadException("没有指定的上传规则"); 
            }

            uploadOption.Rule.Validate(file, info);

            
            var saveFolder = uploadOption.Rule.GetUploadFileFolder(info);

            var saveFilePath = Path.Combine(saveFolder, info.FileName);

            if (File.Exists(saveFilePath))
            {
                // 备份，重命名
                var backUpFileName = $"{Path.GetFileName(info.FileName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(info.FileName)}.bak";
                var backUpFilePath = Path.Combine(saveFolder, backUpFileName);
                var fileInfo = new FileInfo(saveFilePath);
                fileInfo.MoveTo(backUpFilePath);
            }

            using (var fs = new FileStream(saveFilePath, FileMode.CreateNew))
            {
                await file.OpenReadStream().CopyToAsync(fs);
            }
            
            var urlHelper = new UrlHelper(_actionContextAccessor.ActionContext);

            return new UploadResult()
            {
                Group = info.Group,
                FileName = info.FileName,
                FileId = null,
                Url = uploadOption.Rule.GetFileUrl(urlHelper, info),
                IsFinish = true
            };
        }
    }
}
