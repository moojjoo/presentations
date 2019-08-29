using System.ComponentModel.DataAnnotations.Schema;
using FromSQLDbQuery.Models;
using Microsoft.EntityFrameworkCore;

namespace FromSQLDbQuery.Context
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        [NotMapped]
        public DbSet<ShortBlog> ShortBlogs { get; set; }

        public DbQuery<ShortBlogQuery> ShortBlogsQuery { get; set; }

        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions<BloggingContext> options) : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=Demo.FromSQL;Integrated Security=true;";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortBlog>().HasKey(x => x.BlogId);
            //modelBuilder.Query<ShortBlogQuery>().ToView("ViewName");
        }
    }
}