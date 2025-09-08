namespace BlogSystemAPI.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string? PostTitle { get; set; }
    }
}