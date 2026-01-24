using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Common;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Instruments;
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
	public class InstrumentApiClient : IInstrumentApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public InstrumentApiClient(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
		{
			_httpContextAccessor = httpContextAccessor;
			_httpClient = httpClient;
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
		public async Task<PagedResultDTO<InstrumentDTO>> GetInstrumentsAsync(int pageNumber, int pageSize)
		{
			AttachToken();
			var response = await _httpClient.GetAsync($"/api/Instrument/get-instruments?page={pageNumber}&pageSize={pageSize}");
			if(!response.IsSuccessStatusCode)
			{
				throw new Exception("Failed to fetch instruments.");
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<PagedResultDTO<InstrumentDTO>>(
				body,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}) ?? new PagedResultDTO<InstrumentDTO>();

		}
	}
}
