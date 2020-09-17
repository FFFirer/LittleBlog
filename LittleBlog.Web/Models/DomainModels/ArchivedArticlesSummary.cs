using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBlog.Web.Models
{
    public class ArchivedArticlesSummary
    {
        [NotMapped]
        public string DisplayArchiveDate { get; set; }
        public string ArchiveDate { get; set; }
        public int ArticlesCount { get; set; }
    }
}
