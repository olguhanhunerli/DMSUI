using DMSUI.Business.Exceptions;
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
            return (await response.ReadAsAsync<PagedResultDTO<CalibrationItemDTO>>()) ?? new PagedResultDTO<CalibrationItemDTO>();
        }

        public async Task<CalibrationItemDTO?> GetCalibrationItemByIdAsync(int calibrationId)
        {
            AttachToken();
            var response = await _httpClient.GetAsync($"api/InstrumentCalibrations/{calibrationId}");
            return await response.ReadAsAsync<CalibrationItemDTO>();
        }

        public async Task<bool> DeleteByIdAsync(int calibrationId)
        {
            AttachToken();
            var response = await _httpClient.DeleteAsync($"api/InstrumentCalibrations/{calibrationId}");
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<bool> UpdateCalibrationAsync(int id, EditCalibrationDTO editCalibrationDTO)
        {
            AttachToken();
            var response = await _httpClient.PutAsJsonAsync($"api/InstrumentCalibrations/{id}", editCalibrationDTO);
            return await response.EnsureSuccessOrThrowAsync();
        }

        public async Task<ulong?> CreateCalibrationAsync(CreateCalibrationDTO createCalibrationDTO)
        {
            AttachToken();
            var response = await _httpClient.PostAsJsonAsync("api/InstrumentCalibrations", createCalibrationDTO);

            response.ThrowIfUnauthorizedOrForbidden();
            if (!response.IsSuccessStatusCode) return null;

            var created = await response.Content.ReadFromJsonAsync<CalibrationItemDTO>();
            return (ulong?)(created?.CalibrationId);
        }
    }
}
