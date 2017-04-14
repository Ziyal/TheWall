using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class Message : BaseEntity
    {

        [Key]
        public int message_id { get; set; }

        [Key]
        public int comments_message_id { get; set; }

        [Required]
        public string message { get; set; }

    }
}