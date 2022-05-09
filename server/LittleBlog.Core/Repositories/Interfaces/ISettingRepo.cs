using System.Collections;
using LittleBlog.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Repositories
{
    public interface ISettingRepo
    {
        /// <summary>
        /// 获取某个配置的所有项
        /// </summary>
        /// <param name="sectionName">类名</param>
        /// <returns></returns>
        Task<IList<SettingModel>> GetOneAsync(string sectionName);

        /// <summary>
        /// 获取某个配置的所有项
        /// </summary>
        /// <param name="sectionName">类名</param>
        /// <param name="subSectionNames">分组</param>
        /// <returns></returns>
        Task<IList<SettingModel>> GetListAsync(string sectionName, params string[] subSectionNames);

        /// <summary>
        /// 更新或新增配置选项
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<long> SaveListAsync(IList<SettingModel> list);

        /// <summary>
        /// 保存或新增配置选项
        /// </summary>
        /// <param name="toSave">要新增或更新的配置项</param>
        /// <param name="toDelete">要删除的配置项</param>
        /// <returns></returns>
        Task<long> SaveOrDeleteAsync(IList<SettingModel> toSave, IList<SettingModel> toDelete);
    }
}
