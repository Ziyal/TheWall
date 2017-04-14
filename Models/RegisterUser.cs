using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class RegisterUser : BaseEntity
    {
        [Required]
        [MinLength(2)]
        [DisplayAttribute(Name = "First Name")]
        public string first_name { get; set; }

        [Required]
        [MinLength(2)]
        [DisplayAttribute(Name = "Last Name")]
        public string last_name { get; set; }


        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Confirm Password does not match")]
        [CompareAttribute("password")]
        [DisplayAttribute(Name = "Confirm Password")]
        public string PasswordC { get; set; }
    }
}