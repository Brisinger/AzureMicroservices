using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECommerce.API.Products.Models;


namespace ECommerce.API.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSuccess, IEnumerable<Product> products, string ErrorMessage)>
            GetProductsAsync();
        Task<(bool IsSuccess, Product product, string ErrorMessage)>
            GetProductAsync(int id);
    }
}
