using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.UnitOfWork;

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
            List<BlogPost> posts = unit.PostRepository.GetAll();

            if (posts.Count == 0)
                return new List<BlogPostDTO>();

            List<BlogPostDTO> blogPostDTOs = new List<BlogPostDTO>();

            foreach (var p in posts)
            {
                BlogPostDTO post = new BlogPostDTO()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Status = p.Status
                };
                blogPostDTOs.Add(post);
            }
            return blogPostDTOs;
        }

        public BlogPostDTO GetById(int id)
        {
            BlogPost? post = unit.PostRepository.GetById(id);

            if (post == null) return null;
            else
            {
                BlogPostDTO blogPostDTO = new BlogPostDTO()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    Status = post.Status
                };

                return blogPostDTO;
            }
        }

        public BlogPostDTO Add(BlogPostDTO PDTO)
        {
            BlogPost post = new BlogPost()
            {
                Id = PDTO.Id,
                Title = PDTO.Title,
                Content = PDTO.Content,
                Status = PDTO.Status
            };

            unit.PostRepository.Add(post);
            unit.Save();

            return new BlogPostDTO
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Status = post.Status
            };
        }

        public string Update(BlogPostDTO PDTO)
        {
            var existingPost = unit.PostRepository.GetById(PDTO.Id);
            if (existingPost == null)
                return "NotFound";

            if (existingPost.Id == PDTO.Id && existingPost.Title == PDTO.Title
                && existingPost.Content == PDTO.Content && existingPost.Status == PDTO.Status)
            { return "NoChanges"; }
            else
            {
                existingPost.Id = PDTO.Id;
                existingPost.Title = PDTO.Title;
                existingPost.Content = PDTO.Content;
                existingPost.Status = PDTO.Status;
            }
            unit.PostRepository.Update(existingPost);
            unit.PostRepository.Save();

            return "Updated";
        }

        public bool Delete(int id)
        {
            var toDo = unit.PostRepository.GetById(id);
            if (toDo == null) return false;

            unit.PostRepository.Delete(toDo);
            unit.PostRepository.Save();

            return true;
        }
    }
}