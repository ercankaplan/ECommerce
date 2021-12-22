using ECommerce.Api.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IProductsService
    {
        Task<(bool IsSuccess, Product Product, string Error)> GetProductAsync(int id);

        Task<(bool IsSuccess, IEnumerable<Product> Products, string Error)> GetProductsAsync();
    }
}
