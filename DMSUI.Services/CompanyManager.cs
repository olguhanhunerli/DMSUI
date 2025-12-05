using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Company;
using DMSUI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Services
{
    public class CompanyManager : ICompanyManager
    {
        private readonly ICompanyApiClient _companyApiClient;

        public CompanyManager(ICompanyApiClient companyApiClient)
        {
            _companyApiClient = companyApiClient;
        }

        public async Task<List<CompanyListDTO>> GetAllCompaniesAsync()
        {
            return await _companyApiClient.GetCompanyListsAsync();
        }

		public async Task<CompanyListDTO> GetCompanyListById(int id)
		{
			return await _companyApiClient.GetCompanyListById(id);
		}

		public async Task<bool> UpdateCompanyAsync(int id, CompanyUpdateDTO company)
		{
			return await _companyApiClient.UpdateCompanyAsync(id, company);
		}
	}
}
