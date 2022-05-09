using AutoMapper;
using LittleBlog.Core.Models.Domain;
using LittleBlog.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Models.MapProfile
{
    public class MarkdownThemeProfile : Profile
    {
        public MarkdownThemeProfile()
        {
            CreateMap<MarkdownTheme, MarkdownThemeDto>().ReverseMap();
            CreateMap<MarkdownTheme, MarkdownThemeSummaryDto>();
        }
    }
}
