using System;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Orders.Db
{
    public class OrdersDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }
    }
}
