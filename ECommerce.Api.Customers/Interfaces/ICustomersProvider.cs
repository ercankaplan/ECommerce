using ECommerce.Api.Customers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Interfaces
{
    public interface ICustomersProvider
    {
        Task<(bool IsSuccess, IEnumerable<CustomerVM> Customers, string Error)> GetCustomersAsync();
        Task<(bool IsSuccess, CustomerVM Customer, string Error)> GetCustomerAsync(int id);
    }
}
