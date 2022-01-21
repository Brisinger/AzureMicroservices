using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECommerce.API.Search.Models;


namespace ECommerce.API.Search.Interfaces
{
    public interface IOrdersService
    {
        Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)>
             GetOrdersAsync(int customerId);
    }
}
