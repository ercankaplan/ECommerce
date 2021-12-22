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
        private ICustomersService mCustomersService;
        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
        {
            this.mOrderService = ordersService;
            this.mProductsService = productsService;
            this.mCustomersService = customersService;
        }

        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int CustomerId)
        {
            var orderResult = await mOrderService.GetOrdersAsync(CustomerId);

            var productsResult = await mProductsService.GetProductsAsync();

            var customerResult = await mCustomersService.GetCustomerAsync(CustomerId);

            if (orderResult.IsSuccess)
            {
                foreach (var ord in orderResult.Orders)
                {
                    foreach (var ordItem in ord.Items)
                    {

                        //var result = await mProductsService.GetProductAsync(ordItem.ProductId);

                        //if (result.IsSuccess)
                        //    ordItem.ProductName = result.Product.Name;

                        ordItem.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(x => x.Id == ordItem.ProductId)?.Name :
                            "!!! Product information is not available";
                    }
                }

                var searchResult = new
                {
                    Customer = customerResult.IsSuccess ? customerResult.Customer : new { Name = "!! Customer information is not available" },
                    Orders = orderResult.Orders
                };

                return (true, searchResult );
            }

            return (false, null);
        }
    }
}
