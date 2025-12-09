using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Company;
using DMSUI.Entities.DTOs.Departments;
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
	public class RoleApiClient : IRoleApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;



		public RoleApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
		public async Task<RoleListDTO> GetRoleByIdAsync(int id)
		{
			AttachToken();
			var response = await _httpClient.GetAsync($"/api/Role/get-by-id/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<RoleListDTO>(
				body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			);
		}

		public async Task<List<RoleListDTO>> GetRoleListsAsync()
		{
			AttachToken();
			var response = await _httpClient.GetAsync("api/Role/get-all");
			if (!response.IsSuccessStatusCode)
			{
				return new List<RoleListDTO>();
			}
			var body = await response.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<List<RoleListDTO>>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new List<RoleListDTO>();
		}

		public async Task<bool> UpdateRoleAsync(int id, RoleUpdateDTO roleUpdateDTO)
		{
			AttachToken();
			var response = await _httpClient.PutAsJsonAsync($"api/Role/update/{id}", roleUpdateDTO);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> CreateRoleAsync(RoleCreateDTO roleCreateDTO)
		{
			AttachToken();
			var response = await _httpClient.PostAsJsonAsync("api/Role/create", roleCreateDTO);
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteRoleAsync(int id)
		{
			AttachToken();
			var response = await _httpClient.DeleteAsync($"api/Role/delete/{id}");
			return response.IsSuccessStatusCode;
		}

        public async Task<PagedResultDTO<RoleListDTO>> GetPagedAsync(int page, int pageSize)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Role/get-paged?page={page}&pageSize={pageSize}");
            if (!response.IsSuccessStatusCode)
            {
                return new PagedResultDTO<RoleListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResultDTO<RoleListDTO>>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new PagedResultDTO<RoleListDTO>();
        }
    }
}
