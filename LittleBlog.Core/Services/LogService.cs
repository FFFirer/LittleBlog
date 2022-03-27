using AutoMapper;
using LittleBlog.Core.Extensions;
using LittleBlog.Core.Models;
using LittleBlog.Core.Models.QueryContext;
using LittleBlog.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepo _logRepo;
        private readonly IMapper _mapper;

        public LogService(ILogRepo logRepo, IMapper mapper)
        {
            this._logRepo = logRepo;
            this._mapper = mapper;
        }

        public async Task<Paging<LogDto>> PageAsync(ListLogQueryContext queryContext)
        {
            var pagingLogEntities = await _logRepo.ListAsync(queryContext);

            var pagingLogDtos = _mapper.MapPaging<LogEntity, LogDto>(pagingLogEntities);

            pagingLogDtos.Rows = pagingLogDtos.Rows.OrderByDescending(a => a.Id).ToList();

            return pagingLogDtos;
        }
    }
}
