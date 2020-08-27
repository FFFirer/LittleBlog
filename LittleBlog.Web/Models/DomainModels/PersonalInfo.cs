using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DomainModels
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
