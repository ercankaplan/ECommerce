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
    public class ProductsService : IProductsService
    {

        private ILogger<IProductsService> _logger;
        private IHttpClientFactory _httpClientFactory;
        public ProductsService(IHttpClientFactory httpClientFactory, ILogger<IProductsService> logger)
        {
            this._httpClientFactory = httpClientFactory;
            this._logger = logger;

        }
 
        public async Task<(bool IsSuccess, Product Product, string Error)> GetProductAsync(int id)
        {
            try
            {
                var productServiceClient = _httpClientFactory.CreateClient("ProductsService");
                var response = await productServiceClient.GetAsync($"api/products/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                    var result = JsonSerializer.Deserialize<Product>(content, options);


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
