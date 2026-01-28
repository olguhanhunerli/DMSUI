using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Customers;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerApiClient _customerApiClient;

        public CustomerManager(ICustomerApiClient customerApiClient)
        {
            _customerApiClient = customerApiClient;
        }

        public async Task<bool> CreateCustomer(CustomerCreateDTO createCustomerDTO)
        {
            return await _customerApiClient.CreateCustomer(createCustomerDTO);
        }

        public async Task<bool> DeleteCustomer(int id)
        {
           return await _customerApiClient.DeleteCustomer(id);
        }

        public async Task<CustomersItemDTO> GetCustomerById(int id)
        {
            return await _customerApiClient.GetCustomerById(id);
        }

        public async Task<PagedResultDTO<CustomersItemDTO>> GetCustomersPaging(int page, int pageSize)
        {
            return await _customerApiClient.GetCustomersPaging(page, pageSize);
        }

        public async Task<bool> UpdateCustomer(int id, UpdateCustomerDTO updateCustomerDTO)
        {
            return await _customerApiClient.UpdateCustomer(id, updateCustomerDTO);
        }
    }
}
