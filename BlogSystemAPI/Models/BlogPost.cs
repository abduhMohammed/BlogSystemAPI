using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystemAPI.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        public String? Title { get; set; }

        public String Content { get; set; }

        public DueDate CreatedAt { get; set; }

        public DueDate UpdatedAt { get; set; }

        public string? Status { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public Category? Category { get; set; }

        public List<Comment> comments { get; set; } = new List<Comment>();
    }
}