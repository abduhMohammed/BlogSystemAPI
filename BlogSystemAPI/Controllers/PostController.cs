using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        UnitWork unit;
        public PostController(UnitWork unit)
        {
            this.unit = unit;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<BlogPost> posts = unit.PostRepository.GetAll();

            if (posts.Count == 0)
                return NotFound();

            else
            {
                List<BlogPostDTO> blogPostDTOs = new List<BlogPostDTO>();   

                foreach(var p in posts)
                {
                    BlogPostDTO post = new BlogPostDTO()
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Content = p.Content,
                        Status = p.Status,
                    };
                    
                    blogPostDTOs.Add(post);
                }

                return Ok(blogPostDTOs);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            BlogPost post = unit.PostRepository.GetById(id);
 
            if (post == null) return NotFound();
            else
            {
                BlogPostDTO blogPostDTO = new BlogPostDTO()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    Status = post.Status
                };

                return Ok(blogPostDTO);
            }            
        }

        [HttpPost]
        public IActionResult Add([FromBody]BlogPostDTO PDTO)
        {
            if (PDTO == null) return NotFound();
            if(!ModelState.IsValid) return BadRequest(ModelState);

            BlogPost post = new BlogPost()
            {
                Id = PDTO.Id,
                Title = PDTO.Title,
                Content = PDTO.Content,
                Status = PDTO.Status
            };

            unit.PostRepository.Add(post);
            unit.PostRepository.Save();

            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody]BlogPostDTO PDTO, int id)
        {
            if(PDTO == null) return NotFound();
            var existingPost = unit.PostRepository.GetById(PDTO.Id);

            if(existingPost == null) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            existingPost.Id = PDTO.Id;
            existingPost.Title = PDTO.Title;
            existingPost.Content = PDTO.Content;
            existingPost.Status = PDTO.Status;

            unit.PostRepository.Update(existingPost);
            unit.PostRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            BlogPost post = unit.PostRepository.GetById(id);

            if(post==null) return NotFound();
            if(!ModelState.IsValid) return BadRequest(ModelState);  

            unit.PostRepository.Delete(post);
            unit.PostRepository.Save();

            return NoContent(); 
        }
    }
}