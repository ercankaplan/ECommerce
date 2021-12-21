using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private IOrdersService mOrderService;

        private IProductsService mProductsService;
        public SearchService(IOrdersService ordersService, IProductsService productsService)
        {
            this.mOrderService = ordersService;
            this.mProductsService = productsService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int CustomerId)
        {
            var orderResult = await mOrderService.GetOrdersAsync(CustomerId);

            if (orderResult.IsSuccess)
            {
                foreach (var ord in orderResult.Orders)
                {
                    foreach (var ordItem in ord.Items)
                    {

                        var result = await mProductsService.GetProductAsync(ordItem.ProductId);

                        if (result.IsSuccess)
                            ordItem.ProductName = result.Product.Name;
                    }
                }

                return (true, new { orderResult.Orders });
            }

            return (false, null);
        }
    }
}
