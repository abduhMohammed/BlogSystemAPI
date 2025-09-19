using AutoMapper;
using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BlogSystemAPI.Services
{
    public class CommentService
    {
        private readonly UnitWork unit;
        private readonly IMapper mapper;

        public CommentService(UnitWork unit, IMapper mapper)
        {
            this.unit = unit;
            this.mapper = mapper;
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

            Comment comment = mapper.Map<Comment>(commentDTO);
            comment.BlogPostID = post.Id;

            unit.CommentRepository.Add(comment);
            unit.Save();

            return mapper.Map<CommentDTO>(comment);
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

            mapper.Map(commentDTO, exsitingComment);

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
