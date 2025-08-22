namespace BlogSystemAPI.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        List<BlogPost> blogPosts { get; set; } = new List<BlogPost>();
    }
}