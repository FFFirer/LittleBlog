using System.Collections;
using LittleBlog.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Repositories
{
    public interface ISettingRepo
    {
        Task<IList<SettingModel>> GetOneAsync(string sectionName);

        Task<IList<SettingModel>> GetListAsync(string sectionName, params string[] subSectionNames);

        Task<long> SaveListAsync(IList<SettingModel> list);

        Task<long> SaveOrDeleteAsync(IList<SettingModel> toSave, IList<SettingModel> toDelete);
    }
}
