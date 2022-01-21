using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Customers.Interfaces;


namespace ECommerce.API.Customers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersProvider customersProvider;

        public CustomersController(ICustomersProvider customersProvider)
        {
            this.customersProvider = customersProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var (IsSuccess, customers, _) = await customersProvider.GetCustomersAsync();
            if (IsSuccess)
            {
                return Ok(customers);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var (IsSuccess, customer, _) = await customersProvider.GetCustomerAsync(id);
            if (IsSuccess)
            {
                return Ok(customer);
            }
            else
            {
                return NotFound();
            }
        }
    }
}