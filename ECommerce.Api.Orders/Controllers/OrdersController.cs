using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrdersProvider mOrdersProvider;
        public OrdersController(IOrdersProvider ordersProvider)
        {
            mOrdersProvider = ordersProvider;
        }

        [HttpGet("{cutomerId}")]
        public async Task<IActionResult> GetCustomerOrders(int cutomerId)
        {
            var result = await mOrdersProvider.GetOrdersAsync(cutomerId);

            if (result.IsSuccess)
            {

                return Ok(result.Orders);
            }

            return NotFound();
        }
    }
}
