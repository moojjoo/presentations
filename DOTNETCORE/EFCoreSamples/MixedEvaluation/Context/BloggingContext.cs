using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MixedEvaluation.Models;

namespace MixedEvaluation.Context
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public BloggingContext()
        {
        }

        public BloggingContext(DbContextOptions<BloggingContext> options) : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=Demo.MixedEvaluation;Integrated Security=true;";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(connectionString)
                    .ConfigureWarnings(warnings
                        => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}