using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleBlog.Core.Models
{
    public class LogProfile : Profile
    {
        public LogProfile()
        {
            CreateMap<LogEntity, LogDto>()
                .ForMember(a => a.LogLevel, map => map.MapFrom(b => b.LogLevel.ToString()))
                .ForMember(a => a.Logged, map => map.MapFrom(b => b.Logged.ToLocalTime().ToLongTimeString()));
        }
    }
}
