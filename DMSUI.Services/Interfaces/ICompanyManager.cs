using DMSUI.Entities.DTOs.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services.Interfaces
{
    public interface ICompanyManager
    {
        Task<List<CompanyListDTO>> GetAllCompaniesAsync();
    }
}
