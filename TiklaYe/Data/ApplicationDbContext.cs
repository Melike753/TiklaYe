using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Product>? Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(p => p.CreatedDate)
                    .HasColumnType("datetime2");

                // Diğer ayarlamalar...
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(c => c.CreatedDate)
                    .HasColumnType("datetime2");

                // Diğer ayarlamalar...
            });
        }
    }
}
