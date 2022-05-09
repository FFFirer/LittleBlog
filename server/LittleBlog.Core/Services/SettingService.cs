using LittleBlog.Core.Extensions;
using LittleBlog.Core.Models;
using LittleBlog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
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
            var info = await GetAsync<WebSiteBaseInfo>();

            if (string.IsNullOrEmpty(info.SiteName))
            {
                info.SiteName = "LittleBlog";
            }

            return info;
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
        internal async Task<TSetting> GetAsync<TSetting>() where TSetting : class, new()
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
        }


        public async Task<List<FriendshipLink>> GetFriendshipLinks(params string[] groups)
        {
            var subSections = groups.Select(a => a.ToString()).ToList();
            var section = GetSectionName<FriendshipLink>();

            var settings = await _settings.GetListAsync(section);

            var links = settings.Select(a => new FriendshipLink()
            {
                Description = a.Description,
                Link = a.Key,
                Group = a.SubSection
            });

            return links.ToList();
        }

        public async Task SaveFriendshipLinks(List<FriendshipLink> links)
        {
            var section = GetSectionName<FriendshipLink>();
            var subSections = links.Select(a => a.Group).ToList();

            var settings = await _settings.GetListAsync(section);
            // 组合
            var (toSave, toDelete) = DiffLinks(links, settings);

            // var added = _settings.SaveListAsync(toAdd);
            await _settings.SaveOrDeleteAsync(toSave, toDelete);
        }

        #region 帮助扩展
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

                if (prop.PropertyType == typeof(string))
                {
                    prop.SetValue(targetObject, setting);
                }
                else
                {
                    prop.SetValue(targetObject, Activator.CreateInstance(prop.PropertyType));
                }
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
        /// 合并修改(Key&Section)
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

        /// <summary>
        /// 区分要删除，修改，新增的链接
        /// </summary>
        /// <param name="links">输入的链接</param>
        /// <param name="existsLinks">已经存在的链接</param>
        /// <returns>
        /// toSave, toDelete
        /// </returns>
        private (IList<SettingModel>, IList<SettingModel>) DiffLinks(IList<FriendshipLink> links, IList<SettingModel> existsLinks)
        {
            var inputLinks = ToSettings(links);
            var toUpdateOrAdd = from inputLink in inputLinks
                                join existsLink in existsLinks on new { inputLink.Key, inputLink.Section, inputLink.SubSection } equals new { existsLink.Key, existsLink.Section, existsLink.SubSection } into linksGroup
                                from link in linksGroup.DefaultIfEmpty()
                                select new
                                {
                                    inputLink,
                                    link
                                };

            var toAdd = toUpdateOrAdd.Where(a => a.link == null).Select(a => a.inputLink);

            var toUpdate = toUpdateOrAdd.Where(a => a.link != null).Select(a =>
            {
                a.inputLink.Id = a.link.Id;
                return a.inputLink;
            });

            foreach (var toSave in toUpdateOrAdd)
            {
                if (toSave.link != null)
                {
                    toSave.inputLink.Id = toSave.link.Id;
                }
            }

            var toDelete = from existsLink in existsLinks
                           join inputLink in inputLinks on new { existsLink.Key, existsLink.Section, existsLink.SubSection } equals new { inputLink.Key, inputLink.Section, inputLink.SubSection } into linksGroup
                           from link in linksGroup.DefaultIfEmpty()
                           where link == null
                           select existsLink;

            return (toUpdateOrAdd.Select(a => a.inputLink).ToList(), toDelete.ToList());
        }

        /// <summary>
        /// 设置转化为链接
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private IList<FriendshipLink> ToLinks(IList<SettingModel> settings)
        {
            return settings.Select(a => new FriendshipLink()
            {
                Description = a.Description,
                Link = a.Value,
                Group = a.SubSection
            }).ToList();
        }

        private IList<SettingModel> ToSettings(IList<FriendshipLink> links)
        {
            var section = GetSectionName<FriendshipLink>();
            return links.Select(a => new SettingModel()
            {
                Key = a.Link,
                Value = a.Link,
                Description = a.Description,
                SubSection = a.Group,
                Section = section
            }).ToList();
        }

        private bool IsBuiltInType(Type type)
        {
            return (type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object); 
        }
        #endregion
    }
}
