using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.API.Orders.Interfaces;
using ECommerce.API.Orders.Db;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace ECommerce.API.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any<Order>())
            {
                dbContext.Orders.Add(new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now.AddDays(-10),
                    Total = 200,
                    Items = new List<OrderItem> {
                    new OrderItem { OrderId = 1, ProductId = 1, Quantity = 5, UnitPrice = 20 },
                    new OrderItem { OrderId = 1, ProductId = 2, Quantity = 20, UnitPrice = 5}
                }
                });

                dbContext.Orders.Add(new Order
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Now.AddDays(-8),
                    Total = 700,
                    Items = new List<OrderItem> {
                    new OrderItem { OrderId = 2, ProductId = 3, Quantity = 2, UnitPrice = 150 },
                    new OrderItem { OrderId = 2, ProductId = 4, Quantity = 2, UnitPrice = 200}
                }
                });

                dbContext.Orders.Add(new Order
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.Now.AddDays(-5),
                    Total = 205,
                    Items = new List<OrderItem> {
                    new OrderItem { OrderId = 3, ProductId = 2, Quantity = 1, UnitPrice = 5 },
                    new OrderItem { OrderId = 3, ProductId = 4, Quantity = 1, UnitPrice = 200}
                }
                });

                dbContext.Orders.Add(new Order
                {
                    Id = 4,
                    CustomerId = 3,
                    OrderDate = DateTime.Now,
                    Total = 225,
                    Items = new List<OrderItem> {
                    new OrderItem { OrderId = 4, ProductId = 2, Quantity = 1, UnitPrice = 5 },
                    new OrderItem { OrderId = 4, ProductId = 4, Quantity = 1, UnitPrice = 200},
                    new OrderItem { OrderId = 4, ProductId = 1, Quantity = 1, UnitPrice = 20}
                }
                });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> orders, string ErrorMessage)>
            GetOrdersAsync(int customerId)
        {
            try
            {
                logger?.LogInformation("Querrying Orders.");
                var orders = await Task<IEnumerable<Order>>.Factory
                    .StartNew(() =>
                    dbContext.Orders.Where<Order>(order => order.CustomerId == customerId)
                    .Include(order => order.Items)
                    .ToList<Order>()
                    );
                if (orders != default && orders.Any<Order>())
                {
                    logger?.LogInformation($"{orders.Count<Order>()} Order(s) found for customer id: {customerId}");
                    var result = mapper.Map<IEnumerable<Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, string.Empty);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message)
;
            }
        }
    }
}
