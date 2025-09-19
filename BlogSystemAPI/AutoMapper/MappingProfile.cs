using AutoMapper;
using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;

namespace BlogSystemAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<BlogPost, BlogPostDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
