using LittleBlog.Web.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Repositories.Interfaces
{
    public interface ISettingRepo
    {
        Task<IList<SettingModel>> GetOneAsync(string sectionName);

        Task<long> SaveListAsync(IList<SettingModel> list);
    }
}
