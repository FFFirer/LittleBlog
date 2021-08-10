using LittleBlog.Web.Models;
using LittleBlog.Web.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Services
{
    public class FileService : IFileService
    {
        private const string _uploadFolder = "upload";

        private readonly IOptionsSnapshot<UploadOption> _namedOptionsAccesser;
        public FileService(IOptionsSnapshot<UploadOption> namedOptionsAccessor)
        {
            _namedOptionsAccesser = namedOptionsAccessor;
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

            if(uploadOption != null)
            {
                if (!uploadOption.Rule.FileExtensions.Contains(Path.GetExtension(file.FileName)))
                {
                    throw new Exception($"允许的扩展为[{string.Join(",", uploadOption.Rule.FileExtensions)}]");
                }
            }

            if (string.IsNullOrEmpty(info.Group))
            {
                info.Group = Guid.NewGuid().ToString();
            }
            var saveFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _uploadFolder, info.Group);

            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }

            if (string.IsNullOrEmpty(info.FileName))
            {
                info.FileName = $"TMP_{Guid.NewGuid().ToString("N")}{Path.GetExtension(file.FileName)}";
            }

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

            return new UploadResult()
            {
                Group = info.Group,
                FileName = info.FileName,
                FileId = null,
                Url = $"{_uploadFolder}/{info.Group}/{info.FileName}",
                IsFinish = true
            };
        }
    }
}
