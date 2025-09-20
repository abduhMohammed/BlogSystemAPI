using AutoMapper;
using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;

namespace BlogSystemAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>()
           .ForMember(dest => dest.blogPosts, opt => opt.MapFrom(src => src.blogPosts));

            CreateMap<BlogPost, BlogPostDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
