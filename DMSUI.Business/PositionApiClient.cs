using DMSUI.Business.Interfaces;
using DMSUI.Entities.DTOs.Departments;
using DMSUI.Entities.DTOs.Position;
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
	public class PositionApiClient : IPositionApiClient
	{
		private readonly HttpClient _httpClient;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public PositionApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

		public async Task<List<PositionListDTO>> GetAllPositionsAsync()
		{
			AttachToken();
			var response = await _httpClient.GetAsync("api/Position/get-all");
			if (!response.IsSuccessStatusCode)
			{
				return new List<PositionListDTO>();
			}
			var body = await response.Content.ReadAsStringAsync();
			return System.Text.Json.JsonSerializer.Deserialize<List<PositionListDTO>>(
				body,
				new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			) ?? new List<PositionListDTO>();

		}

		public async Task<bool> AddPositionAsync(PositionCreateDTO positionCreateDTO)
		{
			AttachToken();
			var response = await _httpClient.PostAsJsonAsync("api/Position/create", positionCreateDTO);
			return response.IsSuccessStatusCode;
		}

		public async Task<PositionDetailDTO> GetPositionByIdAsync(int id)
		{
			AttachToken();
			var response = await _httpClient.GetAsync($"api/Position/get-by-id/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			var body = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<PositionDetailDTO>(
				body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
			);
		}
	}
}
