using System.ComponentModel.DataAnnotations;

namespace BlogSystemAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<BlogPost> blogPosts { get; set; }
    }
}