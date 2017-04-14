using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class LoginUser : BaseEntity
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}