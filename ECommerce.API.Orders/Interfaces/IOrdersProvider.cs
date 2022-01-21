using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ECommerce.API.Orders.Interfaces
{
    public interface IOrdersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Models.Order> orders, string ErrorMessage)>
            GetOrdersAsync(int customerId);
    }
}
