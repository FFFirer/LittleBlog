using LittleBlog.Web.Data;
using LittleBlog.Web.Extensions;
using LittleBlog.Web.Models.DomainModels;
using LittleBlog.Web.Models.DtoModel.Settings;
using LittleBlog.Web.Repositories.Interfaces;
using LittleBlog.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LittleBlog.Web.Services
{
    public class SettingService : ISettingService
    {
        private ISettingRepo _settings { get; set; }

        public SettingService(ISettingRepo settingRepo)
        {
            this._settings = settingRepo;
        }

        public async Task<WebSiteBaseInfo> GetWebSiteBaseInfo()
        {
            return await GetAsync<WebSiteBaseInfo>();
        }

        public async Task<WebSiteFiling> GetWebSiteFiling()
        {
            return await GetAsync<WebSiteFiling>();
        }

        /// <summary>
        /// 获取指定设置的值
        /// </summary>
        /// <typeparam name="TSetting"></typeparam>
        /// <returns></returns>
        private async Task<TSetting> GetAsync<TSetting>() where TSetting : class, new()
        {
            var sectionName = GetSectionName<TSetting>();
            var settings = await _settings.GetOneAsync(sectionName);
            return ConvertTo<TSetting>(settings);
        }

        public async Task SaveAsync<TSetting>(TSetting setting) where TSetting : class, new()
        {
            var sectionName = GetSectionName<TSetting>();
            var oldSettingDetails = await _settings.GetOneAsync(sectionName);
            var newSettingDetails = GetDetails(setting);
            var currentSettings = MergeSettingDetails(oldSettingDetails, newSettingDetails);
            var effected = await _settings.SaveListAsync(currentSettings);

            if (effected <= 0)
            {
                throw new Exception("保存失败");
            }
        }

        /// <summary>
        /// 获取段名称
        /// </summary>
        /// <typeparam name="TSetting"></typeparam>
        /// <returns></returns>
        private string GetSectionName<TSetting>() where TSetting : class, new()
        {
            var targetType = typeof(TSetting);
            var sectionAttribute = targetType.GetCustomAttribute<SettingSectionAttribute>();

            if (sectionAttribute == null || string.IsNullOrEmpty(sectionAttribute.SectionName))
            {
                return targetType.Name;
            }

            return sectionAttribute.SectionName;
        }

        /// <summary>
        /// 序列化为对象 
        /// </summary>
        /// <typeparam name="TSetting"></typeparam>
        /// <param name="settings"></param>
        /// <returns></returns>
        private TSetting ConvertTo<TSetting>(IList<SettingModel> settings) where TSetting : class, new()
        {
            var targetType = typeof(TSetting);
            var properties = targetType.GetProperties();

            var targetObject = new TSetting();

            foreach (var prop in properties)
            {
                var keyNameAttribute = prop.GetCustomAttribute<SettingKeyAttribute>();

                string keyName = prop.Name;

                if (keyNameAttribute != null)
                {
                    keyName = keyNameAttribute.KeyName;
                }

                var setting = settings.FirstOrDefault(x => x.Key.Equals(keyName))?.Value ?? string.Empty;

                prop.SetValue(targetObject, setting);
            }

            return targetObject;
        }

        private IList<SettingModel> GetDetails<TSetting>(TSetting setting) where TSetting : class, new()
        {
            var targetType = typeof(TSetting);
            var details = new List<SettingModel>();
            var sectionName = GetSectionName<TSetting>();
            var properties = targetType.GetProperties();
            foreach (var prop in properties)
            {
                var detail = new SettingModel()
                {
                    Section = sectionName,
                    Key = prop.Name,
                };
                var keyNameAttribute = prop.GetCustomAttribute<SettingKeyAttribute>();

                if (keyNameAttribute != null)
                {
                    detail.Key = keyNameAttribute.KeyName;
                }

                var value = prop.GetValue(setting, null);

                detail.Value = value.ToString();
                details.Add(detail);
            }

            return details;
        }

        /// <summary>
        /// 合并修改
        /// </summary>
        /// <param name="oldSettings"></param>
        /// <param name="newSettings"></param>
        private IList<SettingModel> MergeSettingDetails(IList<SettingModel> oldSettings, IList<SettingModel> newSettings)
        {
            var result = new List<SettingModel>();

            var toUpdate = oldSettings.Join(newSettings
                                            , a => new { a.Key, a.Section }
                                            , b => new { b.Key, b.Section }
                                            , (oldone, newonw) =>
              {
                  oldone.Value = newonw.Value;
                  oldone.Description = newonw.Description;
                  return oldone;
              });

            result.AddRange(toUpdate.ToList());

            var toAdd = from newone in newSettings
                        join oldone in oldSettings on new { newone.Key, newone.Section } equals new { oldone.Key, oldone.Section } into allSettings
                        from setting in allSettings.DefaultIfEmpty()
                        where setting == null
                        select newone;

            result.AddRange(toAdd.ToList());

            return result;
        }
    }
}
