using BlogSystemAPI.DTO;
using BlogSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public CategoryController(CategoryService categoryService)
        {
            CategoryService = categoryService;
        }
        public CategoryService CategoryService { get; }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CategoryDTO> categoryDTOs = CategoryService.GetAll();
            return Ok(categoryDTOs);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            CategoryDTO categoryDTO = CategoryService.GetById(id);
            return Ok(categoryDTO);
        }

        [HttpPost]
        public IActionResult Add([FromBody]CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (categoryDTO == null) return BadRequest("Category can not be null");

            var created = CategoryService.Add(categoryDTO);
            return CreatedAtAction("GetById", new { id = categoryDTO.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var Updated = CategoryService.Update(id, categoryDTO);

            if (Updated == "Not Found") return NotFound();
            if (Updated == "No Changes") return Ok("No Changes were made to the category");

            return Ok(new AuthDTO { message = "Category Updated Successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = CategoryService.Delete(id);
            if (!deleted) return NotFound();

            return Ok(new AuthDTO { message = "Category Deleted Successfully" });
        }
    }
}
