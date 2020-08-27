using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels.TagManage
{
    public class TagManageIndexViewModel
    {
        public TagManageIndexViewModel()
        {
            Tags = new List<Tag>();
        }

        public List<Tag> Tags { get; set; }
    }
}
