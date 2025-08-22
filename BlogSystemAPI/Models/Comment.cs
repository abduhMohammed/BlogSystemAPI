using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystemAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DueDate CreatedAt { get; set; }

        [ForeignKey("BlogPost")]
        public int BlogPostID { get; set; }

        public BlogPost BlogPost { get; set; }
    }
}
