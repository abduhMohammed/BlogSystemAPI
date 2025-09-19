using System.ComponentModel.DataAnnotations;

namespace BlogSystemAPI.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Display(Name ="Category Name")]
        [Required]
        public string? Name { get; set; }
    }
}
