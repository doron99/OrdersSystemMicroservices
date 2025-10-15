using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
               .HasMany(o => o.OrderItems) // Order has many OrderItems
               .WithOne(oi => oi.Order) // OrderItem has one Order
               .HasForeignKey(oi => oi.OrderId) // Foreign key in OrderItem
               .OnDelete(DeleteBehavior.Cascade); // delete behavior - cascade delete

            modelBuilder.Entity<Order>()
              .Property(o => o.Version)
              .IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }
}
