using BlogSystemAPI.DTO;
using BlogSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        CommentService commentService;
        public CommentController(CommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet("{postId}")]
        public IActionResult GetByPost(int postID)
        {
            var comments = commentService.GetByPost(postID);

            if (comments == null || comments.Count == 0) 
                return NotFound("No Comments Found for this Post.");

            return Ok(comments);
        }

        [HttpPost]
        public IActionResult Add(CommentDTO commentDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (commentDTO == null) return NotFound();

            var created = commentService.Add(commentDTO); 

            return Created();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CommentDTO commentDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (commentDTO == null) return NotFound();

            var Updated = commentService.Update(id, commentDTO);

            if (Updated == "NotFound")
                return NotFound();
            if (Updated == "NoChanges")
                return Ok("No Changes were made to this comment");

            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = commentService.Delete(id);

            if (!deleted) return NotFound();
            return Ok("Deleted Successfuly");
        }
    }
}
