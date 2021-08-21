using LittleBlog.Web.Data;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.DomainModels;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models.DtoModel;

namespace LittleBlog.Web.Services
{
    public class TagService : ITagService
    {
        private LittleBlogContext _db;

        public TagService(LittleBlogContext context)
        {
            _db = context;
        }

        public async Task DeleteAsync(string tagName)
        {
            var tag = await _db.Tags.FirstOrDefaultAsync(a=>a.Name.Equals(tagName));
            if(tag == null)
            {
                throw new Exception($"没有找到标签{tagName}");
            }

            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Tag>> ListAllAsync()
        {
            return await _db.Tags.AsNoTracking().ToListAsync();
        }

        public async Task SaveAsync(string tagName)
        {
            var tag = await _db.Tags.FirstOrDefaultAsync(a => a.Name.Equals(tagName));
            if (tag != null)
            {
                throw new Exception($"已存在标签{tagName}");
            }

            tag = new Tag()
            {
                Name = tagName,
                CreateTime = DateTime.Now,
            };

            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();
        }
    }
}
