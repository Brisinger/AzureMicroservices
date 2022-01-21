using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ECommerce.API.Products.Interfaces;
using ECommerce.API.Products.Db;
using Microsoft.Extensions.Logging;
using AutoMapper;


namespace ECommerce.API.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        public async Task<(bool IsSuccess, Models.Product product, string ErrorMessage)>
            GetProductAsync(int id)
        {
            try
            {
                Product product = await dbContext.Products.FindAsync(id);
                if (product != default)
                {
                    var result = mapper.Map<Product, Models.Product>
                        (product);
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

        public async Task<(bool IsSuccess, IEnumerable<Models.Product> products, string ErrorMessage)>
            GetProductsAsync()
        {
            try
            {
                logger?.LogInformation("Querrying Products.");
                var products = await Task.Factory
                    .StartNew<IEnumerable<Product>>(() => dbContext.Products.ToList<Product>());
                if (products != default && products.Any<Product>())
                {
                    logger?.LogInformation($"Total products in the inventory: {products.Count<Product>()}");
                    var result = mapper.Map<IEnumerable<Product>, IEnumerable<Models.Product>>
                        (products);
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

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Product { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
                dbContext.Products.Add(new Product { Id = 2, Name = "Mouse", Price = 5, Inventory = 200 });
                dbContext.Products.Add(new Product { Id = 3, Name = "Monitor", Price = 150, Inventory = 100 });
                dbContext.Products.Add(new Product { Id = 4, Name = "CPU", Price = 200, Inventory = 2000 });
                dbContext.SaveChanges();
            }
        }
    }
}
