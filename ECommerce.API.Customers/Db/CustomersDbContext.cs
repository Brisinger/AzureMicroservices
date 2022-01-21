using System;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Customers.Db
{
    public class CustomersDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public CustomersDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
