using AutoMapper;

namespace LittleBlog.Core.Models
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
