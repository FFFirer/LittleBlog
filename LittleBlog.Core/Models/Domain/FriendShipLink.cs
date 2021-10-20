using System.ComponentModel.DataAnnotations;

namespace LittleBlog.Core.Models
{
    public class FriendShipLink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } 

        [Required]
        [MaxLength(255)]
        public string link { get; set; }
    }
}
