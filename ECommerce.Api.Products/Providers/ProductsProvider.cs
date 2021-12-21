using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDbContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;
        public ProductsProvider(ProductsDbContext dbContext, ILogger<ProductsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Product() { Id = 1 , Name = "Keyboard", Price=20, Inventory = 100 });
                dbContext.Products.Add(new Product() { Id = 2, Name = "Mouse", Price = 5, Inventory = 200 });
                dbContext.Products.Add(new Product() { Id = 3, Name = "Monitor", Price = 150, Inventory = 100 });
                dbContext.Products.Add(new Product() { Id = 4, Name = "CPU", Price = 200, Inventory = 2000 });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ProductVM> Products, string Error)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();

                if (products.Any())
                {
                    var productVMs = mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(products);

                    return (true, productVMs, null);
                }
                return (false, null,  "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async  Task<(bool IsSuccess, ProductVM Product, string Error)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.Where(o => o.Id == id).SingleOrDefaultAsync();

                if(product !=null)
                {
                    var productVM = mapper.Map<Product, ProductVM>(product);
                    return (true, productVM, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
            
        }
    }
}
