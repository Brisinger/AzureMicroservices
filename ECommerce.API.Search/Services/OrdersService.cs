using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ECommerce.API.Search.Interfaces;
using ECommerce.API.Search.Models;

namespace ECommerce.API.Search.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ILogger<OrdersService> logger;

        public OrdersService(IHttpClientFactory clientFactory, ILogger<OrdersService> logger)
        {
            this.clientFactory = clientFactory;
            this.logger = logger;
        }

        public async Task<(bool IsSuccess, IEnumerable<Order> Orders, string ErrorMessage)>
            GetOrdersAsync(int customerId)
        {
            try
            {
                var client = clientFactory.CreateClient("OrdersService");
                var response = await client.GetAsync($"api/orders/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<IEnumerable<Order>>(content, options);
                    return (true, result, string.Empty);
                }
                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
