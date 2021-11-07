using LittleBlog.Core.Extensions;
using LittleBlog.Core.Models;
using LittleBlog.Core.Models.QueryContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Repositories
{
    public class LogRepo : ILogRepo
    {
        private readonly LittleBlogContext _db;
        public LogRepo(LittleBlogContext littleBlogContext)
        {
            this._db = littleBlogContext;

        }
        public async Task<Paging<LogEntity>> ListAsync(ListLogQueryContext queryContext)
        {
            var paging = new Paging<LogEntity>();

            var query = _db.Logs.AsNoTracking();

            if (queryContext.StartTime.HasValue)
            {
                query = query.Where(a=>a.Logged >= queryContext.StartTime);
            }

            if (queryContext.EndTime.HasValue)
            {
                query = query.Where(a=>a.Logged <= queryContext.EndTime);
            }

            if (!string.IsNullOrEmpty(queryContext.LogLevel))
            {
                query = query.Where(a => a.LogLevel == queryContext.LogLevel);
            }
            
            if (!string.IsNullOrEmpty(queryContext.Logger))
            {
                if (queryContext.Logger.EndsWith('*'))
                {
                    var queryLogger = queryContext.Logger.TrimEnd('*');
                    query = query.Where(a => a.Logger.StartsWith(queryLogger));
                }
                else
                {
                    query = query.Where(a => a.Logger.Equals(queryContext.Logger));
                }
            }

            paging.Total = await query.CountAsync();

            query = query.OrderByDescending(a=>a.Id);

            query = query.Paging(queryContext);

            paging.Rows = await query.ToListAsync();

            return paging;
        }
    }
}
