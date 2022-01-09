using AutoMapper;
using LittleBlog.Core.Common;
using LittleBlog.Core.Models;
using LittleBlog.Core.Models.Domain;
using LittleBlog.Core.Models.Dto;
using LittleBlog.Core.Options;
using LittleBlog.Core.Repositories.Interfaces;
using LittleBlog.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public class MarkdownThemeService : IMarkdownThemeService
    {
        private readonly IMarkdownThemeRepo _mdThemeRepo;
        private readonly IMapper _mapper;
        private readonly IOptionsSnapshot<UploadOption> _namedOptionsAccessor;
        private readonly IActionContextAccessor _actionContextAccessor;

        public MarkdownThemeService(IMarkdownThemeRepo mdThemeRepo
            , IMapper mapper
            , IOptionsSnapshot<UploadOption> namedOptionsAccessor
            , IActionContextAccessor actionContextAccessor)
        {
            _mdThemeRepo = mdThemeRepo;
            _mapper = mapper;
            _namedOptionsAccessor = namedOptionsAccessor;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<List<MarkdownThemeSummaryDto>> GetAllSummariesAsync()
        {
            var dtos = await _mdThemeRepo.Themes.Select(a => new MarkdownThemeSummaryDto
            {
                Id = a.Id,
                Name = a.Name,
                Url = a.Url,
                PhysicalPath = a.PhysicalPath,
                Remark = a.Remark,
            }).ToListAsync();

            return dtos;
        }

        public async Task<List<MarkdownThemeDto>> GetAllSync()
        {
            var themes = await _mdThemeRepo.GetAllAsync();

            var dtos = _mapper.Map<List<MarkdownThemeDto>>(themes);

            return dtos;
        }

        public async Task<MarkdownThemeDto> GetByIdAsync(Guid id)
        {
            var themeEntity = await _mdThemeRepo.GetOneAsync(id);

            var themeDto = _mapper.Map<MarkdownThemeDto>(themeEntity);

            return themeDto;
        }

        public async Task<int> SaveAsync(MarkdownThemeDto markdownThemeDto)
        {
            var themeEntity = _mapper.Map<MarkdownTheme>(markdownThemeDto);

            if (themeEntity.Id.Equals(Guid.Empty))
            {
                var effected = await _mdThemeRepo.CreateAsync(themeEntity);
            }

            await SaveToDisk(themeEntity);

            return await _mdThemeRepo.UpdateAsync(themeEntity);
        }

        private async Task SaveToDisk(MarkdownTheme theme)
        {
            var uploadOption = _namedOptionsAccessor.Get(MarkdownThemeUploadRule.Key);

            var filename = $"{theme.Name}.css";

            var uploadInfo = new UploadInfo()
            {
                FileName = filename,
                Group = theme.Id.ToString(),
                Type = UploadTypes.MarkdownTheme.ToString(),
            };

            var saveFolder = uploadOption.Rule.GetUploadFileFolder(uploadInfo);

            var saveFilePath = Path.Combine(saveFolder, uploadInfo.FileName);

            if (File.Exists(saveFilePath))
            {
                var backupFileName = $"{Path.GetFileName(uploadInfo.FileName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{Path.GetExtension(uploadInfo.FileName)}.bak";
                var backupFilePath = Path.Combine(saveFolder, backupFileName);
                var fileInfo = new FileInfo(saveFilePath);
                fileInfo.MoveTo(backupFilePath);
            }

            using (var fs = new FileStream(saveFilePath, FileMode.CreateNew))
            {
                using (var writer = new StreamWriter(fs))
                {
                    await writer.WriteAsync(theme.Content.Replace("\n", ""));
                }
            }

            var urlHelper = new UrlHelper(_actionContextAccessor.ActionContext);
            var url = uploadOption.Rule.GetFileUrl(urlHelper, uploadInfo);
            url = $"{url}?v={DateTime.Now.ToString("yyyy.MM.dd.HHmmss")}";

            theme.Url = url;
            theme.PhysicalPath = saveFilePath;
        }

        public Task SaveToDisk(MarkdownThemeDto themeDto)
        {
            var themeEntity = _mapper.Map<MarkdownTheme>(themeDto);

            return this.SaveToDisk(themeEntity);
        }
    }
}
