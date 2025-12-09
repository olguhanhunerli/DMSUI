using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Category;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Company;
using DMSUI.Entities.DTOs.Departments;
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
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<List<CategoryListDTO>> GetAllCategoriesAsync()
        {
            AttachToken();
            var response = await _httpClient.GetAsync("api/Category/get-all");
            if (!response.IsSuccessStatusCode)
            {
                return new List<CategoryListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CategoryListDTO>>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<CategoryListDTO>();
        }

        public async Task<PagedResultDTO<CategoryListDTO>> GetPagedAsync(int page, int pageSize)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Category/get-paged?page={page}&pageSize={pageSize}");
            if (!response.IsSuccessStatusCode)
            {
                return new PagedResultDTO<CategoryListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResultDTO<CategoryListDTO>>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new PagedResultDTO<CategoryListDTO>();
        }

        public async Task<List<CategoryListDTO>> SearchAsync(string keyword)
        {
            AttachToken();

            var body = JsonSerializer.Serialize(new
            {
                keyword = keyword
            });

            var response = await  _httpClient.PostAsync(
                "api/Category/search",
                new StringContent(body, Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                return new List<CategoryListDTO>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<CategoryListDTO>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? new List<CategoryListDTO>();
        }

        public async Task<List<CategoryTreeDTO>> GetTreeAsync()
        {
            AttachToken();
            var response = await _httpClient.GetAsync("api/Category/get-tree");
            if(!response.IsSuccessStatusCode)
                return new List<CategoryTreeDTO>();
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CategoryTreeDTO>>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true}) ?? new List<CategoryTreeDTO>();
        }

        public async Task<bool> CreateCategoryAsync(CreateCategoryDTO dto)
        {
            AttachToken();
            var response = await _httpClient.PostAsJsonAsync("api/Category/create", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            AttachToken();
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/Category/delete")
            {
                Content = JsonContent.Create(new { id })
            };

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

		public async Task<CategoryDetailDTO> GetCategoryByIdAsync(int id)
		{
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Category/get-by-id/{id}");
			if (!response.IsSuccessStatusCode)
                return new CategoryDetailDTO();
            var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<CategoryDetailDTO>(
				 body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			 );
		}

        public async Task<bool> UpdateCategoryAsync(CategoryUpdateDTO dto)
        {
            AttachToken();
            var response = await _httpClient.PutAsJsonAsync("api/Category/update", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RestoreCategoryAsync(CategoryRestoreDTO dto)
        {
            AttachToken();
            var response = await _httpClient.PutAsJsonAsync("api/Category/restore", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
