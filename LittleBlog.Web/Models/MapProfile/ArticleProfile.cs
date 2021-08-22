using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LittleBlog.Web.Models.DtoModel;

namespace LittleBlog.Web.Models.MapProfile
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>()
                .ForMember(a => a.CategoryName, map => map.MapFrom(b => b.Category))
                .ForMember(a => a.SavePath, map => map.MapFrom(b => b.SavePath ?? string.Empty))
                .ReverseMap();
        }
    }
}
