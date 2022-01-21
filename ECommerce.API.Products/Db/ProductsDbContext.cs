using System;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Products.Db
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public ProductsDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
