using LittleBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LittleBlog.Core.Common
{
    /// <summary>
    /// 友情链接帮助类
    /// </summary>
    public static class FriendshipLinkGroupsHelper
    {
        public const string BlogGroupName = "博客";
        public const string DocsGroupName = "文档资源";
        public const string ProjectGgroupName = "开源项目";

        public static List<KeyValuePair<FriendshipLinkGroups, string>> Relationships { get; } = new List<KeyValuePair<FriendshipLinkGroups, string>>()
        {
            new KeyValuePair<FriendshipLinkGroups, string>(FriendshipLinkGroups.Blog, BlogGroupName),
            new KeyValuePair<FriendshipLinkGroups, string>(FriendshipLinkGroups.Docs, DocsGroupName),
            new KeyValuePair<FriendshipLinkGroups, string>(FriendshipLinkGroups.Project, ProjectGgroupName)
        };

        public static Dictionary<FriendshipLinkGroups, string> GetGroupNames(List<FriendshipLinkGroups> groups)
        {
            var datas = from @group in groups.Distinct()
                        join relationship in Relationships on @group equals relationship.Key into groupNames
                        from groupName in groupNames.DefaultIfEmpty()
                        select new
                        {
                            @group,
                            groupName
                        };

            return datas.GroupBy(a => a.group).ToDictionary(x=>x.Key, y=>y.FirstOrDefault().groupName.Value);
        }

        public static string GetGroupName(FriendshipLinkGroups group)
        {
            return Relationships.Where(a => a.Key.Equals(group)).Select(a => a.Value).FirstOrDefault();
        }
    }
}
