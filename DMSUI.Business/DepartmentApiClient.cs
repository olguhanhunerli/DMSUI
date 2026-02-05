using DMSUI.Business.Exceptions;
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
            return (await response.ReadAsAsync<List<DepartmentListDTO>>()) ?? new List<DepartmentListDTO>();
        }

        public async Task<DepartmentDetailDTO?> GetDepartmentsByIdAsync(int id)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Department/get-by-id/{id}");
            return await response.ReadAsAsync<DepartmentDetailDTO>();
        }

        public async Task<bool> UpdateDepartmentAsync(DepartmentUpdateDTO department)
        {
            AttachToken();
            var response = await _httpClient.PutAsJsonAsync("api/Department/update", department);
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<bool> CreateDepartmentAsync(DepartmentCreateDTO department)
        {
            AttachToken();
            var response = await _httpClient.PostAsJsonAsync("api/Department/create", department);
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            AttachToken();
            var response = await _httpClient.DeleteAsync($"api/Department/delete/{id}");
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<PagedResultDTO<DepartmentListDTO>> GetPagedAsync(int page, int pageSize)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Department/get-paged?page={page}&pageSize={pageSize}");
            return (await response.ReadAsAsync<PagedResultDTO<DepartmentListDTO>>()) ?? new PagedResultDTO<DepartmentListDTO>();
        }
    }
