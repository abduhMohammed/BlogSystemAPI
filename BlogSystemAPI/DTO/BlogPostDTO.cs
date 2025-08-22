using Microsoft.VisualBasic;

namespace BlogSystemAPI.DTO
{
    public class BlogPostDTO
    {
        public int Id { get; set; }

        public String? Title { get; set; }

        public String Content { get; set; }

        public string? Status { get; set; }
    }
}