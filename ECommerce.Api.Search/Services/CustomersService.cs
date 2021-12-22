using ECommerce.Api.Search.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class CustomersService : ICustomersService
    {

        private ILogger<ICustomersService> _logger;
        private IHttpClientFactory _httpClientFactory;

        public CustomersService(IHttpClientFactory httpClientFactory, ILogger<ICustomersService> logger)
        {
            this._httpClientFactory = httpClientFactory;
            this._logger = logger;


        }

        public async Task<(bool IsSuccess, dynamic Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {

                var customerServiceClient = _httpClientFactory.CreateClient("CustomersService");
                var response = await customerServiceClient.GetAsync($"api/customers/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<dynamic>(content, options);

                    return (true, result, null);
                }

                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return (false, null, ex.Message);
            }
        }
    }
}
