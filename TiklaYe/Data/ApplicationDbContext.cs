using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TiklaYe.Models;

namespace TiklaYe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category için CreatedDate özelliğinin datetime2 olarak tanımlanması
            modelBuilder.Entity<Category>()
                .Property(c => c.CreatedDate)
                .HasColumnType("datetime2");
        }
    }
}