using DMSUI.Entities.DTOs.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business.Interfaces
{
    public interface ICompanyApiClient
    {
        Task<List<CompanyListDTO>> GetCompanyListsAsync();
    }
}
