using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ECommerce.Context
{
  

    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
        : base(options) { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderNoSeq> OrderNoSeqs { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=ecommerce.db");
            }
        }
    }

}
