using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public string comment { get; set; }

        [Required]
        public int id { get; set; }
    }
}