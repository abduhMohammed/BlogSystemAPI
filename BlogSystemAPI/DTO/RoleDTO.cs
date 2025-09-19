using System.ComponentModel.DataAnnotations;

namespace BlogSystemAPI.DTO
{
    public class RoleDTO
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string Role { get; set; }
    }
}