using System.ComponentModel.DataAnnotations;

namespace LittleBlog.Core.Models
{
    public class PersonalInfo
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(255)]
        public string BlogOwner { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}
