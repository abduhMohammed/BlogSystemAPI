using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BlogSystemAPI.Services
{
    public class PostService
    {
        UnitWork unit;
        public PostService(UnitWork unit)
        {
            this.unit = unit;
        }

        public List<BlogPostDTO> GetAll()
        {
            var posts = unit.PostRepository
                .GetAll()
                .Include(c => c.Category)
                .ToList();

            if (!posts.Any())
                return new List<BlogPostDTO>();

            List<BlogPostDTO> blogPostDTOs = new List<BlogPostDTO>();

            foreach (var p in posts)
            {
                BlogPostDTO post = new BlogPostDTO()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Status = p.Status,
                    CategoryName = p.Category?.Name
                };
                blogPostDTOs.Add(post);
            }
            return blogPostDTOs;
        }

        public BlogPostDTO GetById(int id)
        {
            BlogPost? post = unit.PostRepository
                .GetAll()
                .Include(c => c.Category)
                .FirstOrDefault(p => p.Id == id);

            if (post == null) return null;
            else
            {
                BlogPostDTO blogPostDTO = new BlogPostDTO()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    Status = post.Status,
                    CategoryName = post.Category?.Name
                };

                return blogPostDTO;
            }
        }

        public BlogPostDTO Add(BlogPostDTO PDTO)
        {
            var category = unit.CategoryRepository
                       .GetAll()
                       .FirstOrDefault(c => c.Name == PDTO.CategoryName);

            if (category == null) return null;

            BlogPost post = new BlogPost()
            {
                Id = PDTO.Id,
                Title = PDTO.Title,
                Content = PDTO.Content,
                Status = PDTO.Status,
                CategoryID = category.Id
            };

            unit.PostRepository.Add(post);
            unit.Save();

            return new BlogPostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Status = post.Status,
                CategoryName = category.Name
            };
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

            // Apply updates
            existingPost.Title = PDTO.Title;
            existingPost.Content = PDTO.Content;
            existingPost.Status = PDTO.Status;
            existingPost.CategoryID = category.Id;

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