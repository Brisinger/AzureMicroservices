using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Orders.Interfaces;

namespace ECommerce.API.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersProvider ordersProvider;

        public OrdersController(IOrdersProvider ordersProvider)
        {
            this.ordersProvider = ordersProvider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrdersAsync([FromRoute(Name = "id")] int customerId)
        {
            var (IsSuccess, orders, _) = await ordersProvider.GetOrdersAsync(customerId);
            if (IsSuccess)
            {
                return Ok(orders);
            }
            else
            {
                return NotFound();
            }
        }
    }
}