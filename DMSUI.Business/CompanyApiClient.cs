using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Company;
using DMSUI.Entities.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class CompanyApiClient : ICompanyApiClient
    {
        private readonly HttpClient _httpClient;

        public CompanyApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CompanyListDTO>> GetCompanyListsAsync()
        {
            var response = await _httpClient.GetAsync("api/Company/GetAllCompanies");
            if (response == null)
            {
                return new List<CompanyListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<List<CompanyListDTO>>(
                body,
                new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<CompanyListDTO>();
        }
    }
}
