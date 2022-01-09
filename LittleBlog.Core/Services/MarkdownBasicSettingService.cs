using LittleBlog.Core.Models.Dto.Settings;
using LittleBlog.Core.Repositories;
using LittleBlog.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public class MarkdownBasicSettingService : SettingService, IMarkdownBasicSettingService
    {
        public MarkdownBasicSettingService(ISettingRepo settingRepo) : base(settingRepo)
        {
        }

        public async Task<MarkdownThemeInfo> GetMarkdownThemeInfo()
        {
            var themeInfo = await this.GetAsync<MarkdownThemeInfo>();

            if(themeInfo == null)
            {
                themeInfo = new MarkdownThemeInfo();
            }

            return themeInfo;
        }

        public async Task SaveMarkdownThemeInfo(MarkdownThemeInfo themeInfo)
        {
            await this.SaveAsync(themeInfo);
        }
    }
}
