using System.ComponentModel.DataAnnotations;

namespace BlogSystemAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        List<BlogPost> blogPosts { get; set; } = new List<BlogPost>();
    }
}