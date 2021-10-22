using LittleBlog.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Repositories
{
    public interface ISettingRepo
    {
        Task<IList<SettingModel>> GetOneAsync(string sectionName);

        Task<IList<SettingModel>> GetListAsync(string sectionName, List<string> subSectionNames);

        Task<long> SaveListAsync(IList<SettingModel> list);
    }
}
