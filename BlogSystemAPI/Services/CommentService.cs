using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BlogSystemAPI.Services
{
    public class CommentService
    {
        UnitWork unit;
        public CommentService(UnitWork unit)
        {
            this.unit = unit;
        }

        //GeyByPost
        public List<CommentDTO> GetByPost(int postID)
        {
            return unit.CommentRepository
                    .GetAll()
                    .Where(c => c.BlogPostID == postID)
                    .Select(c => new CommentDTO
                    {
                        Id = c.Id,
                        Content = c.Content
                    })
                    .ToList();
        }

        //Add Comment
        public CommentDTO Add(CommentDTO commentDTO)
        {
            var post = unit.PostRepository
                .GetAll()
                .FirstOrDefault(p => p.Title == commentDTO.PostTitle);

            if (post == null) return null;

            var comment = new Comment
            {
                Id = commentDTO.Id,
                Content = commentDTO.Content,
                BlogPostID = post.Id
            };

            unit.CommentRepository.Add(comment);
            unit.Save();

            return new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                PostTitle = post.Title
            };
        }

        //Updtae Comment
        public String Update(int id, CommentDTO commentDTO)
        {
            var exsitingComment = unit.CommentRepository
                            .GetAll()
                            .Include(p => p.BlogPost)
                            .FirstOrDefault(p => p.Id == commentDTO.Id);

            if (exsitingComment == null) return "NotFound";

            var post = unit.PostRepository
                .GetAll()
                .FirstOrDefault(p => p.Title == commentDTO.PostTitle);

            if (post == null) return "Invalid Post";

            if (exsitingComment.Id == commentDTO.Id && exsitingComment.Content == commentDTO.Content
                && exsitingComment.BlogPostID == post.Id)
            { 
                return "NoChanges";
            }

            exsitingComment.Id = commentDTO.Id;
            exsitingComment.Content = commentDTO.Content;
            exsitingComment.BlogPostID = post.Id;

            unit.CommentRepository.Update(exsitingComment);
            unit.Save();

            return "Updated";
        }

        //Delete Comment
        public bool Delete(int commentID)
        {
            var comment =unit.CommentRepository.GetById(commentID);
            if (comment == null)
                return false;

            unit.CommentRepository.Delete(comment);
            unit.Save();

            return true;
        }
    }
}
