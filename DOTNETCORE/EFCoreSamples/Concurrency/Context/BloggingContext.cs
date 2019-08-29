using System;
using Concurrency.Models;
using Microsoft.EntityFrameworkCore;

namespace Concurrency.Context
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=(localdb)\mssqllocaldb;Database=Demo.Concurrency;Integrated Security=true;";
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}