using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class OrdersService : IOrdersService
    {

        private ILogger<IOrdersService> _logger;
        private IHttpClientFactory _httpClientFactory;
        public OrdersService(IHttpClientFactory httpClientFactory,ILogger<IOrdersService> logger)
        {
            this._httpClientFactory = httpClientFactory;
            this._logger = logger;

        }

        public async Task<(bool IsSuccess, IEnumerable<Order> Orders, string Error)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orderserviceClient = _httpClientFactory.CreateClient("OrdersService");
                var response = await orderserviceClient.GetAsync($"api/orders/{customerId}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();

                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                    var result = JsonSerializer.Deserialize <IEnumerable<Order>>(content, options);

                    return (true, result, null);
                }

                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return (false, null, ex.Message);
            }
        }
    }
}
