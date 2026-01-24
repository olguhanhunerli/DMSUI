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

		public async Task<InstrumentDTO> GetInstrumentByIdAsync(int instrumentId)
		{
			AttachToken();
			var response = await _httpClient.GetAsync($"/api/Instrument/get-instrument-by-id/{instrumentId}");
			response.EnsureSuccessStatusCode();

			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<InstrumentDTO>(body, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			}) ?? new InstrumentDTO();

		}

		public async Task<PagedResultDTO<InstrumentDTO>> GetDeletedByInstrumentsAsync(int pageNumber, int pageSize)
		{
			AttachToken();
			var response = await _httpClient.GetAsync($"/api/Instrument/get-instruments-deleted?page={pageNumber}&pageSize={pageSize}");
			if (!response.IsSuccessStatusCode)
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

		public Task<InstrumentDTO> GetInstrumentDeletedByIdAsync(int instrumentId)
		{
			AttachToken();
			var response = _httpClient.GetAsync($"/api/Instrument/get-instrument-by-deleted-id/{instrumentId}");
			if (!response.Result.IsSuccessStatusCode)
			{
				throw new Exception("Failed to fetch instrument.");
			}
			var body = response.Result.Content.ReadAsStringAsync();
			return Task.FromResult(JsonSerializer.Deserialize<InstrumentDTO>(
				body.Result,
				new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				}) ?? new InstrumentDTO());
		}

		public async Task CreateInstrumentAsync(CreateInstrumentDTO dto)
		{
			AttachToken();
			
			var response = await _httpClient.PostAsJsonAsync($"/api/Instrument/create-instrument", dto);
			var body = await response.Content.ReadAsStringAsync();
			Console.WriteLine(dto);
			Console.WriteLine("Create status: " + (int)response.StatusCode);
			Console.WriteLine("Create body: " + body);
			response.EnsureSuccessStatusCode();
			
		}

		public async Task UpdateInstrumentAsync(int id,UpdateInstrumentDTO dto)
		{
			AttachToken();

			var response = await _httpClient.PutAsJsonAsync($"/api/Instrument/update-instrument/{id}", dto);
			response.EnsureSuccessStatusCode();
		}

		public async Task DeleteInstrumentAsync(int id)
		{
			AttachToken();
			var response = await _httpClient.DeleteAsync($"/api/Instrument/delete-instrument/{id}");
			if (!response.IsSuccessStatusCode)
			{
				throw new Exception("Failed to delete instrument.");
			}
			response.EnsureSuccessStatusCode();
		}

		public async Task RollBackInstrumentAsync(int id)
		{
			AttachToken();
			var response = await _httpClient.PostAsync($"/api/Instrument/backup-delete-instrument/{id}", null);
			if (!response.IsSuccessStatusCode)
			{
				throw new Exception("Failed to rollback instrument.");
			}
			response.EnsureSuccessStatusCode();
		}

		public async Task ToggleInstrumentActiveAsync(int id, bool isActive)
		{
			AttachToken();
			var request = new HttpRequestMessage(
				HttpMethod.Patch,
				$"/api/Instrument/toggle-instrument-active/{id}?isActive={isActive}"
			);

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
		}
	}
}
