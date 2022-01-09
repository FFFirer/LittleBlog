using LittleBlog.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Repositories.Interfaces
{
    public interface IMarkdownThemeRepo
    {
        IQueryable<MarkdownTheme> Themes { get; }

        Task<IList<MarkdownTheme>> GetAllAsync();

        Task<MarkdownTheme> GetOneAsync(Guid id);

        Task<int> CreateAsync(MarkdownTheme theme);

        Task<int> UpdateAsync(MarkdownTheme theme);

        Task<int> DeleteAsync(Guid id);
    }
}
