using AutoMapper;
using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BlogSystemAPI.Services
{
    public class PostService
    {
        private readonly UnitWork unit;
        private readonly IMapper mapper;

        public PostService(UnitWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
        }

        public List<BlogPostDTO> GetAll()
        {
            var posts = unit.PostRepository
                .GetAll()
                .Include(c => c.Category)
                .ToList();

            if (!posts.Any())
                return new List<BlogPostDTO>();

            return mapper.Map<List<BlogPostDTO>>(posts);
        }

        public BlogPostDTO GetById(int id)
        {
            BlogPost? post = unit.PostRepository
                .GetAll()
                .Include(c => c.Category)
                .FirstOrDefault(p => p.Id == id);

            if (post == null) return null;

            return mapper.Map<BlogPostDTO>(post);
        }

        public BlogPostDTO Add(BlogPostDTO PDTO)
        {
            var category = unit.CategoryRepository
                       .GetAll()
                       .FirstOrDefault(c => c.Name == PDTO.CategoryName);

            if (category == null) 
                return null;

            var post = mapper.Map<BlogPost>(PDTO);
            post.Category = category;

            unit.PostRepository.Add(post);
            unit.Save();

            return mapper.Map<BlogPostDTO>(post);
        }

        public string Update(BlogPostDTO PDTO)
        {
            var existingPost = unit.PostRepository
                .GetAll()
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == PDTO.Id);

            if (existingPost == null)
                return "NotFound";

            var category = unit.CategoryRepository
                   .GetAll()
                   .FirstOrDefault(c => c.Name == PDTO.CategoryName);

            if (category == null)
                return "InvalidCategory";

            // Check if no changes
            if (existingPost.Title == PDTO.Title
                && existingPost.Content == PDTO.Content
                && existingPost.Status == PDTO.Status
                && existingPost.CategoryID == category.Id)
            {
                return "NoChanges";
            }

            mapper.Map(PDTO, existingPost);

            unit.PostRepository.Update(existingPost);
            unit.Save();

            return "Updated";
        }

        public bool Delete(int id)
        {
            var toDo = unit.PostRepository.GetById(id);
            if (toDo == null) return false;

            unit.PostRepository.Delete(toDo);
            unit.Save();

            return true;
        }
    }
}