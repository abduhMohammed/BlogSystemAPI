using System.ComponentModel.DataAnnotations;

namespace BlogSystemAPI.DTO
{
    public class RegisterDTO
    {
        [Required]
        [Display(Name= "Username")]
        public string? UserName{ get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}