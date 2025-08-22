using System.ComponentModel.DataAnnotations;

namespace BlogSystemAPI.DTO
{
    public class LoginDTO
    {
        public string Username{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password{ get; set; }
    }
}