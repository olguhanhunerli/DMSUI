using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.CalibrationFile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DMSUI.Business
{
	public class CalibrationFileApiClient : ICalibrationFileApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CalibrationFileApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
		public async Task<bool> UploadCalibrationFilesAsync(UploadCalibrationFileDTO dto)
		{
			AttachToken();
			using var form = new MultipartFormDataContent();
			form.Add(new StringContent(dto.CalibrationId.ToString()), "CalibrationId");
			form.Add(new StringContent(dto.InstrumentName ?? ""), "InstrumentName");
			form.Add(new StringContent(dto.FileType ?? ""), "FileType");
			form.Add(new StringContent(dto.Description ?? ""), "Description");

			using var stream = dto.File.OpenReadStream();
			var fileContent = new StreamContent(stream);
			fileContent.Headers.ContentType =
				new MediaTypeHeaderValue(dto.File.ContentType);

			form.Add(fileContent, "File", dto.File.FileName);

			var response = await _httpClient.PostAsync("api/CalibrationFile/upload", form);
			return response.IsSuccessStatusCode;
		}
	}
}
