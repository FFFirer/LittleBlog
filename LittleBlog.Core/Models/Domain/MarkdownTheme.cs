﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Models.Domain
{

    public class MarkdownTheme
    {
        [Key]
        [MaxLength(100)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string PhysicalPath { get; set; }
        public string Content { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? LastEditTime { get; set; }
    }
}
