using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Profiles;
using ECommerce.Api.Products.Providers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            //Arrange

            var dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProducts_ReturnsAllProducts))
                .Options;
            var dbContext = new ProductsDbContext(dbContextOptions);
            CreateProducts(dbContext);

            var mapCfg = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));

            var mapper = new Mapper(mapCfg);

            //Action

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var productsResult = await productsProvider.GetProductsAsync();

            //Assert

            Assert.True(productsResult.IsSuccess);
            Assert.Null(productsResult.Error);
            Assert.True(productsResult.Products.Any());
         

        }

        [Fact]
        public async Task GetProduct_ReturnsProductWithValidId()
        {
            //Arrange

            var dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProduct_ReturnsProductWithValidId))
                .Options;
            var dbContext = new ProductsDbContext(dbContextOptions);
            CreateProducts(dbContext);

            var mapCfg = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));

            var mapper = new Mapper(mapCfg);

            //Action

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var productsResult = await productsProvider.GetProductAsync(1);

            //Assert

            Assert.True(productsResult.IsSuccess);
            Assert.Null(productsResult.Error);
            Assert.NotNull(productsResult.Product);
            Assert.True(productsResult.Product.Id == 1);


        }

        [Fact]
        public async Task GetProduct_ReturnsNullWithInvalidId()
        {
            //Arrange

            var dbContextOptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProduct_ReturnsNullWithInvalidId))
                .Options;
            var dbContext = new ProductsDbContext(dbContextOptions);
            CreateProducts(dbContext);

            var mapCfg = new MapperConfiguration(cfg => cfg.AddProfile(new ProductProfile()));

            var mapper = new Mapper(mapCfg);

            //Action

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var productsResult = await productsProvider.GetProductAsync(-1);

            //Assert

            Assert.False(productsResult.IsSuccess);
            Assert.NotNull(productsResult.Error);
            Assert.Null(productsResult.Product);


        }


        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = new Random().Next(10, 100),
                    Price = (decimal)(new Random().NextDouble() * new Random().Next(10, 100))
                });
                dbContext.SaveChanges();
            }
        }
    }
}
