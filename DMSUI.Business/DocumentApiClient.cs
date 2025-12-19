using DMSUI.Business.Interfaces;
using DMSUI.Controllers;
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
using static System.Net.WebRequestMethods;

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
            var response = await _httpClient.GetAsync($"api/Document/approved?page={page}&pageSize={pageSize}");
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

        public async Task<CreateDocumentResponseDTO> CreateAsync(CreateDocumentDTO dto)
        {
            AttachToken();

            using var form = new MultipartFormDataContent();

            form.Add(new StringContent(dto.TitleTr ?? ""), "TitleTr");
            form.Add(new StringContent(dto.TitleEn ?? ""), "TitleEn");
            form.Add(new StringContent(dto.CategoryId.ToString()), "CategoryId");
            form.Add(new StringContent(dto.DepartmentId.ToString()), "DepartmentId");
            form.Add(new StringContent(dto.RevisionNumber.ToString()), "RevisionNumber");
            form.Add(new StringContent(dto.IsPublic.ToString()), "IsPublic");

            foreach (var id in dto.ApproverUserIds)
                form.Add(new StringContent(id.ToString()), "ApproverUserIds");

            if (dto.MainFile != null)
            {
                var main = new StreamContent(dto.MainFile.OpenReadStream());
                main.Headers.ContentType = new MediaTypeHeaderValue(dto.MainFile.ContentType);
                form.Add(main, "MainFile", dto.MainFile.FileName);
            }

            if (dto.Attachments != null)
            {
                foreach (var file in dto.Attachments)
                {
                    var stream = new StreamContent(file.OpenReadStream());
                    stream.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    form.Add(stream, "Attachments", file.FileName);
                }
            }

            var response = await _httpClient.PostAsync("api/Document/create", form);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CreateDocumentResponseDTO>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
        }

		public async Task<DocumentDetailDTO> GetByIdAsync(int documentId)
		{
            AttachToken();
			var response = await _httpClient.GetAsync($"api/Document/{documentId}");
			if (!response.IsSuccessStatusCode)
			{
				return new DocumentDetailDTO();
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<DocumentDetailDTO>(body,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
		}

		public async Task<PdfFileResultDTO?> GetPdfAsync(int documentId)
		{
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Document/{documentId}/pdf");
            if(!response.IsSuccessStatusCode)
			{
				return null;
			}
            var bytes = await response.Content.ReadAsByteArrayAsync();
			var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? "document.pdf";
			return new PdfFileResultDTO
			{
				FileName = fileName,
				FileBytes = bytes
			};
		}
	}
}
