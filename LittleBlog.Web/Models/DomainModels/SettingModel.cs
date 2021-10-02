using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DomainModels
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
