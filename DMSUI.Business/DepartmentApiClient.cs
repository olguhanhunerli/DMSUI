using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Category;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Position;
using DMSUI.Entities.DTOs.Role;
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
	public class DepartmentApiClient : IDepartmentApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public DepartmentApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
		public async Task<List<DepartmentListDTO>> GetAllDepartmentsAsync()
		{
			AttachToken();
			var response = await _httpClient.GetAsync("api/Department/get-all");
			if (!response.IsSuccessStatusCode)
			{
				return new List<DepartmentListDTO>();
			}
			var body = await response.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<List<DepartmentListDTO>>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new List<DepartmentListDTO>();
		}

		public async Task<DepartmentDetailDTO> GetDepartmentsByIdAsync(int id)
		{
			AttachToken();
			var response = await _httpClient.GetAsync($"api/Department/get-by-id/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync() ;
			return JsonSerializer.Deserialize<DepartmentDetailDTO>(
				body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			);
		}

		public async Task<bool> UpdateDepartmentAsync(DepartmentUpdateDTO department)
		{
			AttachToken();
			var response = await _httpClient.PutAsJsonAsync("api/Department/update", department);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> CreateDepartmentAsync(DepartmentCreateDTO department)
		{
			AttachToken();
			var response = await _httpClient.PostAsJsonAsync("api/Department/create", department);
			return response.IsSuccessStatusCode;	
		}

		public async Task<bool> DeleteDepartmentAsync(int id)
		{
			AttachToken();
			var response = await _httpClient.DeleteAsync($"api/Department/delete/{id}");
			return response.IsSuccessStatusCode;	
		}

        public async Task<PagedResultDTO<DepartmentListDTO>> GetPagedAsync(int page, int pageSize)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Department/get-paged?page={page}&pageSize={pageSize}");
            if (!response.IsSuccessStatusCode)
            {
                return new PagedResultDTO<DepartmentListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResultDTO<DepartmentListDTO>>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new PagedResultDTO<DepartmentListDTO>();
        }
    }
}
