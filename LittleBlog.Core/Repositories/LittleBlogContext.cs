using LittleBlog.Core.Models;
using LittleBlog.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LittleBlog.Core
{
    public class LittleBlogContext : IdentityDbContext<LittleBlogIdentityUser>
    {
        public LittleBlogContext(DbContextOptions<LittleBlogContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LittleBlogIdentityUser>(entity => entity.Property(m => m.Id).HasMaxLength(127));
            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(127));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(127));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(127));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(m => m.RoleId).HasMaxLength(127));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.UserId).HasMaxLength(127));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(127));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(m => m.Name).HasMaxLength(127));
            builder.Entity<ArchivedArticlesSummary>(aas => aas.HasNoKey());

            builder.Entity<ArticleCategory>().HasKey(a => new {a.ArticleId, a.CategoryName});
            builder.Entity<ArticleTag>().HasKey(a => new { a.ArticleId, a.TagName });

            builder.Entity<SettingModel>();

            base.OnModelCreating(builder);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }

        //public DbSet<FriendShipLink> FriendShipLinks { get; set; }
        //public DbSet<BlogBasicInfo> BlogBasicInfos { get; set; }

        public DbSet<SettingModel> SettingModels { get; set; }

        #region 无键实体类
        public DbSet<ArchivedArticlesSummary> ArchivedArticlesSummaries { get; set; }

        #endregion

    }
}
