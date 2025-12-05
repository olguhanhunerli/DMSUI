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
		Task<CompanyListDTO> GetCompanyListById(int id);
		Task<bool> UpdateCompanyAsync(int ıd, CompanyUpdateDTO dto);
	}
}
