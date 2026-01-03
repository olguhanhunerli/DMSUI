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
        public async Task<PagedResultDTO<DocumentListDTO>> GetPagedAsync(int page, int pageSize, int roleId, int departmentId)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Document/approved?roleId={roleId}&departmentId={departmentId}&page={page}&pageSize={pageSize}");
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
            form.Add(new StringContent(dto.VersionNote.ToString()), "VersionNote");
            form.Add(new StringContent(dto.DocumentCode.ToString()), "DocumentCode");
            if (dto.AllowedDepartmentIds != null && dto.AllowedDepartmentIds.Any())
            {
                foreach (var depId in dto.AllowedDepartmentIds)
                {
                    form.Add(new StringContent(depId.ToString()), "AllowedDepartmentIds");
                }
            }
            if (dto.AllowedRoleIds != null && dto.AllowedRoleIds.Any())
            {
                foreach (var depId in dto.AllowedRoleIds)
                {
                    form.Add(new StringContent(depId.ToString()), "AllowedRoleIds");
                }
            }
            if (dto.AllowedUserIds != null && dto.AllowedUserIds.Any())
            {
                foreach (var depId in dto.AllowedUserIds)
                {
                    form.Add(new StringContent(depId.ToString()), "AllowedUserIds");
                }
            }
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

		public async Task<PagedResultDTO<DocumentListDTO>> GetPagedRejectAsync(int page, int pageSize)
		{
            AttachToken();
			var response = await _httpClient.GetAsync($"api/Document/rejected?page={page}&pageSize={pageSize}");
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

		public async Task<DownloadFileResult> DownloadOriginalAsync(int documentId)
		{
            AttachToken();
			var response = await _httpClient.GetAsync(
		                                                $"api/Document/download/{documentId}",
		                                                HttpCompletionOption.ResponseHeadersRead
	                                                );

			response.EnsureSuccessStatusCode();

			return new DownloadFileResult
			{
				Stream = await response.Content.ReadAsStreamAsync(),
				ContentType = response.Content.Headers.ContentType?.ToString()
							  ?? "application/octet-stream",
				FileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
						   ?? $"document_{documentId}"
			};
		}

		public async Task<DownloadFileResult> DownloadPdfAsync(int documentId)
		{
            AttachToken();
			var response = await _httpClient.GetAsync(
		        $"api/Document/download-pdf/{documentId}",
		        HttpCompletionOption.ResponseHeadersRead
	        );

			response.EnsureSuccessStatusCode();

			return new DownloadFileResult
			{
				Stream = await response.Content.ReadAsStreamAsync(),
				ContentType = "application/pdf",
				FileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
						   ?? $"document_{documentId}.pdf"
			};
		}

		public async Task<PagedResultDTO<DocumentListDTO>> GetDocumentsByCategoryAsync(int page, int pageSize, int categoryId)
		{
            AttachToken();
            var response = await _httpClient.GetAsync($"api/Document/get-paged-by-category?categoryId={categoryId}&page={page}&pageSize={pageSize}");
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
	}
}
