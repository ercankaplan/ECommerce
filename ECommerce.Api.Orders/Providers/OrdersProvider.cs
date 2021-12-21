using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {

        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;
        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                List<OrderItem> orderList = new List<OrderItem>();

                orderList.Add(new OrderItem() { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, UnitPrice = 5.5M });
                orderList.Add(new OrderItem() { Id = 2, OrderId = 1, ProductId = 2, Quantity = 1, UnitPrice = 0.8M });


                dbContext.Orders.Add(new Order() { Id = 1, CustomerId = 1, Items = orderList, OrderDate = new DateTime(2021, 1, 1) });
                dbContext.OrderItems.AddRange(orderList);


                List<OrderItem>  orderList2 = new List<OrderItem>();

                orderList2.Add(new OrderItem() { Id = 3, OrderId = 2, ProductId = 3, Quantity = 10, UnitPrice = 4 });
                orderList2.Add(new OrderItem() { Id = 4, OrderId = 2, ProductId = 1, Quantity = 20, UnitPrice = 12 });

                dbContext.Orders.Add(new Order() { Id = 2, CustomerId = 2, Items = orderList2, OrderDate = new DateTime(2021, 5, 4) });
                dbContext.OrderItems.AddRange(orderList2);

                dbContext.SaveChanges();
            }
        }


        public async Task<(bool IsSuccess, IEnumerable<OrderVM> Orders, string Error)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();

                if (orders.Any())
                {

                    foreach (var o in orders)
                    {
                        o.Items = dbContext.OrderItems.Where(x => x.OrderId == o.Id).ToList();
                    }

                    var orderVMs = mapper.Map<List<Order>, List<OrderVM>>(orders);

                    return (true, orderVMs, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return (false, null, ex.Message);
            }
        }
    }
}
