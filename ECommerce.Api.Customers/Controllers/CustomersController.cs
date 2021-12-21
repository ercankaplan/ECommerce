using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersProvider mCustomersProvider;

        public CustomersController(ICustomersProvider customersProvider)
        {
            mCustomersProvider = customersProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await  mCustomersProvider.GetCustomersAsync();

            if (result.IsSuccess)
                return Ok(result.Customers);

            return NotFound();

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var result = await mCustomersProvider.GetCustomerAsync(id);

            if (result.IsSuccess)
                return Ok(result.Customer);

            return NotFound();
        }
    }
}
