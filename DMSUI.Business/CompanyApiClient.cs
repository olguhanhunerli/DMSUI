using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Company;
using DMSUI.Entities.DTOs.Role;
using DMSUI.Entities.DTOs.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class CompanyApiClient : ICompanyApiClient
    {
        private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CompanyApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
		{
			_httpClient = httpClient;
			_httpContextAccessor = httpContextAccessor;
		}
		private void AttachToken()
		{
			var token = _httpContextAccessor.HttpContext?
				.Request.Cookies["access_token"];

			_httpClient.DefaultRequestHeaders.Remove("Authorization");

			if (!string.IsNullOrWhiteSpace(token))
			{
				_httpClient.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", token);
			}
		}

		public async Task<CompanyListDTO> GetCompanyListById(int id)
		{
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Company/GetCompanyById/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<CompanyListDTO>(
				body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			);
		}

		public async Task<List<CompanyListDTO>> GetCompanyListsAsync()
        {
            var response = await _httpClient.GetAsync("api/Company/GetAllCompanies");
            if (response == null)
            {
                return new List<CompanyListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CompanyListDTO>>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<CompanyListDTO>();
        }

		public async Task<bool> UpdateCompanyAsync(int id, CompanyUpdateDTO company)
		{
			AttachToken();
			var response = await _httpClient.PutAsJsonAsync($"api/Company/UpdateCompany/{id}", company);
			return response.IsSuccessStatusCode;

		}
	}
}
