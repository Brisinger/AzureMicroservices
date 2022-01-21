using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Search.Interfaces;
using ECommerce.API.Search.Models;

namespace ECommerce.API.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordersService,
            IProductsService productsService,
            ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var (IsSuccess, OrdersResult, ErrorMessage) = await ordersService.GetOrdersAsync(customerId);
            var (IsProductResponseSuccess, ProductsResult, ProductErrorMessage) = await productsService.GetProductsAsync();
            var (IsCustomerResponseSuccess, CustomersResult, CustomerErrorMessage) = await customersService.GetCustomerAsync(customerId);
            if (IsSuccess)
            {
                foreach (var order in OrdersResult)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = IsProductResponseSuccess ? ProductsResult?
                            .FirstOrDefault<Product>(p => p.Id == item.ProductId)?.Name
                            : "Product information is not available";
                    }
                }
                var result = new
                {
                    Customer = IsCustomerResponseSuccess ?
                                CustomersResult : new { Name = "Customer information is not available" },
                    Orders = OrdersResult,
                };
                return (IsSuccess, result);
            }
            return (IsSuccess, null);
        }
    }
}
