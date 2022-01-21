using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECommerce.API.Customers.Models;


namespace ECommerce.API.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<Customer> customers, string ErrorMessage)>
            GetCustomersAsync();

        Task<(bool IsSuccess, Customer customer, string ErrorMessage)>
            GetCustomerAsync(int id);

    }
}
