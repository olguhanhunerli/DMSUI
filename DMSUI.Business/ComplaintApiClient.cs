using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Complaints;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DMSUI.Business
{
    public class ComplaintApiClient : IComplaintApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ComplaintApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
		public async Task<PagedResultDTO<ComplaintItemsDTO>> GetComplaintsPaging(int page, int pageSize)
		{
			AttachToken();
			var response = await _httpClient.GetAsync(
				$"api/Complaint?pageNumber={page}&pageSize={pageSize}"
			);
			if (!response.IsSuccessStatusCode)
			{
				return new PagedResultDTO<ComplaintItemsDTO>();
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<PagedResultDTO<ComplaintItemsDTO>>(
				body,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}
			) ?? new PagedResultDTO<ComplaintItemsDTO>();
		}

		public async Task<ComplaintItemsDTO> CreateComplaint(CreateComplaintDTO complaint)
		{
			AttachToken();
			var response = await _httpClient.PostAsJsonAsync(
				$"api/Complaint",
				complaint
			);
			if(!response.IsSuccessStatusCode)
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<ComplaintItemsDTO>(
				body,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}
			);
		}

		public async Task<ComplaintItemsDTO> GetComplaintById(string complaintNo)
		{
			AttachToken();
			var response = await _httpClient.GetAsync(
				$"api/Complaint/{complaintNo}"
			);
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<ComplaintItemsDTO>(
				body,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}
			);
		}

		public async Task<ComplaintItemsDTO> UpdateComplaint(string complaintNo, UpdateComplaintDTO dto)
		{
			AttachToken();
			var response = await _httpClient.PutAsJsonAsync(
				$"api/Complaint/{complaintNo}",
				dto
			);
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<ComplaintItemsDTO>(
				body,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}
			);
		}

		public async Task<bool> ClosedComplaint(string complaintNo)
		{
			AttachToken();
			var response = await _httpClient.PostAsync($"api/Complaint/close?complaintNo={complaintNo}", null);
			if (!response.IsSuccessStatusCode)
			{
				return false;
			}
			return response.IsSuccessStatusCode;
		}

		public async Task<ComplaintAttachmentMiniDTO> CreateComplaintAttachment(string complaintNo, IFormFile file)
		{
			AttachToken();
			using var form = new MultipartFormDataContent();
			using var stream = file.OpenReadStream();
			var fileContent = new StreamContent(stream);
			fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
			form.Add(fileContent, "file", file.FileName);
			var response = await _httpClient.PostAsync(
				$"api/Complaint/{complaintNo}/attachments",
				form
			);
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<ComplaintAttachmentMiniDTO>(
				body,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}
			);
		}

		public async Task<List<ComplaintAttachmentMiniDTO>> GetComplaintAttachment(string complaintNo)
		{
			AttachToken();
			var response = await _httpClient.GetAsync(
				$"api/Complaint/{complaintNo}/attachments"
			);
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<ComplaintAttachmentMiniDTO>>(
				body,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}
			);
		}

		public async Task<(byte[] FileBytes, string ContentType, string FileName)> DownloadComplaintFilesAsync(int fileId)
		{
			AttachToken();
			var response = await _httpClient.GetAsync(
				$"api/Complaint/attachments/download/{fileId}"
			);
			if (!response.IsSuccessStatusCode)
			{
				return (null, null, null);
			}
			var fileBytes = await response.Content.ReadAsByteArrayAsync();
			var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
			var contentDisposition = response.Content.Headers.ContentDisposition;
			var fileName = contentDisposition != null ? contentDisposition.FileName.Trim('"') : "downloaded_file";
			return (fileBytes, contentType, fileName);
		}

		public async Task<bool> DeleteComplaintAttachment(int fileId)
		{
			AttachToken();
			var response = await _httpClient.DeleteAsync(
				$"api/Complaint/attachments/{fileId}"
			);
			if (!response.IsSuccessStatusCode)
			{
				return false;
			}
			return true;
		}

        public async Task<List<ComplaintForCapaSelectDTO>> GetComplaintForCapaSelect(string? search, int take)
        {
			AttachToken();
			var response = await _httpClient.GetAsync($"api/Complaint/for-capa-select?search={search}&take={take}");
			if (!response.IsSuccessStatusCode) 
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<List<ComplaintForCapaSelectDTO>>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );
        }
    }
}