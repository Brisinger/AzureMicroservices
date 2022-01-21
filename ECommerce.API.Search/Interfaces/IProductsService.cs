using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECommerce.API.Search.Models;

namespace ECommerce.API.Search.Interfaces
{
    public interface IProductsService
    {
        Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)>
             GetProductsAsync();
    }
}
