using LittleBlog.Core.Models;
using System;
using System.Collections.Generic;

namespace LittleBlog.Web.Mock
{
    public class MockData
    {
        public static MockData Instance { get; } = new MockData();
        public List<Article> articles { get; set; }
        public MockData()
        {
            articles = new List<Article>();
            articles.Add(new Article
            {
                Id = 1,
                Title = "Test Title 1",
                Content = @"<h1>Test Content One</h1>",
                Author = "Tom",
                SavePath = string.Empty,
                CreateTime = DateTime.Now.AddDays(-1),
                LastEditTime = DateTime.Now.AddHours(-1)
            });

            articles.Add(new Article
            {
                Id = 1,
                Title = "Test Title 2",
                Content = @"<h1>Test Content Two</h1>",
                Author = "Jack",
                SavePath = string.Empty,
                CreateTime = DateTime.Now.AddDays(-1),
                LastEditTime = DateTime.Now.AddHours(-1)
            });
            articles.Add(new Article
            {
                Id = 1,
                Title = "Test Title 3",
                Content = @"<h1>Test Content Three</h1>",
                Author = "Helen",
                SavePath = string.Empty,
                CreateTime = DateTime.Now.AddDays(-1),
                LastEditTime = DateTime.Now.AddHours(-1)
            });
            articles.Add(new Article
            {
                Id = 1,
                Title = "Test Title 4",
                Content = @"<h1>Test Content Four</h1>",
                Author = "Black",
                SavePath = string.Empty,
                CreateTime = DateTime.Now.AddDays(-1),
                LastEditTime = DateTime.Now.AddHours(-1)
            }); ;
        }
    }
}
