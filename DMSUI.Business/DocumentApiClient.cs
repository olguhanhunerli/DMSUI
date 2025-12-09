using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Document;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class DocumentApiClient : IDocumentApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<PagedResultDTO<DocumentListDTO>> GetPagedAsync(int page, int pageSize)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Document/get-paged?page={page}&pageSize={pageSize}");
            if (!response.IsSuccessStatusCode)
            {
                return new PagedResultDTO<DocumentListDTO>();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PagedResultDTO<DocumentListDTO>>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new PagedResultDTO<DocumentListDTO>();
        }

		public async Task<DocumentCreatePreviewDTO> GetDocumentCreatePreview(int categoryId)
		{
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Document/create-preview?categoryId={categoryId}");
            if (!response.IsSuccessStatusCode)
            {
                return new DocumentCreatePreviewDTO();
            }
            var body = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<DocumentCreatePreviewDTO>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		}
	}
}
