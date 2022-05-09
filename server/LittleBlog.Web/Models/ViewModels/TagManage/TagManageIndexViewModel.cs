using LittleBlog.Core.Models;
using System.Collections.Generic;

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
