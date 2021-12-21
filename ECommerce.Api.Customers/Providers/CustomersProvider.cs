using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {

        private readonly CustomerDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;
        public CustomersProvider(CustomerDbContext dbContext, ILogger<CustomersProvider> logger,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Customer() { Id = 1 , Name = "Ercan Kaplan" , Address = "Ankara" });
                dbContext.Customers.Add(new Customer() { Id = 2, Name = "Defne Öcal", Address = "Kastamonu" });
                dbContext.Customers.Add(new Customer() { Id = 3, Name = "Ali Berkan Kaplan", Address = "İstanbul" });
                dbContext.Customers.Add(new Customer() { Id = 4, Name = "Ömer Seyfettin", Address = "Bursa" });
                dbContext.Customers.Add(new Customer() { Id = 5, Name = "Salih Tuna", Address = "İzmit" });
                dbContext.SaveChanges();
            }
        }


        public async Task<(bool IsSuccess, IEnumerable<CustomerVM> Customers, string Error)> GetCustomersAsync()
        {
            try
            {
                var customers = await dbContext.Customers.ToListAsync();

                if (customers.Any())
                {
                    logger?.LogInformation("Customers Found");

                    var customerVMs = mapper.Map<List<Customer>, List<CustomerVM>>(customers);

                    return (true, customerVMs, null);

                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.StackTrace);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, CustomerVM Customer, string Error)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await dbContext.Customers.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (customer != null)
                {
                    logger?.LogInformation("Customer Found");

                    CustomerVM customerVM = mapper.Map<Customer, CustomerVM>(customer);
                    return (true, customerVM, null);
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
