using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Calibration;
using DMSUI.Entities.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DMSUI.Business
{
	public class CalibrationApiClient : ICalibrationApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CalibrationApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
		public async Task<PagedResultDTO<CalibrationItemDTO>> GetCalibrationItemsAsync(int pageNumber, int pageSize)
		{
			AttachToken();
			var response = await _httpClient.GetAsync($"api/InstrumentCalibrations?page={pageNumber}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
			{
				return new PagedResultDTO<CalibrationItemDTO>();
			}
			var body = await response.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<PagedResultDTO<CalibrationItemDTO>>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new PagedResultDTO<CalibrationItemDTO>();
		}

		public async Task<CalibrationItemDTO> GetCalibrationItemByIdAsync(int calibrationId)
		{
			AttachToken();
			var respoonse = await _httpClient.GetAsync($"api/InstrumentCalibrations/{calibrationId}");
			if (!respoonse.IsSuccessStatusCode)
			{
				return new CalibrationItemDTO();
			}
			var body = await respoonse.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<CalibrationItemDTO>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new CalibrationItemDTO();
		}

		public async Task<bool> DeleteByIdAsync(int calibrationId)
		{
			AttachToken();
			var response = await _httpClient.DeleteAsync($"api/InstrumentCalibrations/{calibrationId}");
			return response.IsSuccessStatusCode;
		}

		public async Task<bool> UpdateCalibrationAsync(int id, EditCalibrationDTO editCalibrationDTO)
		{
			AttachToken();
			var response = await _httpClient.PutAsJsonAsync($"api/InstrumentCalibrations/{id}", editCalibrationDTO);

			var responseText = await response.Content.ReadAsStringAsync();
			Console.WriteLine($"UpdateCalibration Status: {(int)response.StatusCode} {response.ReasonPhrase}");
			Console.WriteLine($"ResponseBody: {responseText}");

			if (!response.IsSuccessStatusCode)
				return false;

			return true;
		}

		public async Task<ulong?> CreateCalibrationAsync(CreateCalibrationDTO createCalibrationDTO)
		{
			AttachToken();
			var result = await _httpClient.PostAsJsonAsync("api/InstrumentCalibrations", createCalibrationDTO);
			if(!result.IsSuccessStatusCode)
			{
				return null;
			}
			var created = await result.Content.ReadFromJsonAsync<CalibrationItemDTO>();
			return (ulong?)(created?.CalibrationId);
		}
	}
}
