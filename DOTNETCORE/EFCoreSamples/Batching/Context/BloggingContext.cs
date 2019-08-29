using Batching.Models;
using Microsoft.EntityFrameworkCore;

namespace Batching.Context
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions<BloggingContext> options): base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=Demo.Batching;Integrated Security=true;";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
                //optionsBuilder.UseSqlServer(connectionString,options => options.MaxBatchSize(2));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}