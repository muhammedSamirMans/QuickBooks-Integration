using Karage.Domain.Common.DTOs;
using Karage.Domain.Common; 

namespace Karage.Domain.Interfaces
{
    public interface ICustomerService
    {
        #region Customers
        Task<List<CustomerVM>> readAll();
        Task<CustomerVM> readById(int customerId);
        Task<ResponseVM> Create(CustomerDTO customerDTO);
        #endregion
    }
}
