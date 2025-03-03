using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using Karage.Application.Interfaces;
using Karage.Domain.Common.DTOs;
using Karage.Domain.Common;
using Karage.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karage.Infrastructure.Services
{
    public class CustomerService: ICustomerService
    {
        private readonly IQuickBooksService quickBooksService;
        public CustomerService(IQuickBooksService quickBooksService)
        {
            this.quickBooksService = quickBooksService;
        }

        public async Task<ResponseVM> Create(CustomerDTO customerDTO)
        {
            ResponseVM responseVM = new ResponseVM();
            try
            {
                var serviceContext = await quickBooksService.GetServiceContextAsync();
                var dataService = new DataService(serviceContext);

                #region Create customer
                // Create a new customer object
                var newCustomer = new Customer
                {
                    DisplayName = customerDTO.Name, 
                    Balance = customerDTO.Balance
                }; 
                #endregion

                // 🔥 Send the Customer to QuickBooks
                Customer createdCustomer = dataService.Add(newCustomer);

                var result = new CustomerVM
                {
                    Id = createdCustomer.Id,
                    Name = createdCustomer.DisplayName, 
                    Balance = createdCustomer.Balance 
                };
                responseVM.Status = 200;
                responseVM.Data = result;
                responseVM.Message = "Success";
                return responseVM;
            }
            catch (Exception ex)
            {
                responseVM.Status = 500;
                responseVM.Message = ex.Message;
                return responseVM;
            }
        }

        public async Task<List<CustomerVM>> readAll()
        {
            var serviceContext = await quickBooksService.GetServiceContextAsync();
            // Use QueryService with manually set token
            var queryService = new QueryService<Customer>(serviceContext);

            try
            {
                List<Customer> customers = queryService.ExecuteIdsQuery("SELECT * FROM Customer Order by Id desc").ToList();
                // Map to ViewModel
                return customers.Select(customer => new CustomerVM
                {
                    Id = customer.Id,
                    Name = customer.DisplayName,
                    Balance = customer.Balance 
                }).ToList();

            }
            catch (Exception ex)
            {

                // Map to ViewModel
                return null;
            }

        }

        public Task<CustomerVM> readById(int customerId)
        {
            throw new NotImplementedException();
        }
    }
}
