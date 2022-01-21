using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Products.Providers;
using AutoMapper;

namespace ECommerce.API.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<Db.ProductsDbContext>()
                .UseInMemoryDatabase<Db.ProductsDbContext>(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new Db.ProductsDbContext(options);
            var productProfile = new Profiles.ProductProfile();
            var configuration = new MapperConfiguration(cfe => cfe.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            CreateProductsAsync(dbContext).Wait();
            var productsProvider = new ProductsProvider(dbContext, default, mapper);
            var (IsSuccess, Products, ErrorMessage) = await productsProvider.GetProductsAsync();

            //Assertion.
            Assert.True(IsSuccess);
            Assert.True(Products.Any<Models.Product>());
            Assert.Equal(string.Empty, ErrorMessage);
        }
        [Fact]
        public async Task GetProductReturnsProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<Db.ProductsDbContext>()
                .UseInMemoryDatabase<Db.ProductsDbContext>(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new Db.ProductsDbContext(options);
            var productProfile = new Profiles.ProductProfile();
            var configuration = new MapperConfiguration(cfe => cfe.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            CreateProductsAsync(dbContext).Wait();
            var productsProvider = new ProductsProvider(dbContext, default, mapper);
            var (IsSuccess, Product, ErrorMessage) = await productsProvider.GetProductAsync(1);

            //Assertion.
            Assert.True(IsSuccess);
            Assert.NotNull(Product);
            Assert.Equal(1, Product.Id);
            Assert.Equal(string.Empty, ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnsProductUsingInValidId()
        {
            var options = new DbContextOptionsBuilder<Db.ProductsDbContext>()
                .UseInMemoryDatabase<Db.ProductsDbContext>(nameof(GetProductsReturnsAllProducts))
                .Options;
            var dbContext = new Db.ProductsDbContext(options);
            var productProfile = new Profiles.ProductProfile();
            var configuration = new MapperConfiguration(cfe => cfe.AddProfile(productProfile));
            var mapper = new Mapper(configuration);
            CreateProductsAsync(dbContext).Wait();
            var productsProvider = new ProductsProvider(dbContext, default, mapper);
            var (IsSuccess, Product, ErrorMessage) = await productsProvider.GetProductAsync(-1);

            //Assertion.
            Assert.False(IsSuccess);
            Assert.Null(Product);
            Assert.Equal("Not found", ErrorMessage);
        }
        private async Task CreateProductsAsync(Db.ProductsDbContext dbContext)
        {
            if (!dbContext.Products.Any<Db.Product>())
            {
                for (int i = 1; i < 10; i++)
                {
                    dbContext.Products.Add(new Db.Product { Id = i, Name = Guid.NewGuid().ToString(), Inventory = i + 10, Price = (decimal)(i * 3.14) });
                }
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
