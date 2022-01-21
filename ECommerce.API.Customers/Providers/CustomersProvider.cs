using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ECommerce.API.Customers.Interfaces;
using ECommerce.API.Customers.Db;
using AutoMapper;


namespace ECommerce.API.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext dbContext,
            ILogger<CustomersProvider> logger, IMapper mapper
            )
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Customer { Id = 1, Name = "Jessica Smith", Address = "20 Elm St." });
                dbContext.Customers.Add(new Customer { Id = 2, Name = "John Smith", Address = "20 Main St." });
                dbContext.Customers.Add(new Customer { Id = 3, Name = "Willam Johnson", Address = "100 10th St." });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess,
            IEnumerable<Models.Customer> customers,
            string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                logger?.LogInformation("Querying Customers");
                var customers = await Task.Factory.StartNew<IEnumerable<Customer>>(()
                    => dbContext.Customers.ToList<Customer>()
                );
                if (customers != null && customers.Any())
                {
                    logger?.LogInformation($"{customers.Count<Customer>()} customers(s) found.");
                    var result = mapper.Map<IEnumerable<Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, string.Empty);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess,
            Models.Customer customer,
            string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                logger?.LogInformation("Querying Customers");
                var customer = await Task.Factory.StartNew<Customer>(()
                    => dbContext.Customers.Where<Customer>(c => c.Id == id)
                        .FirstOrDefault<Customer>()
                );
                if (customer != null)
                {
                    logger?.LogInformation($"customer with id: {id} found.");
                    var result = mapper.Map<Customer, Models.Customer>(customer);
                    return (true, result, string.Empty);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
