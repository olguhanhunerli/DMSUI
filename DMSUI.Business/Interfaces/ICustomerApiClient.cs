using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface ICustomerApiClient
    {
        Task<PagedResultDTO<CustomersItemDTO>> GetCustomersPaging(int page, int pageSize);
        Task<CustomersItemDTO> GetCustomerById(int id);
        Task<bool> UpdateCustomer(int id,UpdateCustomerDTO updateCustomerDTO);
        Task<bool> DeleteCustomer(int id);
        Task<bool> CreateCustomer(CustomerCreateDTO createCustomerDTO); 
    }
}
