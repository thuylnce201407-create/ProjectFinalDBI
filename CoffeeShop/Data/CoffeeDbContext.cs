using CoffeeShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Data
{
    public class CoffeeDbContext : DbContext
    {
        public CoffeeDbContext(DbContextOptions<CoffeeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .ToTable("Menu");

            modelBuilder.Entity<Category>()
                .ToTable("Category");

            modelBuilder.Entity<Employee>()
        .ToTable("Employee");

            modelBuilder.Entity<Customer>()
            .ToTable("Customer");

            modelBuilder.Entity<Order>()
            .ToTable("Orders");

            modelBuilder.Entity<OrderDetail>()
            .ToTable("OrderDetail");
        }
    }
}