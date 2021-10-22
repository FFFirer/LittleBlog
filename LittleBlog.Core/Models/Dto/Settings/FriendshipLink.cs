using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleBlog.Core.Models
{
    /// <summary>
    /// 友情链接
    /// </summary>
    public class FriendshipLink
    {
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set;}

        /// <summary>
        /// 链接
        /// </summary>
        [JsonProperty("link")]
        public string Link { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        [JsonProperty("group")]
        public string Group { get; set; }
    }
}
