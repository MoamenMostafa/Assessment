using Management.DL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.DL.Context
{
    public class ApplicationDBContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _configuration.GetConnectionString("DefaultConnection"),
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,             
                    maxRetryDelay: TimeSpan.FromSeconds(30), 
                    errorNumbersToAdd: null       
                )
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2); 
        }
    }
}


