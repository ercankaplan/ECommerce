using ECommerce.Api.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSuccess, IEnumerable<ProductVM> Products, string Error)> GetProductsAsync();
        Task<(bool IsSuccess, ProductVM Product, string Error)> GetProductAsync(int id);
    }
}
