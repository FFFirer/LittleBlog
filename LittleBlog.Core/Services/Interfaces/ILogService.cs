using LittleBlog.Core.Models;
using LittleBlog.Core.Models.QueryContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface ILogService
    {
        Task<Paging<LogDto>> QueryAsync(ListLogQueryContext queryContext);
    }
}
