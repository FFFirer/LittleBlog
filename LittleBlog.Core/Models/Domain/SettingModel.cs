using System.ComponentModel.DataAnnotations;

namespace LittleBlog.Core.Models
{
    public class SettingModel
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Key { get; set;  }

        [Required]
        [StringLength(500)]
        public string Value { get; set; }


        public string Description { get; set;  }

        public string Section { get; set; }
        public string SubSection { get; set; }
    }
}
